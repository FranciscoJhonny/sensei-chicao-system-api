using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Define operações de escrita relacionadas à entidade Federação
    /// no banco de dados, incluindo criação, atualização e inativação.
    /// </summary>
    public interface IFederacaoSqlWriteAdapter
    {
        /// <summary>
        /// Insere uma nova federação no banco de dados.
        /// </summary>
        /// <param name="federacao">Objeto contendo os dados da federação a ser cadastrada.</param>
        /// <returns>
        /// Retorna o ID gerado para a nova federação cadastrada.
        /// </returns>
        Task<int> PostFederacaoAsync(Federacao federacao);

        /// <summary>
        /// Atualiza os dados de uma federação existente.
        /// </summary>
        /// <param name="federacao">Objeto contendo os dados atualizados da federação.</param>
        /// <returns>
        /// Retorna 1 se a operação for bem-sucedida, caso contrário 0.
        /// </returns>
        Task<int> PutFederacaoAsync(Federacao federacao);

        /// <summary>
        /// Inativa uma federação pelo ID informado,
        /// registrando também o usuário que realizou a operação.
        /// </summary>
        /// <param name="federacaoId">ID da federação a ser inativada.</param>
        /// <param name="usuarioOperacaoId">ID do usuário responsável pela operação.</param>
        /// <returns>
        /// Retorna true se a federação foi inativada com sucesso; caso contrário, false.
        /// </returns>
        Task<bool> InativarFederacaoPorIdAsync(int federacaoId, int usuarioOperacaoId);
    }

}