using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoPutDto
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
        public int UsuarioOperacaoId { get; set; }
        public ICollection<EnderecoPutDto> Enderecos { get; set; } = new List<EnderecoPutDto>();
        public ICollection<TelefonePutDto> Telefones { get; set; } = new List<TelefonePutDto>();
    }
}