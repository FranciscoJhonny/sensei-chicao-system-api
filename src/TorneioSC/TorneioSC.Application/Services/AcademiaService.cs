using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using TorneioSC.Application.Services.Util;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios de Academias.
    /// Coordena operações de leitura e escrita, validações, logs e tratamento de erros.
    /// </summary>
    public class AcademiaService : IAcademiaService
    {
        private readonly IAcademiaSqlReadAdapter _academiaSqlReadAdapter;
        private readonly IAcademiaSqlWriteAdapter _academiaSqlWriteAdapter;
        private readonly ILogger<AcademiaService> _logger;

        /// <summary>
        /// Inicializa uma nova instância do serviço de academias.
        /// </summary>
        /// <param name="academiaSqlReadAdapter">Adaptador de leitura para operações de consulta.</param>
        /// <param name="academiaSqlWriteAdapter">Adaptador de escrita para operações de criação e atualização.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public AcademiaService(
            IAcademiaSqlReadAdapter academiaSqlReadAdapter,
            IAcademiaSqlWriteAdapter academiaSqlWriteAdapter,
            ILogger<AcademiaService> logger)
        {
            _academiaSqlReadAdapter = academiaSqlReadAdapter ?? throw new ArgumentNullException(nameof(academiaSqlReadAdapter));
            _academiaSqlWriteAdapter = academiaSqlWriteAdapter ?? throw new ArgumentNullException(nameof(academiaSqlWriteAdapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Cria uma nova academia no sistema após validação dos dados.
        /// </summary>
        /// <param name="academia">Objeto contendo os dados da nova academia.</param>
        /// <returns>ID da academia criada.</returns>
        /// <exception cref="ArgumentException">Lançada se os dados forem inválidos.</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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
        /// Atualiza os dados de uma academia existente.
        /// </summary>
        /// <param name="academia">Objeto contendo os dados atualizados da academia.</param>
        /// <returns>Número de linhas afetadas (geralmente 1 se sucesso).</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for inválido ou dados inválidos.</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<int> PutAcademiaAsync(Academia academia)
        {
            _logger.LogInformation("Atualizando academia ID: {AcademiaId}", academia.AcademiaId);

            try
            {
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

                var linhasAfetadas = await _academiaSqlWriteAdapter.PutAcademiaAsync(academia);

                if (linhasAfetadas > 0)
                    _logger.LogInformation("Academia ID {AcademiaId} atualizada com sucesso", academia.AcademiaId);
                else
                    _logger.LogWarning("Academia ID {AcademiaId} não encontrada", academia.AcademiaId);

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
        /// Inativa uma academia logicamente (exclusão suave).
        /// </summary>
        /// <param name="academiaId">ID da academia a ser inativada.</param>
        /// <param name="usuarioOperacaoId">ID do usuário que está realizando a operação.</param>
        /// <returns>True se inativada com sucesso; caso contrário, false.</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for inválido.</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<bool> InativarAcademiaPorIdAsync(int academiaId, int usuarioOperacaoId)
        {
            _logger.LogInformation("Inativando academia ID: {AcademiaId}", academiaId);

            try
            {
                if (academiaId <= 0)
                {
                    _logger.LogWarning("ID de academia inválido para inativação: {AcademiaId}", academiaId);
                    throw new ArgumentException("ID da academia deve ser maior que zero", nameof(academiaId));
                }

                var inativado = await _academiaSqlWriteAdapter.InativarAcademiaPorIdAsync(academiaId, usuarioOperacaoId);

                if (inativado)
                    _logger.LogInformation("Academia ID {AcademiaId} inativada com sucesso", academiaId);
                else
                    _logger.LogWarning("Academia ID {AcademiaId} não encontrada", academiaId);

                return inativado;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                      ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao inativar academia ID: {AcademiaId}", academiaId);
                throw new OperacaoAcademiaException("inativação", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao inativar academia ID: {AcademiaId}", academiaId);
                throw;
            }
        }

        #endregion

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém uma academia pelo seu ID.
        /// </summary>
        /// <param name="academiaId">ID da academia.</param>
        /// <returns>Academia encontrada ou null se não existir.</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for inválido.</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<Academia?> ObterAcademiaPorIdAsync(int academiaId)
        {
            _logger.LogInformation("Buscando academia por ID: {AcademiaId}", academiaId);

            try
            {
                if (academiaId <= 0)
                {
                    _logger.LogWarning("ID de academia inválido: {AcademiaId}", academiaId);
                    throw new ArgumentException("ID da academia deve ser maior que zero", nameof(academiaId));
                }

                var academia = await _academiaSqlReadAdapter.ObterAcademiaPorIdAsync(academiaId);

                if (academia == null)
                {
                    _logger.LogInformation("Academia não encontrada para o ID: {AcademiaId}", academiaId);
                    return null;
                }

                _logger.LogInformation("Academia encontrada: {Nome} (ID: {AcademiaId})", academia.Nome, academiaId);

                return academia;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar academia por ID: {AcademiaId}", academiaId);
                throw new OperacaoAcademiaException($"buscar academia com ID {academiaId}", ex);
            }
            catch ( System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar academia por ID: {AcademiaId}", academiaId);
                throw;
            }
        }

        /// <summary>
        /// Obtém todas as academias ativas do sistema.
        /// </summary>
        /// <returns>Lista de todas as academias ativas.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<IEnumerable<Academia>> ObterAcademiasAsync()
        {
            _logger.LogInformation("Buscando todas as academias");

            try
            {
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

        /// <summary>
        /// Obtém uma lista paginada de academias com base em filtros (nome, CNPJ, município, federação, ativo).
        /// </summary>
        /// <param name="filtro">Objeto contendo critérios de busca e paginação.</param>
        /// <returns>Tupla contendo a lista de academias e o total de registros.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<(IEnumerable<Academia> Academias, int Total)> ObterAcademiasPorFiltroAsync(FiltroAcademia filtro)
        {
            _logger.LogInformation("Buscando academias por filtro: {@Filtro}", filtro);

            try
            {
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

        /// <summary>
        /// Obtém estatísticas gerais das academias (total, ativas, inativas, cidades, federações).
        /// </summary>
        /// <returns>Objeto com as estatísticas detalhadas.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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

        /// <summary>
        /// Obtém uma academia pelo CNPJ (somente ativas).
        /// </summary>
        /// <param name="cnpj">CNPJ da academia (com ou sem máscara).</param>
        /// <returns>Academia encontrada ou null se não existir.</returns>
        /// <exception cref="ArgumentException">Lançada se o CNPJ for nulo ou vazio.</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<Academia?> ObterPorCnpjAsync(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ não pode ser vazio", nameof(cnpj));

            return await _academiaSqlReadAdapter.ObterPorCnpjAsync(Recursos.RemoverMascaraCNPJ(cnpj.Trim()));
        }

        /// <summary>
        /// Verifica se um CNPJ já está em uso por outra academia (usado em atualizações).
        /// </summary>
        /// <param name="cnpj">CNPJ a ser verificado.</param>
        /// <param name="academiaId">ID da academia sendo editada (excluída da verificação).</param>
        /// <returns>Academia encontrada com o CNPJ ou null se disponível.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<Academia?> ObterPorCnpjUpdateAsync(string cnpj, int academiaId)
        {
            try
            {
                return await _academiaSqlReadAdapter.ObterPorCnpjUpdateAsync(Recursos.RemoverMascaraCNPJ(cnpj), academiaId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar CNPJ duplicado para academia {AcademiaId}", academiaId);
                throw new OperacaoAcademiaException("verificação de CNPJ único", ex);
            }
        }

        /// <summary>
        /// Obtém um resumo básico de todas as academias (ID, Nome, CNPJ, Cidade, Ativo).
        /// </summary>
        /// <returns>Lista de resumos das academias ativas.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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

        /// <summary>
        /// Obtém um resumo de academias filtradas (por nome, CNPJ, município, federação).
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis.</param>
        /// <returns>Lista de resumos das academias que atendem aos critérios.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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

        /// <summary>
        /// Obtém um resumo de academias com paginação.
        /// </summary>
        /// <param name="pagina">Página atual (padrão: 1).</param>
        /// <param name="tamanhoPagina">Quantidade de registros por página (padrão: 10, máx: 100).</param>
        /// <returns>Tupla contendo os resumos e o total de registros.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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

        /// <summary>
        /// Obtém o total de academias ativas com base em filtros opcionais.
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis (nome, CNPJ, município, federação, ativo).</param>
        /// <returns>Número total de academias que atendem aos critérios.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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

        /// <summary>
        /// Obtém o total de academias ativas no sistema.
        /// </summary>
        /// <returns>Quantidade de academias ativas.</returns>
        /// <exception cref="OperacaoAcademiaException">Lançada em caso de erro no banco de dados.</exception>
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

        #endregion

        #region 🔽 Validações

        /// <summary>
        /// Valida os dados de uma academia antes de persistir.
        /// </summary>
        /// <param name="academia">Objeto a ser validado.</param>
        /// <returns>Lista de mensagens de erro. Vazio se válido.</returns>
        private List<string> ValidarAcademia(Academia academia)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(academia.Nome))
                erros.Add("Nome é obrigatório");
            else if (academia.Nome.Length > 150)
                erros.Add("Nome não pode exceder 150 caracteres");

            if (string.IsNullOrWhiteSpace(academia.Email))
                erros.Add("Email é obrigatório");
            else if (!IsValidEmail(academia.Email))
                erros.Add("Email em formato inválido");

            if (academia.Telefones == null || !academia.Telefones.Any())
            {
                erros.Add("Pelo menos um telefone é obrigatório");
            }
            else
            {
                int index = 1;
                foreach (var telefone in academia.Telefones)
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

        /// <summary>
        /// Verifica se um endereço de email é válido.
        /// </summary>
        /// <param name="email">Email a ser validado.</param>
        /// <returns>True se válido; caso contrário, false.</returns>
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

        /// <summary>
        /// Verifica se um número de telefone tem formato válido (10 ou 11 dígitos após limpeza).
        /// </summary>
        /// <param name="telefone">Número de telefone a ser validado.</param>
        /// <returns>True se válido; caso contrário, false.</returns>
        private bool IsValidTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            string cleanedTelefone = Recursos.RemoverMascaraTelefone(telefone);
            return cleanedTelefone.Length == 10 || cleanedTelefone.Length == 11;
        }

        #endregion
    }
}