namespace TorneioSC.WebApi.Dtos.MunicipioDtos
{
    /// <summary>
    /// DTO que representa um município no sistema
    /// </summary>
    public class MunicipioDto
    {
        /// <summary>
        /// ID único do município
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// ID do estado ao qual o município pertence
        /// </summary>
        public int EstadoId { get; set; }

        /// <summary>
        /// Nome do município
        /// </summary>
        public string DescricaoMunicio { get; set; } = string.Empty;
    }
}