using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;
using TorneioSC.Exception.ExceptionBase.ExceptionEvento;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.EventoAdapters
{
    /// <summary>
    /// Implementação do adaptador de leitura para consultas de eventos no SQL Server, sem uso de SqlBuilder.
    /// </summary>
    internal class EventoSqlReadAdapter : IEventoSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<EventoSqlReadAdapter> _logger;

        public EventoSqlReadAdapter(ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<EventoSqlReadAdapter>()
                      ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public async Task<int> ObterTotalEventosAtivosAsync()
        {
            const string sql = "SELECT COUNT(*) FROM Evento WHERE Ativo = 1";

            try
            {
                return await _connection.ExecuteScalarAsync<int>(sql);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter total de evento ativas");
                throw new OperacaoEventoException("contagem de evento ativas", ex);
            }
        }

        public async Task<int> ObterTotalEventosAsync(FiltroEvento? filtro = null)
        {
            var sql = new StringBuilder(@"SELECT COUNT(*) FROM Evento WHERE Ativo = 1");

            var parameters = new DynamicParameters();
            AplicarFiltros(sql, parameters, filtro);

            try
            {
                return await _connection.ExecuteScalarAsync<int>(sql.ToString(), parameters);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter total de academias com filtros");
                throw new OperacaoAcademiaException("contagem de academias com filtros", ex);
            }
        }

        public async Task<Evento?> ObterEventoPorIdAsync(int eventoId)
        {
            try
            {
                if (eventoId <= 0)
                    throw new ArgumentException("ID do evento inválido", nameof(eventoId));

                const string sql = @"SELECT e.EventoId ,e.NomeEvento ,e.DataInicio ,e.DataFim ,e.Local ,e.Responsavel ,
                                            e.EmailResponsavel ,e.TelefoneResponsavel ,e.Observacoes ,t.TorneioId ,
                                            t.NomeTorneio ,t.Tipo ,t.DataInicio AS TorneioDataInicio ,t.DataFim AS TorneioDataFim ,
                                            t.MunicipioId ,t.Contratante ,m.DescricaoMunicio ,es.DescricaoEstado ,
                                            c.CategoriaId ,c.NomeCategoria  ,c.IdadeMin ,c.IdadeMax ,c.Sexo ,
                                            c.PesoMin ,c.PesoMax ,c.ModalidadeId ,mo.NomeModalidade
                                     FROM   Evento e
                                            INNER JOIN EventoTorneio et ON e.EventoId = et.EventoId
                                            INNER JOIN Torneio t ON et.TorneioId = t.TorneioId
                                            INNER JOIN Municipio m ON t.MunicipioId = m.MunicipioId
                                            INNER JOIN Estado es ON m.EstadoId = es.EstadoId
                                            LEFT JOIN TorneioCategoria tc ON t.TorneioId = tc.TorneioId
                                                                             AND tc.Ativo = 1
                                            LEFT JOIN Categoria c ON tc.CategoriaId = c.CategoriaId
                                                                     AND c.Ativo = 1
                                            LEFT JOIN Modalidade mo ON c.ModalidadeId = mo.ModalidadeId
                                     WHERE  e.EventoId = @eventoId
                                            AND e.Ativo = 1;";

                Evento? evento = null;
                Torneio? torneio = null;

                var categoriasDict = new Dictionary<int, Categoria>();

                await _connection.QueryAsync<Evento, Torneio, Categoria, Modalidade, object>(
                    sql,
                    (e, t, c, mo) =>
                    {
                        // Inicializa o evento (uma única vez)
                        if (evento == null)
                        {
                            evento = new Evento
                            {
                                EventoId = e.EventoId,
                                NomeEvento = e.NomeEvento,
                                DataInicio = e.DataInicio,
                                DataFim = e.DataFim,
                                Local = e.Local,
                                Responsavel = e.Responsavel,
                                EmailResponsavel = e.EmailResponsavel,
                                TelefoneResponsavel = e.TelefoneResponsavel,
                                Observacoes = e.Observacoes,
                                Ativo = true,
                                Torneio = new Torneio()
                            };

                            torneio = evento.Torneio;
                        }

                        // Preenche o torneio (uma única vez)
                        if (torneio != null && !torneio.TorneioId.Equals(default(int)))
                        {
                            // Já foi preenchido
                        }
                        else if (t != null)
                        {
                            torneio!.TorneioId = t.TorneioId;
                            torneio!.NomeTorneio = t.NomeTorneio;
                            torneio!.Tipo = t.Tipo;
                            torneio!.TorneioDataInicio = t.TorneioDataInicio;
                            torneio!.TorneioDataFim = t.TorneioDataFim;
                            torneio!.MunicipioId = t.MunicipioId;
                            torneio!.Contratante = t.Contratante;
                            torneio!.DescricaoMunicipio = t.Municipio.DescricaoMunicio;
                            torneio!.DescricaoEstado = t.DescricaoEstado;
                            torneio!.Categorias = new List<Categoria>();
                        }

                        // Adiciona categoria, se existir
                        if (c != null && c.CategoriaId != 0 && !categoriasDict.ContainsKey(c.CategoriaId))
                        {
                            var categoria = new Categoria
                            {
                                CategoriaId = c.CategoriaId,
                                NomeCategoria = c.NomeCategoria,
                                IdadeMin = c.IdadeMin,
                                IdadeMax = c.IdadeMax,
                                Sexo = c.Sexo,
                                PesoMin = c.PesoMin,
                                PesoMax = c.PesoMax,
                                ModalidadeId = c.ModalidadeId,
                                NomeModalidade = mo?.ModalidadeNome ?? string.Empty,
                                Ativo = true
                            };

                            categoriasDict[c.CategoriaId] = categoria;
                            torneio?.Categorias.Add(categoria);
                        }

                        return evento; // necessário por causa da assinatura, mas ignorado
                    },
                    new { eventoId },
                    splitOn: "TorneioId,CategoriaId,ModalidadeId");

                return evento;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter evento por ID: {EventoId}", eventoId);
                throw new OperacaoEventoException("busca por ID", ex);
            }
        }

        public async Task<IEnumerable<Evento>> ObterEventosAsync()
        {
            const string sql = "SELECT * FROM Evento WHERE Ativo = 1 ORDER BY Nome";
            return await _connection.QueryAsync<Evento>(sql);
        }

        public async Task<(IEnumerable<Evento> Eventos, int Total)> ObterEventosPorFiltroAsync(FiltroEvento filtro)
        {
            // Query base
            var sql = new StringBuilder(@"
                SELECT e.*
                FROM Evento e
                LEFT JOIN EventoTorneio et ON e.EventoId = et.EventoId
                LEFT JOIN Torneio t ON et.TorneioId = t.TorneioId
                LEFT JOIN TorneioCategoria tc ON t.TorneioId = tc.TorneioId
                LEFT JOIN Categoria c ON tc.CategoriaId = c.CategoriaId
                WHERE 1 = 1");

            var parameters = new DynamicParameters();
            AplicarFiltros(sql, parameters, filtro);

            // Adicionar ordenação e paginação
            sql.Append(" ORDER BY e.Nome ");
            sql.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
            parameters.Add("Offset", (filtro.Pagina - 1) * filtro.TamanhoPagina);
            parameters.Add("PageSize", filtro.TamanhoPagina);

            var eventos = await _connection.QueryAsync<Evento>(sql.ToString(), parameters);

            // Contagem total
            var countSql = new StringBuilder(@"
                SELECT COUNT(*) 
                FROM Evento e
                LEFT JOIN EventoTorneio et ON e.EventoId = et.EventoId
                LEFT JOIN Torneio t ON et.TorneioId = t.TorneioId
                LEFT JOIN TorneioCategoria tc ON t.TorneioId = tc.TorneioId
                LEFT JOIN Categoria c ON tc.CategoriaId = c.CategoriaId
                WHERE 1 = 1");

            AplicarFiltros(countSql, parameters, filtro);

            var total = await _connection.QuerySingleAsync<int>(countSql.ToString(), parameters);

            return (eventos, total);
        }

        public async Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync()
        {
            const string sql = @"
                SELECT 
                    e.EventoId,
                    e.Nome,
                    e.DataInicio,
                    e.DataFim,
                    e.Local,
                    e.Responsavel,
                    e.Ativo,
                    ISNULL(torneios.Quantidade, 0) AS QuantidadeTorneios
                FROM Evento e
                OUTER APPLY (
                    SELECT COUNT(*) AS Quantidade
                    FROM EventoTorneio et
                    INNER JOIN Torneio t ON et.TorneioId = t.TorneioId
                    WHERE et.EventoId = e.EventoId AND et.Ativo = 1 AND t.Ativo = 1
                ) torneios
                WHERE e.Ativo = 1
                ORDER BY e.Nome";

            return await _connection.QueryAsync<EventoResumo>(sql);
        }

        public async Task<(IEnumerable<EventoResumo> Resumos, int Total)> ObterResumoEventosPaginadoAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            const string countSql = @"
                SELECT COUNT(*) 
                FROM Evento e 
                WHERE e.Ativo = 1";

            const string pagedSql = @"SELECT 
                                          e.EventoId,
                                          e.Nome,
                                          e.DataInicio,
                                          e.DataFim,
                                          e.Local,
                                          e.Responsavel,
                                          e.Ativo,
                                          ISNULL(torneios.Quantidade, 0) AS QuantidadeTorneios
                                      FROM Evento e
                                      OUTER APPLY (
                                          SELECT COUNT(*) AS Quantidade
                                          FROM EventoTorneio et
                                          INNER JOIN Torneio t ON et.TorneioId = t.TorneioId
                                          WHERE et.EventoId = e.EventoId AND et.Ativo = 1 AND t.Ativo = 1
                                      ) torneios
                                      WHERE e.Ativo = 1
                                      ORDER BY e.Nome
                                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var total = await _connection.QuerySingleAsync<int>(countSql);
            var resumos = await _connection.QueryAsync<EventoResumo>(pagedSql, new
            {
                Offset = (pagina - 1) * tamanhoPagina,
                PageSize = tamanhoPagina
            });

            return (resumos, total);
        }

        public async Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync(FiltroEvento filtro)
        {
            var sql = new StringBuilder(@"
                                          SELECT 
                                              e.EventoId,
                                              e.Nome,
                                              e.DataInicio,
                                              e.DataFim,
                                              e.Local,
                                              e.Responsavel,
                                              e.Ativo,
                                              ISNULL(torneios.Quantidade, 0) AS QuantidadeTorneios
                                          FROM Evento e
                                          OUTER APPLY (
                                              SELECT COUNT(*) AS Quantidade
                                              FROM EventoTorneio et
                                              INNER JOIN Torneio t ON et.TorneioId = t.TorneioId
                                              WHERE et.EventoId = e.EventoId AND et.Ativo = 1 AND t.Ativo = 1
                                          ) torneios
                                          LEFT JOIN EventoTorneio et ON e.EventoId = et.EventoId
                                          LEFT JOIN Torneio t ON et.TorneioId = t.TorneioId
                                          LEFT JOIN TorneioCategoria tc ON t.TorneioId = tc.TorneioId
                                          LEFT JOIN Categoria c ON tc.CategoriaId = c.CategoriaId
                                          WHERE 1 = 1");

            var parameters = new DynamicParameters();
            AplicarFiltros(sql, parameters, filtro);
            sql.Append(" ORDER BY e.Nome");

            return await _connection.QueryAsync<EventoResumo>(sql.ToString(), parameters);
        }

        public async Task<EstatisticasEventos> ObterEstatisticasEventosAsync()
        {
            const string sql = @"SELECT
                                     COUNT(*) AS Total,
                                     SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END) AS Ativos,
                                     SUM(CASE WHEN Ativo = 0 THEN 1 ELSE 0 END) AS Inativos,
                                     SUM(CASE WHEN DataFim < GETDATE() THEN 1 ELSE 0 END) AS Finalizados,
                                     SUM(CASE WHEN DataInicio <= GETDATE() AND DataFim >= GETDATE() THEN 1 ELSE 0 END) AS EmAndamento,
                                     SUM(CASE WHEN DataInicio > GETDATE() THEN 1 ELSE 0 END) AS Proximos
                                 FROM Evento";

            var result = await _connection.QueryFirstOrDefaultAsync(sql);

            return new EstatisticasEventos
            {
                Total = result?.Total ?? 0,
                Ativos = result?.Ativos ?? 0,
                Inativos = result?.Inativos ?? 0,
                Finalizados = result?.Finalizados ?? 0,
                EmAndamento = result?.EmAndamento ?? 0,
                Proximos = result?.Proximos ?? 0
            };
        }

        // Método auxiliar: Aplica filtros dinamicamente
        private void AplicarFiltros(StringBuilder sql, DynamicParameters parameters, FiltroEvento? filtro)
        {
            if (filtro == null) return;

            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                sql.Append(" AND e.Nome LIKE @Nome");
                parameters.Add("Nome", $"%{filtro.Nome}%");
            }

            if (filtro.DataInicioDe.HasValue)
            {
                sql.Append(" AND e.DataInicio >= @DataInicioDe");
                parameters.Add("DataInicioDe", filtro.DataInicioDe.Value);
            }

            if (filtro.DataInicioAte.HasValue)
            {
                sql.Append(" AND e.DataInicio <= @DataInicioAte");
                parameters.Add("DataInicioAte", filtro.DataInicioAte.Value);
            }

            if (filtro.DataFimDe.HasValue)
            {
                sql.Append(" AND e.DataFim >= @DataFimDe");
                parameters.Add("DataFimDe", filtro.DataFimDe.Value);
            }

            if (filtro.DataFimAte.HasValue)
            {
                sql.Append(" AND e.DataFim <= @DataFimAte");
                parameters.Add("DataFimAte", filtro.DataFimAte.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtro.Local))
            {
                sql.Append(" AND e.Local LIKE @Local");
                parameters.Add("Local", $"%{filtro.Local}%");
            }

            if (!string.IsNullOrWhiteSpace(filtro.Responsavel))
            {
                sql.Append(" AND e.Responsavel LIKE @Responsavel");
                parameters.Add("Responsavel", $"%{filtro.Responsavel}%");
            }

            if (filtro.Ativo.HasValue)
            {
                sql.Append(" AND e.Ativo = @Ativo");
                parameters.Add("Ativo", filtro.Ativo.Value);
            }

            if (filtro.MunicipioId.HasValue)
            {
                sql.Append(" AND t.MunicipioId = @MunicipioId");
                parameters.Add("MunicipioId", filtro.MunicipioId.Value);
            }

            if (filtro.ModalidadeId.HasValue)
            {
                sql.Append(" AND c.ModalidadeId = @ModalidadeId");
                parameters.Add("ModalidadeId", filtro.ModalidadeId.Value);
            }
        }
    }
}