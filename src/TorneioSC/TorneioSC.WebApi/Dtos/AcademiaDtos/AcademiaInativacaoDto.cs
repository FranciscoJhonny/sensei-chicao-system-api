using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    public class AcademiaInativacaoDto
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        public int AcademiaId { get; set; }
        [Required(ErrorMessage = "O ID do usuário que está criando é obrigatório.")]
        public int UsuarioOperacaoId { get; set; }
    }
}
