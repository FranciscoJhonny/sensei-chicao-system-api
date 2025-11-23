using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    /// <summary>
    /// DTO para inativação de academia
    /// </summary>
    public class AcademiaInativacaoDto
    {
        /// <summary>
        /// ID único da academia a ser inativada
        /// </summary>
        [Required(ErrorMessage = "ID é obrigatório")]
        public int AcademiaId { get; set; }

        /// <summary>
        /// ID do usuário que está realizando a operação de inativação
        /// </summary>
        [Required(ErrorMessage = "O ID do usuário que está criando é obrigatório.")]
        public int UsuarioOperacaoId { get; set; }
    }
}