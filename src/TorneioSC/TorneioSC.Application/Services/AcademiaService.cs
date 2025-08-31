using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using TorneioSC.Application.Services.Util;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;
namespace TorneioSC.Application.Services
{
    public class AcademiaService : IAcademiaService
    {
        private readonly IAcademiaSqlReadAdapter _academiaSqlReadAdapter;
        private readonly IAcademiaSqlWriteAdapter _academiaSqlWriteAdapter;
        private readonly ILogger<AcademiaService> _logger;

        public AcademiaService(IAcademiaSqlReadAdapter AcademiaSqlReadAdapter, IAcademiaSqlWriteAdapter AcademiaSqlWriteAdapter, ILogger<AcademiaService> logger)
        {
            _academiaSqlReadAdapter = AcademiaSqlReadAdapter ?? throw new ArgumentNullException(nameof(AcademiaSqlReadAdapter));
            _academiaSqlWriteAdapter = AcademiaSqlWriteAdapter ?? throw new ArgumentNullException(nameof(AcademiaSqlWriteAdapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Academia?> ObterAcademiaPorIdAsync(int academiaId)
        {
            _logger.LogInformation("Buscando academia por ID: {AcademiaId}", academiaId);

            try
            {
                // Validação básica do ID
                if (academiaId <= 0)
                {
                    _logger.LogWarning("ID de academia inválido: {AcademiaId}", academiaId);
                    throw new ArgumentException("ID da academia deve ser maior que zero", nameof(academiaId));
                }

                // Chama o adaptador de leitura para obter a academia
                var academia = await _academiaSqlReadAdapter.ObterAcademiaPorIdAsync(academiaId);

                if (academia == null)
                {
                    _logger.LogInformation("Academia não encontrada para o ID: {AcademiaId}", academiaId);
                    return null;
                }

                _logger.LogInformation("Academia encontrada: {AcademiaNome} (ID: {AcademiaId})",
                    academia.Nome, academiaId);

                return academia;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar academia por ID: {AcademiaId}", academiaId);
                throw new OperacaoAcademiaException($"buscar academia com ID {academiaId}", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar academia por ID: {AcademiaId}", academiaId);
                throw;
            }
        }
        public async Task<IEnumerable<Academia>> ObterAcademiasAsync()
        {
            _logger.LogInformation("Buscando todas as academias");

            try
            {
                // Chama o adaptador de leitura para obter todas as academias
                var academias = await _academiaSqlReadAdapter.ObterAcademiasAsync();

                _logger.LogInformation("Encontradas {Quantidade} academias", academias?.Count() ?? 0);

                return academias ?? Enumerable.Empty<Academia>();
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                            ex.Message.Contains("database") ||
                                            ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar todas as academias");
                throw new OperacaoAcademiaException("buscar todas as academias", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar todas as academias");
                throw;
            }
        }
        public async Task<(IEnumerable<Academia> Academias, int Total)> ObterAcademiasPorFiltroAsync(FiltroAcademia filtro)
        {
            _logger.LogInformation("Buscando academias por filtro: {@Filtro}", filtro);

            try
            {
                // Validação do filtro
                if (filtro.Pagina <= 0)
                {
                    filtro.Pagina = 1;
                    _logger.LogWarning("Página inválida, usando página 1");
                }

                if (filtro.TamanhoPagina <= 0 || filtro.TamanhoPagina > 100)
                {
                    filtro.TamanhoPagina = 10;
                    _logger.LogWarning("Tamanho de página inválido, usando valor padrão 10");
                }

                filtro.Cnpj = Recursos.RemoverMascaraCNPJ(filtro.Cnpj);

                // Chama o adaptador para obter as academias filtradas
                var resultado = await _academiaSqlReadAdapter.ObterAcademiasPorFiltroAsync(filtro);

                _logger.LogInformation("Encontradas {Total} academias, exibindo {Pagina} de {TotalPaginas}",
                    resultado.Total, filtro.Pagina, Math.Ceiling((double)resultado.Total / filtro.TamanhoPagina));

                return resultado;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                            ex.Message.Contains("database") ||
                                            ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar academias com filtros: {@Filtro}", filtro);
                throw new OperacaoAcademiaException("buscar academias com filtros", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar academias com filtros: {@Filtro}", filtro);
                throw;
            }
        }

        public async Task<EstatisticasAcademias> ObterEstatisticasAcademiasAsync()
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterEstatisticasAcademiasAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter estatísticas das academias");
                throw new OperacaoAcademiaException("obtenção de estatísticas", ex);
            }
        }

        public async Task<Academia?> ObterPorCnpjAsync(string Cnpj)
        {
            if (string.IsNullOrWhiteSpace(Cnpj))
                throw new ArgumentException("CNPJ não pode ser vazio", nameof(Cnpj));

            return await _academiaSqlReadAdapter.ObterPorCnpjAsync(Recursos.RemoverMascaraCNPJ(Cnpj.Trim()));
        }

        public async Task<Academia?> ObterPorCnpjUpdateAsync(string Cnpj, int academiaId)
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterPorCnpjUpdateAsync(Recursos.RemoverMascaraCNPJ(Cnpj), academiaId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar CNPJ duplicado para academia {AcademiaId}", academiaId);
                throw new OperacaoAcademiaException("verificação de CNPJ único", ex);
            }
        }

        public async Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync()
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterResumoAcademiasAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo das academias");
                throw new OperacaoAcademiaException("obtenção do resumo", ex);
            }
        }

        public async Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync(FiltroAcademia filtro)
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterResumoAcademiasAsync(filtro);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo das academias com filtros: {@Filtro}", filtro);
                throw new OperacaoAcademiaException("obtenção do resumo com filtros", ex);
            }
        }

