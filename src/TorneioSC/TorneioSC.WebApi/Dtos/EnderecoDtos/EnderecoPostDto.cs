using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    public class EnderecoPostDto
    {
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        [StringLength(200, ErrorMessage = "O logradouro não pode exceder 200 caracteres.")]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(10, ErrorMessage = "O número não pode exceder 10 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "O complemento não pode exceder 50 caracteres.")]
        public string Complemento { get; set; } = string.Empty;

        [StringLength(9, ErrorMessage = "O CEP deve ter até 9 dígitos.")]
        [RegularExpression(@"^\d{8}$|^\d{5}-\d{3}$", ErrorMessage = "CEP inválido. Use 9 dígitos ou formato XXXXX-XXX.")]
        public string Cep { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "O bairro não pode exceder 100 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do usuário que está incluindo é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }
    }
}