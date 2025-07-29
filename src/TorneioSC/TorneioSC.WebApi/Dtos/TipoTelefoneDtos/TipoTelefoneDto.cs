namespace TorneioSC.WebApi.Dtos.TipoTelefoneDtos
{
    public class TipoTelefoneDto
    {
        public int TipoTelefoneId { get; set; }
        public string? DescricaoTipoTelefone { get; set; }
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }
    }
}