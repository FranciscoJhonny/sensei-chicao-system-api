using TorneioSC.WebApi.Dtos.MunicipioDtos;

namespace TorneioSC.WebApi.Dtos.EstadoDtos
{
    /// <summary>
    /// DTO que representa um estado (unidade federativa) no sistema
    /// </summary>
    public class EstadoDto
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

        /// <summary>
        /// Lista de municípios pertencentes a este estado
        /// </summary>
        public ICollection<MunicipioDto> Municipios { get; set; } = new List<MunicipioDto>();
    }
}