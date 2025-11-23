namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO que representa um estado para uso em contextos específicos de federação
    /// </summary>
    public class FederacaoEstadoDto
    {
        /// <summary>
        /// ID único do estado
        /// </summary>
        public int EstadoId { get; set; }

        /// <summary>
        /// Nome completo do estado (ex: "São Paulo", "Rio de Janeiro", "Minas Gerais")
        /// </summary>
        public string DescricaoEstado { get; set; } = string.Empty;

        /// <summary>
        /// Sigla do estado (ex: "SP", "RJ", "MG")
        /// </summary>
        public string Sigla { get; set; } = string.Empty;
    }
}