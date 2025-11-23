using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    /// <summary>
    /// DTO para criação de uma nova academia
    /// </summary>
    public class AcademiaPostDto
    {
        /// <summary>
        /// Nome da academia
        /// </summary>
        [Required(ErrorMessage = "O nome da academia é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome não pode exceder 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// ID do município onde a academia está localizada
        /// </summary>
        [Required(ErrorMessage = "O município é obrigatório.")]
        public int MunicipioId { get; set; }

        /// <summary>
        /// ID da federação à qual a academia está vinculada (opcional)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O ID da federação deve ser maior que 0.")]
        public int? FederacaoId { get; set; }

        /// <summary>
        /// CNPJ da academia (formato: 14 dígitos ou XX.XXX.XXX/XXXX-XX)
        /// </summary>
        [RegularExpression(@"^\d{14}$|^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Use 14 dígitos ou formato XX.XXX.XXX/XXXX-XX.")]
        public string Cnpj { get; set; } = string.Empty;

        /// <summary>
        /// Nome do responsável pela academia
        /// </summary>
        [Required(ErrorMessage = "O nome do responsável é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do responsável não pode exceder 100 caracteres.")]
        public string ResponsavelNome { get; set; } = string.Empty;

        /// <summary>
        /// CPF do responsável pela academia (formato: 11 dígitos ou XXX.XXX.XXX-XX)
        /// </summary>
        [Required(ErrorMessage = "O CPF do responsável é obrigatório.")]
        [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF inválido. Use 11 dígitos ou formato XXX.XXX.XXX-XX.")]
        public string ResponsavelCpf { get; set; } = string.Empty;

        /// <summary>
        /// Email de contato da academia
        /// </summary>
        [EmailAddress(ErrorMessage = "O e-mail fornecido é inválido.")]
        [StringLength(100, ErrorMessage = "O e-mail não pode exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// URL da logo da academia
        /// </summary>
        [StringLength(500, ErrorMessage = "A URL do logo não pode exceder 500 caracteres.")]
        [Url(ErrorMessage = "A URL do logo deve ser uma URL válida.")]
        public string LogoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da academia
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que está criando a academia
        /// </summary>
        [Required(ErrorMessage = "O ID do usuário que está criando é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Lista de endereços da academia (pelo menos um endereço é obrigatório)
        /// </summary>
        [MinLength(1, ErrorMessage = "É necessário informar pelo menos um endereço.")]
        public ICollection<EnderecoPostDto> Enderecos { get; set; } = new List<EnderecoPostDto>();

        /// <summary>
        /// Lista de telefones da academia (pelo menos um telefone é obrigatório)
        /// </summary>
        [MinLength(1, ErrorMessage = "É necessário informar pelo menos um telefone.")]
        public ICollection<TelefonePostDto> Telefones { get; set; } = new List<TelefonePostDto>();

        /// <summary>
        /// Lista de redes sociais da academia (opcional)
        /// </summary>
        public ICollection<RedeSocialPostDto> RedesSociais { get; set; } = new List<RedeSocialPostDto>();
    }
}