using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoCompletaDto
    {
        public int FederacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateTime DataFundacao { get; set; }                
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
