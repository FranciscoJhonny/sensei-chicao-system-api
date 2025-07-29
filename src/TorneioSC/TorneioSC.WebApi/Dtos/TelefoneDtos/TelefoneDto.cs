using TorneioSC.WebApi.Dtos.TipoTelefoneDtos;

namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    public class TelefoneDto
    {
        public int TelefoneId { get; set; }
        public string? NumeroTelefone { get; set; }
        public int? TipoTelefoneId { get; set; }
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public TipoTelefoneDto? TipoTelefone { get; set; }
    }
}