using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO para inativação de uma federação
    /// </summary>
    public class FederacaoInativacaoDto
    {
        /// <summary>
        /// ID único da federação a ser inativada
        /// </summary>
        [Required(ErrorMessage = "ID é obrigatório")]
        public int FederacaoId { get; set; }

        /// <summary>
        /// ID do usuário que está realizando a operação de inativação
        /// </summary>
        [Required(ErrorMessage = "O ID do usuário que está criando é obrigatório.")]
        public int UsuarioOperacaoId { get; set; }
    }
}
