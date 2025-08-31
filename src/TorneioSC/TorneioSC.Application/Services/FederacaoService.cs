using Microsoft.Extensions.Logging;
using TorneioSC.Application.Services.Util;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionFederacao;

namespace TorneioSC.Application.Services
{
    public class FederacaoService : IFederacaoService
    {
        private readonly IFederacaoSqlReadAdapter _federacaoSqlReadAdapter;
        private readonly IFederacaoSqlWriteAdapter _federacaoSqlWriteAdapter;
        private readonly ILogger<FederacaoService> _logger;

        public FederacaoService(
            IFederacaoSqlReadAdapter federacaoSqlReadAdapter,
            IFederacaoSqlWriteAdapter federacaoSqlWriteAdapter,
            ILogger<FederacaoService> logger)
        {
            _federacaoSqlReadAdapter = federacaoSqlReadAdapter ?? throw new ArgumentNullException(nameof(federacaoSqlReadAdapter));
            _federacaoSqlWriteAdapter = federacaoSqlWriteAdapter ?? throw new ArgumentNullException(nameof(federacaoSqlWriteAdapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region 🔽 Métodos de Escrita
        public async Task<int> PostFederacaoAsync(Federacao federacao)
        {
            _logger.LogInformation("Criando nova federação: {Nome}", federacao.Nome);

            try
            {
                // Validação
                var erros = ValidarFederacao(federacao);
                if (erros.Any())
                {
                    _logger.LogWarning("Dados inválidos para criação da federação: {Erros}", string.Join(", ", erros));
                    throw new ValidacaoFederacaoException(erros);
                }

                // Verifica CNPJ duplicado
                var existente = await _federacaoSqlReadAdapter.ObterPorCnpjAsync(federacao.Cnpj);
                if (existente != null)
                {
                    _logger.LogWarning("CNPJ já em uso: {CNPJ}", federacao.Cnpj);
                    throw new CnpjEmUsoException(federacao.Cnpj);
                }

                var id = await _federacaoSqlWriteAdapter.PostFederacaoAsync(federacao);
                _logger.LogInformation("Federação criada com sucesso. ID: {FederacaoId}", id);
                return id;
            }
            catch (ValidacaoFederacaoException)
            {
                throw;
            }
            catch (CnpjEmUsoException)
            {
                throw;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                      ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao criar federação: {Nome}", federacao.Nome);
                throw new OperacaoFederacaoException("criação", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar federação: {Nome}", federacao.Nome);
                throw;
            }
        }
        public async Task<int> PutFederacaoAsync(Federacao federacao)
        {
            _logger.LogInformation("Atualizando federação ID: {FederacaoId}", federacao.FederacaoId);

            try
            {
                if (federacao.FederacaoId <= 0)
                {
                    _logger.LogWarning("ID de federação inválido para atualização: {FederacaoId}", federacao.FederacaoId);
                    throw new ArgumentException("ID da federação é obrigatório", nameof(federacao.FederacaoId));
                }

                var erros = ValidarFederacao(federacao);
                if (erros.Any())
                {
                    _logger.LogWarning("Dados inválidos para atualização da federação ID {FederacaoId}: {Erros}",
                        federacao.FederacaoId, string.Join(", ", erros));
                    throw new ValidacaoFederacaoException(erros);
                }

                var linhasAfetadas = await _federacaoSqlWriteAdapter.PutFederacaoAsync(federacao);

                if (linhasAfetadas > 0)
                    _logger.LogInformation("Federação ID {FederacaoId} atualizada com sucesso", federacao.FederacaoId);
                else
                    _logger.LogWarning("Federação ID {FederacaoId} não encontrada", federacao.FederacaoId);

                return linhasAfetadas;
            }
            catch (ValidacaoFederacaoException)
            {
                throw;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                      ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao atualizar federação ID: {FederacaoId}", federacao.FederacaoId);
                throw new OperacaoFederacaoException("atualização", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar federação ID: {FederacaoId}", federacao.FederacaoId);
                throw;
            }
        }
        public async Task<bool> InativarFederacaoPorIdAsync(int federacaoId, int usuarioOperacaoId)
        {
            _logger.LogInformation("Inativando federação ID: {FederacaoId}", federacaoId);

            try
            {
                if (federacaoId <= 0)
                {
                    _logger.LogWarning("ID de federação inválido para inativação: {FederacaoId}", federacaoId);
                    throw new ArgumentException("ID da federação deve ser maior que zero", nameof(federacaoId));
                }

                var inativado = await _federacaoSqlWriteAdapter.InativarFederacaoPorIdAsync(federacaoId, usuarioOperacaoId);

                if (inativado)
                    _logger.LogInformation("Federação ID {FederacaoId} inativada com sucesso", federacaoId);
                else
                    _logger.LogWarning("Federação ID {FederacaoId} não encontrada para inativação", federacaoId);

                return inativado;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                        ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao inativar federação ID: {FederacaoId}", federacaoId);
                throw new OperacaoFederacaoException("inativação", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao inativar federação ID: {FederacaoId}", federacaoId);
                throw;
            }
        }
        #endregion

        #region 🔽 Métodos de leitura
        /// <summary>
        /// Obtém federações com base em filtros e paginação
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis (Nome, CNPJ, Município, Estado, Ativo)</param>
        /// <returns>Lista de federações completas e total de registros</returns>
        public async Task<(IEnumerable<Federacao> Federacoes, int Total)> ObterFederacoesPorFiltroAsync(FiltroFederacao filtro)
        {
            _logger.LogInformation("Buscando federações por filtro: {@Filtro}", filtro);

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

                // Limpeza de CNPJ
                if (!string.IsNullOrEmpty(filtro.Cnpj))
                {
                    var cnpjLimpo = Recursos.RemoverMascaraCNPJ(filtro.Cnpj.Trim());
                    if (cnpjLimpo.Length == 14)
                        filtro.Cnpj = cnpjLimpo;
                    else
                        filtro.Cnpj = null; // Invalida se não for 14 dígitos
                }

                // Chama o adaptador para obter os dados
                var resultado = await _federacaoSqlReadAdapter.ObterFederacoesPorFiltroAsync(filtro);

                _logger.LogInformation(
                    "Encontradas {Total} federações, exibindo página {Pagina} de {TotalPaginas}",
                    resultado.Total,
                    filtro.Pagina,
                    Math.Ceiling((double)resultado.Total / filtro.TamanhoPagina));

                return resultado;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                      ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar federações com filtros: {@Filtro}", filtro);
                throw new OperacaoFederacaoException("busca com filtros", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar federações com filtros: {@Filtro}", filtro);
                throw;
            }
        }

        public async Task<IEnumerable<Federacao>> ObterFederacaoAsync()
        {
            _logger.LogInformation("Buscando todas as federações");

            try
            {
                var federacoes = await _federacaoSqlReadAdapter.ObterFederacaoAsync();
                _logger.LogInformation("Encontradas {Quantidade} federações", federacoes?.Count() ?? 0);
                return federacoes ?? Enumerable.Empty<Federacao>();
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                      ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar todas as federações");
                throw new OperacaoFederacaoException("listagem", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar todas as federações");
                throw;
            }
        }

        public async Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId)
        {
            _logger.LogInformation("Buscando federação por ID: {FederacaoId}", federacaoId);

            try
            {
                if (federacaoId <= 0)
                {
                    _logger.LogWarning("ID de federação inválido: {FederacaoId}", federacaoId);
                    throw new ArgumentException("ID da federação deve ser maior que zero", nameof(federacaoId));
                }

                var federacao = await _federacaoSqlReadAdapter.ObterFederacaoPorIdAsync(federacaoId);

                if (federacao == null)
                {
                    _logger.LogInformation("Federação não encontrada para o ID: {FederacaoId}", federacaoId);
                    return null;
                }

                _logger.LogInformation("Federação encontrada: {Nome} (ID: {FederacaoId})", federacao.Nome, federacaoId);
                return federacao;
            }
            catch (System.Exception ex) when (ex.Message.Contains("connection") ||
                                      ex.Message.Contains("database") ||
                                      ex.Message.Contains("sql"))
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar federação por ID: {FederacaoId}", federacaoId);
                throw new OperacaoFederacaoException("busca por ID", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar federação por ID: {FederacaoId}", federacaoId);
                throw;
            }
        }

        public async Task<Federacao?> ObterPorCnpjAsync(string cnpj)
        {
            _logger.LogInformation("Buscando federação por CNPJ");

            try
            {
                if (string.IsNullOrWhiteSpace(cnpj))
                {
                    _logger.LogWarning("CNPJ nulo ou vazio fornecido");
                    return null;
                }

                var cnpjLimpo = Recursos.RemoverMascaraCNPJ(cnpj.Trim());
                if (cnpjLimpo.Length != 14)
                {
                    _logger.LogWarning("CNPJ inválido (tamanho incorreto): {CNPJ}", cnpj);
                    return null;
                }

                var federacao = await _federacaoSqlReadAdapter.ObterPorCnpjAsync(cnpjLimpo);
                if (federacao != null)
                    _logger.LogInformation("Federação encontrada com CNPJ: {CNPJ}", cnpjLimpo);

                return federacao;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar federação por CNPJ: {CNPJ}", cnpj);
                throw new OperacaoFederacaoException("busca por CNPJ", ex);
            }
        }

        public async Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId)
        {
            _logger.LogInformation("Verificando CNPJ duplicado para atualização da federação ID: {FederacaoId}", federacaoId);

            try
            {
                if (string.IsNullOrWhiteSpace(cnpj))
                    return null;

                var cnpjLimpo = Recursos.RemoverMascaraCNPJ(cnpj.Trim());
                if (cnpjLimpo.Length != 14)
                    return null;

                return await _federacaoSqlReadAdapter.ObterPorCnpjUpdateAsync(cnpjLimpo, federacaoId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar CNPJ duplicado para federação {FederacaoId}", federacaoId);
                throw new OperacaoFederacaoException("verificação de CNPJ único", ex);
            }
        }

        public async Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync()
        {
            try
            {
                return await _federacaoSqlReadAdapter.ObterResumoFederacoesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo das federações");
                throw new OperacaoFederacaoException("obtenção do resumo", ex);
            }
        }

        public async Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync(FiltroFederacao filtro)
        {
            try
            {
                return await _federacaoSqlReadAdapter.ObterResumoFederacoesAsync(filtro);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo das federações com filtros: {@Filtro}", filtro);
                throw new OperacaoFederacaoException("obtenção do resumo com filtros", ex);
            }
        }

        public async Task<(IEnumerable<FederacaoResumo> Resumos, int Total)> ObterResumoFederacoesPaginadoAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

            try
            {
                return await _federacaoSqlReadAdapter.ObterResumoFederacoesPaginadoAsync(pagina, tamanhoPagina);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo paginado das federações (página {Pagina}, tamanho {TamanhoPagina})", pagina, tamanhoPagina);
                throw new OperacaoFederacaoException("obtenção do resumo paginado", ex);
            }
        }

        public async Task<int> ObterTotalFederacoesAsync(FiltroFederacao? filtro = null)
        {
            try
            {
                return await _federacaoSqlReadAdapter.ObterTotalFederacoesAsync(filtro);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao contar federações com filtros: {@Filtro}", filtro);
                throw new OperacaoFederacaoException("contagem de federações", ex);
            }
        }

        public async Task<EstatisticasFederacoes> ObterEstatisticasFederacoesAsync()
        {
            try
            {
                return await _federacaoSqlReadAdapter.ObterEstatisticasFederacoesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter estatísticas das federações");
                throw new OperacaoFederacaoException("obtenção de estatísticas", ex);
            }
        }

        #endregion

        #region 🔽 Validações

        private List<string> ValidarFederacao(Federacao federacao)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(federacao.Nome))
                erros.Add("Nome é obrigatório");
            else if (federacao.Nome.Length > 150)
                erros.Add("Nome não pode exceder 150 caracteres");

            if (string.IsNullOrWhiteSpace(federacao.Cnpj))
                erros.Add("CNPJ é obrigatório");
            else
            {
                var cnpjLimpo = Recursos.RemoverMascaraCNPJ(federacao.Cnpj);
                if (cnpjLimpo.Length != 14)
                    erros.Add("CNPJ deve conter 14 dígitos");
            }

            if (string.IsNullOrWhiteSpace(federacao.Email))
                erros.Add("Email é obrigatório");
            else if (!IsValidEmail(federacao.Email))
                erros.Add("Email em formato inválido");

            if (federacao.Telefones == null || !federacao.Telefones.Any())
            {
                erros.Add("Pelo menos um telefone é obrigatório");
            }
            else
            {
                int index = 1;
                foreach (var telefone in federacao.Telefones)
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

        private bool IsValidEmail(string email) => new System.Net.Mail.MailAddress(email).Address == email;

        private bool IsValidTelefone(string telefone)
        {
            var cleaned = Recursos.RemoverMascaraTelefone(telefone);
            return cleaned.Length == 10 || cleaned.Length == 11;
        }

        #endregion
    }
}