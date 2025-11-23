namespace TorneioSC.WebApi.Dtos.TipoTelefoneDtos
{
    /// <summary>
    /// DTO que representa um tipo de telefone no sistema
    /// </summary>
    public class TipoTelefoneDto
    {
        /// <summary>
        /// ID único do tipo de telefone
        /// </summary>
        public int TipoTelefoneId { get; set; }

        /// <summary>
        /// Descrição do tipo de telefone (ex: "Celular", "Residencial", "Comercial", "WhatsApp")
        /// </summary>
        public string? DescricaoTipoTelefone { get; set; }

        /// <summary>
        /// Indica se o tipo de telefone está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que incluiu o tipo de telefone
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão do tipo de telefone no sistema
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
    }
}