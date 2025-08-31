using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.AtletaDtos;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    public class AcademiaDto
    {
        public int AcademiaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? FederacaoId { get; set; }
        public int MunicipioId { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string ResponsavelNome { get; set; } = string.Empty;
        public string ResponsavelCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        public FederacaoDto? Federacao { get; set; }
        public MunicipioDto Municipio { get; set; } = new MunicipioDto();
        public UsuarioDto UsuarioInclusao { get; set; } = new UsuarioDto();
        public UsuarioDto UsuarioOperacao { get; set; } = new UsuarioDto();
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();
        public ICollection<TorneioDto> Torneios { get; set; } = new List<TorneioDto>();
        public ICollection<AtletaDto> Atletas { get; set; } = new List<AtletaDto>();
        public ICollection<AcademiaRedeSocialDto> AcademiaRedeSociais { get; set; } = new List<AcademiaRedeSocialDto>();
        public ICollection<EquipePontuacao> Pontuacoes { get; set; } = new List<EquipePontuacao>();
    }
}
