using TorneioSC.WebApi.Dtos.AtletaDtos;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoAcademiaDtos
{
    /// <summary>
    /// DTO que representa uma academia com informações de federação para consultas específicas
    /// </summary>
    public class FederacaoAcademiaDto
    {
        /// <summary>
        /// ID único da academia
        /// </summary>
        public int AcademiaId { get; set; }

        /// <summary>
        /// Nome da academia
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// ID da federação à qual a academia está vinculada (opcional)
        /// </summary>
        public int? FederacaoId { get; set; }

        /// <summary>
        /// ID do município onde a academia está localizada
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// Dados da federação à qual a academia está vinculada
        /// </summary>
        public FederacaoDto? Federacao { get; set; }

        /// <summary>
        /// Dados do município onde a academia está localizada
        /// </summary>
        public FederacaoMunicipioDto Municipio { get; set; } = new FederacaoMunicipioDto();

        /// <summary>
        /// Lista de endereços da academia
        /// </summary>
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();

        /// <summary>
        /// Lista de telefones da academia
        /// </summary>
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();

        /// <summary>
        /// Lista de atletas vinculados à academia
        /// </summary>
        public ICollection<AtletaDto> Atletas { get; set; } = new List<AtletaDto>();
    }
}