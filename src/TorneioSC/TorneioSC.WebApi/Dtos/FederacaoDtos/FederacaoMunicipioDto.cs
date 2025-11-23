using TorneioSC.WebApi.Dtos.AcademiaDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO que representa um município com informações relacionadas a federações
    /// </summary>
    public class FederacaoMunicipioDto
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

        /// <summary>
        /// Dados do estado ao qual o município pertence
        /// </summary>
        public FederacaoEstadoDto Estado { get; set; } = new FederacaoEstadoDto();

        /// <summary>
        /// Lista de academias localizadas neste município
        /// </summary>
        public ICollection<AcademiaDto> Academias { get; set; } = new List<AcademiaDto>();
    }
}