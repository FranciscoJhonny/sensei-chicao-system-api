using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoTelefoneDto
    {
        public int Federacao_TelefoneId { get; set; }
        public int TelefoneId { get; set; }
        public int FederacaoId { get; set; }
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public TelefoneDto Telefone { get; set; } = new TelefoneDto();
    }
}