        public async Task<(IEnumerable<AcademiaResumo> Resumos, int Total)> ObterResumoAcademiasPaginadoAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

            try
            {
                return await _academiaSqlReadAdapter.ObterResumoAcademiasPaginadoAsync(pagina, tamanhoPagina);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo paginado das academias (página {Pagina}, tamanho {TamanhoPagina})", pagina, tamanhoPagina);
                throw new OperacaoAcademiaException("obtenção do resumo paginado", ex);
            }
        }

        public async Task<int> ObterTotalAcademiasAsync(FiltroAcademia? filtro = null)
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterTotalAcademiasAsync(filtro);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao contar academias com filtros: {@Filtro}", filtro);
                throw new OperacaoAcademiaException("contagem de academias", ex);
            }
        }

        public async Task<int> ObterTotalAcademiasAtivasAsync()
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterTotalAcademiasAtivasAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao contar academias ativas");
                throw new OperacaoAcademiaException("contagem de academias ativas", ex);
            }
        }

        /// <summary>
        /// Cria uma nova academia no sistema
        /// </summary>
        /// <param name="academia">Objeto Academia contendo os dados para criação</param>
        /// <returns>ID da academia criada</returns>
        /// <exception cref="ArgumentException">Lançada quando os dados da academia são inválidos</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada quando ocorre erro na operação de criação</exception>
        public async Task<int> PostAcademiaAsync(Academia academia)
        {
            _logger.LogInformation("Criando nova academia: {Nome}", academia.Nome);

            try
            {
                // Validação dos dados
                var erros = ValidarAcademia(academia);
                if (erros.Any())
                {
                    _logger.LogWarning("Dados inválidos para criação da academia: {Erros}", string.Join(", ", erros));
                    throw new ArgumentException($"Dados inválidos: {string.Join(", ", erros)}");
                }


                //var novaAcademia = PrepararCriarAcademia(academia);
                // Chama o adaptador para inserir a academia

                var academiaId = await _academiaSqlWriteAdapter.PostAcademiaAsync(academia);

                _logger.LogInformation("Academia criada com sucesso. ID: {AcademiaId}", academiaId);

                return academiaId;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                            ex.Message.Contains("database") ||
                                            ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao criar academia: {Nome}", academia.Nome);
                throw new OperacaoAcademiaException("criação", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar academia: {Nome}", academia.Nome);
                throw;
            }
        }

        /// <summary>
        /// Atualiza os dados de uma academia existente
        /// </summary>
        /// <param name="academia">Objeto Academia contendo os dados atualizados</param>
        /// <returns>Número de registros afetados (1 se atualizado com sucesso, 0 se não encontrado)</returns>
        /// <exception cref="ArgumentException">Lançada quando os dados da academia são inválidos ou ID não informado</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada quando ocorre erro na operação de atualização</exception>
        public async Task<int> PutAcademiaAsync(Academia academia)
        {
            _logger.LogInformation("Atualizando academia ID: {AcademiaId}", academia.AcademiaId);

            try
            {
                // Validação básica
                if (academia.AcademiaId <= 0)
                {
                    _logger.LogWarning("ID de academia inválido para atualização: {AcademiaId}", academia.AcademiaId);
                    throw new ArgumentException("ID da academia é obrigatório para atualização", nameof(academia.AcademiaId));
                }

                var erros = ValidarAcademia(academia);
                if (erros.Any())
                {
                    _logger.LogWarning("Dados inválidos para atualização da academia ID {AcademiaId}: {Erros}",
                        academia.AcademiaId, string.Join(", ", erros));
                    throw new ArgumentException($"Dados inválidos: {string.Join(", ", erros)}");
                }

                // Chama o adaptador para atualizar a academia
                // var academiaAtualizada = PrepararAtualizarAcademia(academia);

                var linhasAfetadas = await _academiaSqlWriteAdapter.PutAcademiaAsync(academia);

                if (linhasAfetadas > 0)
                {
                    _logger.LogInformation("Academia ID {AcademiaId} atualizada com sucesso", academia.AcademiaId);
                }
                else
                {
                    _logger.LogWarning("Academia ID {AcademiaId} não encontrada para atualização", academia.AcademiaId);
                }

                return linhasAfetadas;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                            ex.Message.Contains("database") ||
                                            ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao atualizar academia ID: {AcademiaId}", academia.AcademiaId);
                throw new OperacaoAcademiaException("atualização", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar academia ID: {AcademiaId}", academia.AcademiaId);
                throw;
            }
        }

        /// <summary>
        /// Exclui uma academia do sistema (exclusão lógica ou física)
        /// </summary>
        /// <param name="academiaId">ID da academia a ser excluída</param>
        /// <returns>True se a academia foi excluída com sucesso, False se não foi encontrada</returns>
        /// <exception cref="ArgumentException">Lançada quando o ID da academia é inválido</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada quando ocorre erro na operação de exclusão</exception>
        public async Task<bool> InativarAcademiaPorIdAsync(int academiaId, int usuarioOperacaoId)
        {
            _logger.LogInformation("Excluindo academia ID: {AcademiaId}", academiaId);

            try
            {
                // Validação básica
                if (academiaId <= 0)
                {
                    _logger.LogWarning("ID de academia inválido para exclusão: {AcademiaId}", academiaId);
                    throw new ArgumentException("ID da academia deve ser maior que zero", nameof(academiaId));
                }

                // Chama o adaptador para excluir a academia
                var excluido = await _academiaSqlWriteAdapter.InativarAcademiaPorIdAsync(academiaId, usuarioOperacaoId);

                if (excluido)
                {
                    _logger.LogInformation("Academia ID {AcademiaId} excluída com sucesso", academiaId);
                }
                else
                {
                    _logger.LogWarning("Academia ID {AcademiaId} não encontrada para exclusão", academiaId);
                }

                return excluido;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                            ex.Message.Contains("database") ||
                                            ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao excluir academia ID: {AcademiaId}", academiaId);
                throw new OperacaoAcademiaException("exclusão", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao excluir academia ID: {AcademiaId}", academiaId);
                throw;
            }
        }
        private List<string> ValidarAcademia(Academia Academia)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(Academia.Nome))
                erros.Add("Nome é obrigatório");
            else if (Academia.Nome.Length > 150)
                erros.Add("Nome não pode exceder 150 caracteres");

            //if (string.IsNullOrWhiteSpace(Academia.Cnpj))
            //    erros.Add("CNPJ é obrigatório");
            //else if (!IsValidCNPJ(Academia.Cnpj))
            //    erros.Add("CNPJ em formato inválido");

            if (string.IsNullOrWhiteSpace(Academia.Email))
                erros.Add("Email é obrigatório");
            else if (!IsValidEmail(Academia.Email))
                erros.Add("Email em formato inválido");
            // Validação dos telefones
            if (Academia.Telefones == null || !Academia.Telefones.Any())
            {
                erros.Add("Pelo menos um telefone é obrigatório");
            }
            else
            {
                int index = 1;
                foreach (var telefone in Academia.Telefones)
                {
                    if (string.IsNullOrWhiteSpace(telefone.NumeroTelefone))
                        erros.Add($"Telefone #{index} está vazio");
                    else if (!IsValidTelefone(telefone.NumeroTelefone!))
                        erros.Add($"Telefone #{index} está em formato inválido");

                    index++;
                }
            }

            return erros;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            // Remove tudo que não for dígito
            string cleanedTelefone = Recursos.RemoverMascaraTelefone(telefone);

            // Telefone deve ter 10 (fixo) ou 11 (celular) dígitos com DDD
            return cleanedTelefone.Length == 10 || cleanedTelefone.Length == 11;
        }


    }
}