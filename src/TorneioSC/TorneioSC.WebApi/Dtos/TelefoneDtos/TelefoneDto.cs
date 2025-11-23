using TorneioSC.WebApi.Dtos.TipoTelefoneDtos;

namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    /// <summary>
    /// DTO que representa um telefone no sistema
    /// </summary>
    public class TelefoneDto
    {
        /// <summary>
        /// ID único do telefone
        /// </summary>
        public int TelefoneId { get; set; }

        /// <summary>
        /// Número do telefone (formato: (XX) XXXXX-XXXX ou variações)
        /// </summary>
        public string? NumeroTelefone { get; set; }

        /// <summary>
        /// ID do tipo de telefone (ex: 1=Celular, 2=Residencial, 3=Comercial)
        /// </summary>
        public int? TipoTelefoneId { get; set; }

        /// <summary>
        /// Indica se o telefone está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que incluiu o telefone
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão do telefone no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; }

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
        /// Dados do tipo de telefone vinculado
        /// </summary>
        public TipoTelefoneDto? TipoTelefone { get; set; }
    }
}