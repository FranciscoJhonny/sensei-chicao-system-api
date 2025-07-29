using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoDto
    {
        public int FederacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int MunicipioId { get; set; }
        public string CNPJ { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Site { get; set; } = string.Empty;
        public DateTime DataFundacao { get; set; }
        public string Portaria { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();
    }
}