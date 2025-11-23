using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.CertificadoDtos
{
    /// <summary>
    /// DTO que representa um certificado no sistema
    /// </summary>
    public class CertificadoDto
    {
        /// <summary>
        /// ID único do certificado
        /// </summary>
        public int CertificadoId { get; set; }

        /// <summary>
        /// ID da inscrição associada ao certificado
        /// </summary>
        public int InscricaoId { get; set; }

        /// <summary>
        /// Tipo do certificado (ex: "Participação", "Campeão", "Vice-Campeão", "3º Lugar")
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que o certificado foi emitido
        /// </summary>
        public DateTime EmitidoEm { get; set; } = DateTime.Now;

        /// <summary>
        /// Arquivo PDF do certificado em formato binário (opcional)
        /// </summary>
        public byte[]? ArquivoPDF { get; set; }

        /// <summary>
        /// Indica se o certificado está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu o certificado
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão do certificado no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Inscrição associada ao certificado
        /// </summary>
        public InscricaoDto Inscricao { get; set; } = new InscricaoDto();

        /// <summary>
        /// Usuário que incluiu o certificado no sistema
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no certificado
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}