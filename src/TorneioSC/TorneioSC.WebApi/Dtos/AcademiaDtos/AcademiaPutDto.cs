using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    public class AcademiaPutDto
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        public int AcademiaId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome não pode exceder 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public string Email { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public int MunicipioId { get; set; }
        public int? FederacaoId { get; set; }
        public string ResponsavelNome { get; set; } = string.Empty;
        public string ResponsavelCpf { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public int UsuarioAlteracaoId { get; set; }

        // Coleções com IDs para sincronização
        public ICollection<EnderecoPutDto> Enderecos { get; set; } = new List<EnderecoPutDto>();
        public ICollection<TelefonePutDto> Telefones { get; set; } = new List<TelefonePutDto>();
        public ICollection<RedeSocialPutDto> RedesSociais { get; set; } = new List<RedeSocialPutDto>();
    }
}