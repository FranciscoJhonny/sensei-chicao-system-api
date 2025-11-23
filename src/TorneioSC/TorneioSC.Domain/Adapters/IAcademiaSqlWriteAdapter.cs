using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Define operações de escrita relacionadas à entidade Academia,
    /// incluindo criação, atualização e inativação de academias.
    /// </summary>
    public interface IAcademiaSqlWriteAdapter
    {
        /// <summary>
        /// Insere uma nova academia no banco de dados.
        /// </summary>
        /// <param name="academia">Objeto contendo os dados da academia a ser cadastrada.</param>
        /// <returns>
        /// Retorna o ID gerado para a nova academia cadastrada.
        /// </returns>
        Task<int> PostAcademiaAsync(Academia academia);

        /// <summary>
        /// Atualiza os dados de uma academia existente.
        /// </summary>
        /// <param name="academia">Objeto contendo os dados atualizados da academia.</param>
        /// <returns>
        /// Retorna 1 se a atualização foi bem-sucedida; caso contrário, 0.
        /// </returns>
        Task<int> PutAcademiaAsync(Academia academia);

        /// <summary>
        /// Inativa uma academia pelo ID informado, registrando também
        /// o usuário responsável pela operação.
        /// </summary>
        /// <param name="academiaId">ID da academia a ser inativada.</param>
        /// <param name="usuarioOperacaoId">ID do usuário que realizou a operação.</param>
        /// <returns>
        /// Retorna true se a academia foi inativada com sucesso; caso contrário, false.
        /// </returns>
        Task<bool> InativarAcademiaPorIdAsync(int academiaId, int usuarioOperacaoId);
    }

}
