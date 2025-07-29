using TorneioSC.WebApi.Dtos.EnderecoDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoEnderecoDto
    {
        public int Federacao_EnderecoId { get; set; }
        public int FederacaoId { get; set; }
        public int EnderecoId { get; set; }
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public EnderecoDto Endereco { get; set; } = new EnderecoDto();
    }
}