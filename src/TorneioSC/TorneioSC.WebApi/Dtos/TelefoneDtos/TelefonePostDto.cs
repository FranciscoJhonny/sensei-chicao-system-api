using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    public class TelefonePostDto
    {
        [Required(ErrorMessage = "O número de telefone é obrigatório.")]
        [StringLength(15, ErrorMessage = "O número de telefone não pode exceder 15 caracteres.")]
        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public string NumeroTelefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de telefone é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Tipo de telefone inválido.")]
        public int TipoTelefoneId { get; set; }

        [Required(ErrorMessage = "O ID do usuário que está incluindo é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }
    }
}