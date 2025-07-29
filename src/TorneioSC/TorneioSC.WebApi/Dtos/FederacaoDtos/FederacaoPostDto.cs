using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoPostDto
    {
        public string Nome { get; set; } = string.Empty;
        public int MunicipioId { get; set; }
        public string CNPJ { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Site { get; set; } = string.Empty;
        public DateTime DataFundacao { get; set; }
        public string Portaria { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        // Dados de endereço (opcional - pode ser em endpoint separado)
        public ICollection<EnderecoPostDto> Enderecos { get; set; } = new List<EnderecoPostDto>();
        // Dados de telefone (opcional - pode ser em endpoint separado)
        public ICollection<TelefonePostDto> Telefones { get; set; } = new List<TelefonePostDto>();
    }
}