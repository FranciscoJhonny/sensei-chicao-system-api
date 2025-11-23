namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO para vincular um endereço existente a uma federação
    /// </summary>
    public class VincularEnderecoDto
    {
        /// <summary>
        /// ID da federação à qual o endereço será vinculado
        /// </summary>
        public int FederacaoId { get; set; }

        /// <summary>
        /// ID do endereço existente que será vinculado à federação
        /// </summary>
        public int EnderecoId { get; set; }
    }
}