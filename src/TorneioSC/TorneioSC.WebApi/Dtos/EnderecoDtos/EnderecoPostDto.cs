using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    /// <summary>
    /// DTO para criação de um novo endereço
    /// </summary>
    public class EnderecoPostDto
    {
        /// <summary>
        /// Nome da rua, avenida, praça, etc.
        /// </summary>
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        [StringLength(200, ErrorMessage = "O logradouro não pode exceder 200 caracteres.")]
        public string Logradouro { get; set; } = string.Empty;

        /// <summary>
        /// Número do endereço
        /// </summary>
        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(10, ErrorMessage = "O número não pode exceder 10 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        /// <summary>
        /// Complemento do endereço (apartamento, bloco, sala, etc.)
        /// </summary>
        [StringLength(50, ErrorMessage = "O complemento não pode exceder 50 caracteres.")]
        public string Complemento { get; set; } = string.Empty;

        /// <summary>
        /// Código Postal (CEP) no formato 8 dígitos (XXXXXXXX) ou formato XXXXX-XXX
        /// </summary>
        [StringLength(9, ErrorMessage = "O CEP deve ter até 9 dígitos.")]
        [RegularExpression(@"^\d{8}$|^\d{5}-\d{3}$", ErrorMessage = "CEP inválido. Use 8 dígitos ou formato XXXXX-XXX.")]
        public string Cep { get; set; } = string.Empty;

        /// <summary>
        /// Bairro do endereço
        /// </summary>
        [StringLength(100, ErrorMessage = "O bairro não pode exceder 100 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que está incluindo o endereço
        /// </summary>
        [Required(ErrorMessage = "O ID do usuário que está incluindo é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }
    }
}