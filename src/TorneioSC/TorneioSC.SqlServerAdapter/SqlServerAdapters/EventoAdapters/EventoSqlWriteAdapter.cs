using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionEvento;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.EventoAdapters
{
    /// <summary>
    /// Adaptador de persistência para operações de escrita relacionadas a eventos no banco de dados SQL Server.
    /// Responsável por criar, atualizar e inativar eventos, torneios e suas categorias associadas.
    /// </summary>
    internal class EventoSqlWriteAdapter : IEventoSqlWriteAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<EventoSqlWriteAdapter> _logger;

        /// <summary>
        /// Inicializa uma nova instância do adaptador com uma conexão e um logger.
        /// </summary>
        /// <param name="loggerFactory">Fábrica de loggers para criar uma instância de <see cref="ILogger{EventoSqlWriteAdapter}"/>.</param>
        /// <param name="context">Contexto contendo a conexão com o banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="context"/> ou <paramref name="loggerFactory"/> forem nulos.</exception>
        public EventoSqlWriteAdapter(ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<EventoSqlWriteAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        /// <summary>
        /// Insere um novo evento no banco de dados, juntamente com seu torneio associado e as categorias definidas.
        /// </summary>
        /// <param name="evento">Objeto contendo os dados do evento, torneio e categorias a serem inseridos.</param>
        /// <returns>O ID do evento recém-criado.</returns>
        /// <exception cref="OperacaoEventoException">
        /// Lançada quando ocorre um erro durante a operação de inserção (ex: falha de banco de dados, violação de restrição).
        /// </exception>
        public async Task<int> PostEventoAsync(Evento evento)
        {
            using var transaction = _connection.BeginTransaction();
            try
            {
                // 1. Inserir o evento
                const string sqlEvento = @"INSERT INTO dbo.Evento 
                                                  ( Nome, DataInicio, DataFim, Local, Responsavel, EmailResponsavel, 
                                                   TelefoneResponsavel, Observacoes, Ativo, 
                                                   UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                   UsuarioOperacaoId, DataOperacao)
                                           VALUES 
                                                  ( @Nome, @DataInicio, @DataFim, @Local, @Responsavel, @EmailResponsavel, 
                                                   @TelefoneResponsavel, @Observacoes, @Ativo, 
                                                   @UsuarioInclusaoId, GETDATE(), 'I', 
                                                   @UsuarioOperacaoId, GETDATE());
                                           SELECT CAST(SCOPE_IDENTITY() AS int);";

                int eventoId = await _connection.ExecuteScalarAsync<int>(sqlEvento, evento, transaction);

                // 2. Inserir o torneio
                const string sqlTorneio = @"INSERT INTO dbo.Torneio
                                                   ( Nome, Tipo, DataInicio, DataFim, MunicipioId,
                                                     Contratante, Ativo, UsuarioInclusaoId, DataInclusao,
                                                     NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                            VALUES
                                                   ( @Nome, @Tipo, GETDATE(), GETDATE(), @MunicipioId,
                                                     @Contratante, @Ativo, @UsuarioInclusaoId, GETDATE(),
                                                     'I', @UsuarioOperacaoId, GETDATE() );
                                            SELECT CAST(SCOPE_IDENTITY() AS int);";

                int torneioId = await _connection.ExecuteScalarAsync<int>(sqlTorneio, evento.Torneio, transaction);

                // 3. Associar evento ao torneio
                const string sqlEventoTorneio = @"INSERT INTO dbo.EventoTorneio
                                                      ( EventoId, TorneioId, Ativo, UsuarioInclusaoId,
                                                        DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                               VALUES
                                                      ( @EventoId, @TorneioId, @Ativo, @UsuarioInclusaoId,
                                                        GETDATE(), 'I', @UsuarioOperacaoId, GETDATE() );";

                await _connection.ExecuteAsync(sqlEventoTorneio, new
                {
                    EventoId = eventoId,
                    TorneioId = torneioId,
                    Ativo = true,
                    evento.UsuarioInclusaoId,
                    UsuarioOperacaoId = evento.UsuarioInclusaoId
                }, transaction);

                // 4. Inserir categorias e associar ao torneio
                foreach (var categoria in evento.Torneio.Categorias)
                {
                    const string sqlCategoria = @"INSERT INTO dbo.Categoria
                                                          ( Nome, IdadeMin, IdadeMax, Sexo, PesoMin, PesoMax, ModalidadeId, Ativo,
                                                            UsuarioInclusaoId, DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                                  VALUES
                                                          ( @Nome, @IdadeMin, @IdadeMax, @Sexo, @PesoMin, @PesoMax, @ModalidadeId, @Ativo,
                                                            @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE() );
                                                  SELECT CAST(SCOPE_IDENTITY() AS int);";

                    int categoriaId = await _connection.ExecuteScalarAsync<int>(sqlCategoria, categoria, transaction);

                    const string sqlTorneioCategoria = @"INSERT INTO dbo.TorneioCategoria
                                                                 ( TorneioId, CategoriaId, Ativo, UsuarioInclusaoId, DataInclusao,
                                                                   NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                                         VALUES
                                                                 ( @TorneioId, @CategoriaId, @Ativo, @UsuarioInclusaoId, GETDATE(),
                                                                   'I', @UsuarioOperacaoId, GETDATE() );";

                    await _connection.ExecuteAsync(sqlTorneioCategoria, new
                    {
                        TorneioId = torneioId,
                        CategoriaId = categoriaId,
                        Ativo = true,
                        evento.UsuarioInclusaoId,
                        UsuarioOperacaoId = evento.UsuarioInclusaoId
                    }, transaction);
                }

                transaction.Commit();
                return eventoId;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erro ao criar evento");
                throw new OperacaoEventoException("criar", ex);
            }
        }

        /// <summary>
        /// Atualiza um evento existente, incluindo seu torneio e categorias associadas.
        /// </summary>
        /// <param name="evento">Objeto contendo os dados atualizados do evento. O campo <see cref="Evento.EventoId"/> deve ser válido.</param>
        /// <returns>O ID do evento atualizado.</returns>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="evento"/> for nulo.</exception>
        /// <exception cref="ArgumentException">Lançada se o <see cref="Evento.EventoId"/> for inválido (menor ou igual a zero).</exception>
        /// <exception cref="EventoNaoEncontradoException">Lançada se o evento não for encontrado no banco de dados.</exception>
        /// <exception cref="AtualizacaoEventoException">Lançada em caso de erro durante a atualização.</exception>
        public async Task<int> PutEventoAsync(Evento evento)
        {
            if (evento == null)
                throw new ArgumentNullException(nameof(evento));

            if (evento.EventoId <= 0)
                throw new ArgumentException("ID do evento inválido", nameof(evento.EventoId));

            using var transaction = _connection.BeginTransaction();

            try
            {
                // 1. Atualizar o evento
                const string sqlEvento = @"UPDATE Evento
                                           SET Nome = @Nome,
                                               DataInicio = @DataInicio,
                                               DataFim = @DataFim,
                                               Local = @Local,
                                               Responsavel = @Responsavel,
                                               EmailResponsavel = @EmailResponsavel,
                                               TelefoneResponsavel = @TelefoneResponsavel,
                                               Observacoes = @Observacoes,
                                               Ativo = @Ativo,
                                               DataOperacao = GETDATE(),
                                               UsuarioOperacaoId = @UsuarioOperacaoId,
                                               NaturezaOperacao = 'A'
                                           WHERE EventoId = @EventoId";

                var rowsAffected = await _connection.ExecuteAsync(sqlEvento, evento, transaction);

                if (rowsAffected == 0)
                    throw new EventoNaoEncontradoException(evento.EventoId);

                // 2. Obter o TorneioId associado ao evento
                const string sqlGetTorneioId = @"SELECT TorneioId FROM EventoTorneio WHERE EventoId = @EventoId AND Ativo = 1";

                var torneioId = await _connection.QuerySingleOrDefaultAsync<int?>(sqlGetTorneioId, new { evento.EventoId }, transaction);

                int finalTorneioId;

                if (torneioId == null)
                {
                    // Caso não exista torneio, criar um novo
                    const string sqlInsertTorneio = @"INSERT INTO dbo.Torneio
                                                            ( Nome, Tipo, DataInicio, DataFim, MunicipioId,
                                                              Contratante, Ativo, UsuarioInclusaoId, DataInclusao,
                                                              NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                                       VALUES
                                                           ( @Nome, @Tipo, GETDATE(), GETDATE(), @MunicipioId,
                                                             @Contratante, @Ativo, @UsuarioInclusaoId, GETDATE(),
                                                             'I', @UsuarioOperacaoId, GETDATE() );
                                                       SELECT CAST(SCOPE_IDENTITY() AS int);";

                    finalTorneioId = await _connection.ExecuteScalarAsync<int>(sqlInsertTorneio, evento.Torneio, transaction);

                    // Associar o novo torneio ao evento
                    const string sqlInsertEventoTorneio = @"INSERT INTO dbo.EventoTorneio
                                                                ( EventoId, TorneioId, Ativo, UsuarioInclusaoId,
                                                                  DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                                            VALUES
                                                                ( @EventoId, @TorneioId, 1, @UsuarioInclusaoId,
                                                                  GETDATE(), 'I', @UsuarioOperacaoId, GETDATE() )";

                    await _connection.ExecuteAsync(sqlInsertEventoTorneio, new
                    {
                        evento.EventoId,
                        TorneioId = finalTorneioId,
                        UsuarioInclusaoId = evento.UsuarioOperacaoId,
                        evento.UsuarioOperacaoId
                    }, transaction);
                }
                else
                {
                    finalTorneioId = torneioId.Value;

                    // 3. Atualizar o torneio existente
                    const string sqlUpdateTorneio = @"UPDATE dbo.Torneio
                                                      SET Nome = @Nome,
                                                          Tipo = @Tipo,
                                                          MunicipioId = @MunicipioId,
                                                          Contratante = @Contratante,
                                                          Ativo = @Ativo,
                                                          DataOperacao = GETDATE(),
                                                          UsuarioOperacaoId = @UsuarioOperacaoId,
                                                          NaturezaOperacao = 'A'
                                                      WHERE TorneioId = @TorneioId";

                    await _connection.ExecuteAsync(sqlUpdateTorneio, new
                    {
                        evento.Torneio.NomeTorneio,
                        evento.Torneio.Tipo,
                        evento.Torneio.MunicipioId,
                        evento.Torneio.Contratante,
                        evento.Torneio.Ativo,
                        evento.UsuarioOperacaoId,
                        TorneioId = finalTorneioId
                    }, transaction);
                }

                // 4. Sincronizar categorias do torneio
                await SincronizarCategoriasAsync(finalTorneioId, evento.Torneio.Categorias, evento.UsuarioOperacaoId, transaction);

                // Commit da transação
                transaction.Commit();

                return evento.EventoId;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erro ao atualizar evento {EventoId}", evento.EventoId);
                throw new AtualizacaoEventoException(evento.EventoId, ex);
            }
        }

        /// <summary>
        /// Sincroniza as categorias de um torneio: atualiza existentes, insere novas e desativa as removidas.
        /// </summary>
        /// <param name="torneioId">ID do torneio cujas categorias serão sincronizadas.</param>
        /// <param name="novasCategorias">Lista de categorias atualizadas a serem associadas ao torneio.</param>
        /// <param name="usuarioOperacaoId">ID do usuário responsável pela operação.</param>
        /// <param name="transaction">Transação de banco de dados ativa.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        private async Task SincronizarCategoriasAsync(int torneioId, IEnumerable<Categoria> novasCategorias, int? usuarioOperacaoId, IDbTransaction transaction)
        {
            if (novasCategorias == null) return;

            // Obter categorias atuais do torneio
            const string sqlGetCategorias = @"SELECT tc.CategoriaId FROM TorneioCategoria tc WHERE tc.TorneioId = @TorneioId AND tc.Ativo = 1";

            var categoriasExistentes = (await _connection.QueryAsync<int>(sqlGetCategorias, new { TorneioId = torneioId }, transaction)).ToHashSet();

            var novasIds = new HashSet<int>();
            foreach (var categoria in novasCategorias)
            {
                if (categoria.CategoriaId > 0 && categoriasExistentes.Contains(categoria.CategoriaId))
                {
                    // Atualizar categoria existente
                    const string sqlUpdate = @"UPDATE dbo.Categoria
                                               SET Nome = @Nome,
                                                   IdadeMin = @IdadeMin,
                                                   IdadeMax = @IdadeMax,
                                                   Sexo = @Sexo,
                                                   PesoMin = @PesoMin,
                                                   PesoMax = @PesoMax,
                                                   ModalidadeId = @ModalidadeId,
                                                   Ativo = @Ativo,
                                                   DataOperacao = GETDATE(),
                                                   UsuarioOperacaoId = @UsuarioOperacaoId,
                                                   NaturezaOperacao = 'A'
                                               WHERE CategoriaId = @CategoriaId";

                    await _connection.ExecuteAsync(sqlUpdate, new
                    {
                        categoria.NomeCategoria,
                        categoria.IdadeMin,
                        categoria.IdadeMax,
                        categoria.Sexo,
                        categoria.PesoMin,
                        categoria.PesoMax,
                        categoria.ModalidadeId,
                        categoria.Ativo,
                        UsuarioOperacaoId = usuarioOperacaoId,
                        categoria.CategoriaId
                    }, transaction);

                    novasIds.Add(categoria.CategoriaId);
                }
                else
                {
                    // Inserir nova categoria
                    const string sqlInsert = @"INSERT INTO dbo.Categoria
                                                   ( Nome, IdadeMin, IdadeMax, Sexo, PesoMin, PesoMax, ModalidadeId, Ativo,
                                                     UsuarioInclusaoId, DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                               VALUES
                                                   ( @Nome, @IdadeMin, @IdadeMax, @Sexo, @PesoMin, @PesoMax, @ModalidadeId, @Ativo,
                                                     @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE() );
                                               SELECT CAST(SCOPE_IDENTITY() AS int);";

                    var categoriaId = await _connection.ExecuteScalarAsync<int>(sqlInsert, new
                    {
                        categoria.NomeCategoria,
                        categoria.IdadeMin,
                        categoria.IdadeMax,
                        categoria.Sexo,
                        categoria.PesoMin,
                        categoria.PesoMax,
                        categoria.ModalidadeId,
                        categoria.Ativo,
                        UsuarioInclusaoId = usuarioOperacaoId,
                        UsuarioOperacaoId = usuarioOperacaoId
                    }, transaction);

                    // Associar ao torneio
                    const string sqlInsertTorneioCategoria = @"INSERT INTO dbo.TorneioCategoria
                                                                  ( TorneioId, CategoriaId, Ativo, UsuarioInclusaoId, DataInclusao,
                                                                    NaturezaOperacao, UsuarioOperacaoId, DataOperacao )
                                                               VALUES
                                                                   ( @TorneioId, @CategoriaId, 1, @UsuarioInclusaoId, GETDATE(),
                                                                     'I', @UsuarioOperacaoId, GETDATE() )";

                    await _connection.ExecuteAsync(sqlInsertTorneioCategoria, new
                    {
                        TorneioId = torneioId,
                        CategoriaId = categoriaId,
                        UsuarioInclusaoId = usuarioOperacaoId,
                        UsuarioOperacaoId = usuarioOperacaoId
                    }, transaction);

                    novasIds.Add(categoriaId);
                }
            }

            // Desativar categorias que não estão mais presentes
            var paraDesativar = categoriasExistentes.Except(novasIds).ToList();
            if (paraDesativar.Count > 0)
            {
                const string sqlDesativar = @"UPDATE TorneioCategoria
                                              SET Ativo = 0,
                                                  DataOperacao = GETDATE(),
                                                  UsuarioOperacaoId = @UsuarioOperacaoId
                                              WHERE TorneioId = @TorneioId AND CategoriaId IN @CategoriaIds";

                await _connection.ExecuteAsync(sqlDesativar, new
                {
                    TorneioId = torneioId,
                    CategoriaIds = paraDesativar,
                    UsuarioOperacaoId = usuarioOperacaoId
                }, transaction);
            }
        }

        /// <summary>
        /// Inativa logicamente um evento, marcando-o como inativo no banco de dados.
        /// </summary>
        /// <param name="eventoId">ID do evento a ser inativado.</param>
        /// <param name="usuarioOperacaoId">ID do usuário que está realizando a operação de inativação.</param>
        /// <returns><c>true</c> se o evento foi inativado com sucesso; caso contrário, lança uma exceção.</returns>
        /// <exception cref="ArgumentException">Lançada se <paramref name="eventoId"/> ou <paramref name="usuarioOperacaoId"/> forem inválidos.</exception>
        /// <exception cref="EventoNaoEncontradoException">Lançada se o evento não for encontrado.</exception>
        /// <exception cref="ExclusaoEventoException">Lançada em caso de erro durante a operação de inativação.</exception>
        public async Task<bool> InativarEventoPorIdAsync(int eventoId, int usuarioOperacaoId)
        {
            if (eventoId <= 0)
                throw new ArgumentException("ID do evento inválido", nameof(eventoId));

            if (usuarioOperacaoId <= 0)
                throw new ArgumentException("ID do usuário operador inválido", nameof(usuarioOperacaoId));

            try
            {
                const string sql = @"UPDATE Evento
                                        SET Ativo = 0,
                                            DataOperacao = GETDATE(),
                                            UsuarioOperacaoId = @usuarioOperacaoId,
                                            NaturezaOperacao = 'A'
                                        WHERE EventoId = @eventoId";

                var rowsAffected = await _connection.ExecuteAsync(sql, new { eventoId, usuarioOperacaoId });

                if (rowsAffected == 0)
                    throw new EventoNaoEncontradoException(eventoId);

                return true;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao inativar evento {EventoId} pelo usuário {UsuarioOperacaoId}", eventoId, usuarioOperacaoId);
                throw new ExclusaoEventoException(eventoId, ex);
            }
        }
    }
}