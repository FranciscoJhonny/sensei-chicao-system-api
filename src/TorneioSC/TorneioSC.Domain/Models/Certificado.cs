namespace TorneioSC.Domain.Models
{
    public class Certificado
    {
        public int CertificadoId { get; set; }
        public int InscricaoId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime EmitidoEm { get; set; } = DateTime.Now;
        public byte[]? ArquivoPDF { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Inscricao Inscricao { get; set; } = new Inscricao();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}