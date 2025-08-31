using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    public class AcademiaPostDto
    {
        [Required(ErrorMessage = "O nome da academia é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome não pode exceder 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O município é obrigatório.")]
        public int MunicipioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O ID da federação deve ser maior que 0.")]
        public int? FederacaoId { get; set; }
        
        [RegularExpression(@"^\d{14}$|^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Use 14 dígitos ou formato XX.XXX.XXX/XXXX-XX.")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do responsável é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do responsável não pode exceder 100 caracteres.")]
        public string ResponsavelNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF do responsável é obrigatório.")]
        [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$",ErrorMessage = "CPF inválido. Use 11 dígitos ou formato XXX.XXX.XXX-XX.")]
        public string ResponsavelCpf { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "O e-mail fornecido é inválido.")]
        [StringLength(100, ErrorMessage = "O e-mail não pode exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A URL do logo não pode exceder 500 caracteres.")]
        [Url(ErrorMessage = "A URL do logo deve ser uma URL válida.")]
        public string LogoUrl { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do usuário que está criando é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }

        // Coleções
        [MinLength(1, ErrorMessage = "É necessário informar pelo menos um endereço.")]
        public ICollection<EnderecoPostDto> Enderecos { get; set; } = new List<EnderecoPostDto>();

        [MinLength(1, ErrorMessage = "É necessário informar pelo menos um telefone.")]
        public ICollection<TelefonePostDto> Telefones { get; set; } = new List<TelefonePostDto>();

        public ICollection<RedeSocialPostDto> RedesSociais { get; set; } = new List<RedeSocialPostDto>();
    }
}
