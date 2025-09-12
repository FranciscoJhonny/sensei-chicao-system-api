using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Adaptador de persistência responsável pelas operações de escrita no banco de dados relacionadas a eventos.
    /// Define métodos para criação, atualização e inativação de eventos e seus dados associados (torneio, categorias, etc).
    /// </summary>
    public interface IEventoSqlWriteAdapter
    {
        /// <summary>
        /// Insere um novo evento no banco de dados, juntamente com seu torneio associado e as categorias definidas.
        /// </summary>
        /// <param name="evento">Objeto contendo os dados do evento, torneio e categorias a serem persistidos.</param>
        /// <returns>O ID do evento recém-criado.</returns>
        /// <exception cref="OperacaoEventoException">
        /// Lançada quando ocorre um erro durante a operação de inserção (ex: falha de banco, violação de restrição).
        /// </exception>
        Task<int> PostEventoAsync(Evento evento);

        /// <summary>
        /// Atualiza um evento existente no banco de dados, incluindo seu torneio e categorias associadas.
        /// </summary>
        /// <param name="evento">Objeto contendo os dados atualizados do evento. O campo <see cref="Evento.EventoId"/> deve ser válido.</param>
        /// <returns>O ID do evento atualizado.</returns>
        /// <exception cref="EventoNaoEncontradoException">
        /// Lançada quando o evento com o <see cref="Evento.EventoId"/> fornecido não existe.
        /// </exception>
        /// <exception cref="AtualizacaoEventoException">
        /// Lançada quando ocorre um erro durante a operação de atualização.
        /// </exception>
        Task<int> PutEventoAsync(Evento evento);

        /// <summary>
        /// Inativa logicamente um evento no banco de dados, marcando-o como inativo.
        /// </summary>
        /// <param name="eventoId">ID do evento a ser inativado.</param>
        /// <param name="usuarioOperacaoId">ID do usuário responsável pela operação de inativação.</param>
        /// <returns><c>true</c> se a inativação for bem-sucedida; caso contrário, lança uma exceção.</returns>
        /// <exception cref="EventoNaoEncontradoException">
        /// Lançada quando o evento com o ID fornecido não é encontrado.
        /// </exception>
        /// <exception cref="ExclusaoEventoException">
        /// Lançada quando ocorre um erro durante a operação de inativação.
        /// </exception>
        Task<bool> InativarEventoPorIdAsync(int eventoId, int usuarioOperacaoId);
    }
}