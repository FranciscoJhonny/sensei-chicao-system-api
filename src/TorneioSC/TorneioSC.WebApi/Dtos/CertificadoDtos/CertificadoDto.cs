using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.CertificadoDtos
{
    public class CertificadoDto
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

        public InscricaoDto Inscricao { get; set; } = new InscricaoDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}
