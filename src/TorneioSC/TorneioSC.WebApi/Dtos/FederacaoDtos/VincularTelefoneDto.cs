namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO para vincular um telefone existente a uma federação
    /// </summary>
    public class VincularTelefoneDto
    {
        /// <summary>
        /// ID da federação à qual o telefone será vinculado
        /// </summary>
        public int FederacaoId { get; set; }

        /// <summary>
        /// ID do telefone existente que será vinculado à federação
        /// </summary>
        public int TelefoneId { get; set; }
    }
}