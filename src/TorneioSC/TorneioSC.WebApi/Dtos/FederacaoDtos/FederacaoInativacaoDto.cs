using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoInativacaoDto
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        public int FederacaoId { get; set; }
        [Required(ErrorMessage = "O ID do usuário que está criando é obrigatório.")]
        public int UsuarioOperacaoId { get; set; }
    }
}
