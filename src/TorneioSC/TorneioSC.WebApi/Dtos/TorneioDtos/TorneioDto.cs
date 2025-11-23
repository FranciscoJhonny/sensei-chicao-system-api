using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.CategoriaDtos;
using TorneioSC.WebApi.Dtos.ChaveamentoDtos;
using TorneioSC.WebApi.Dtos.EquipePontuacaoDtos;
using TorneioSC.WebApi.Dtos.EstatisticaPosEventoDtos;
using TorneioSC.WebApi.Dtos.EstatisticaPreEventoDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.TorneioDtos
{
    /// <summary>
    /// DTO que representa um torneio no sistema
    /// </summary>
    public class TorneioDto
    {
        /// <summary>
        /// ID único do torneio
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// Nome do torneio
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do torneio (ex: "Municipal", "Estadual", "Nacional", "Internacional")
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de início do torneio
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término do torneio
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// ID do município onde o torneio será realizado
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// Nome do contratante/organizador do torneio (opcional)
        /// </summary>
        public string? Contratante { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o torneio está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que criou o torneio
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do torneio no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Município onde o torneio será realizado
        /// </summary>
        public MunicipioDto Municipio { get; set; } = new MunicipioDto();

        /// <summary>
        /// Usuário que criou o torneio
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no torneio
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }

        /// <summary>
        /// Lista de academias participantes do torneio
        /// </summary>
        public ICollection<AcademiaDto> Academias { get; set; } = new List<AcademiaDto>();

        /// <summary>
        /// Lista de inscrições no torneio
        /// </summary>
        public ICollection<InscricaoDto> Inscricoes { get; set; } = new List<InscricaoDto>();

        /// <summary>
        /// Lista de chaveamentos do torneio
        /// </summary>
        public ICollection<ChaveamentoDto> Chaveamentos { get; set; } = new List<ChaveamentoDto>();

        /// <summary>
        /// Lista de pontuações das equipes no torneio
        /// </summary>
        public ICollection<EquipePontuacaoDto> EquipePontuacoes { get; set; } = new List<EquipePontuacaoDto>();

        /// <summary>
        /// Lista de estatísticas geradas após o torneio
        /// </summary>
        public ICollection<EstatisticaPosEventoDto> EstatisticasPosEvento { get; set; } = new List<EstatisticaPosEventoDto>();

        /// <summary>
        /// Lista de estatísticas geradas antes do torneio
        /// </summary>
        public ICollection<EstatisticaPreEventoDto> EstatisticasPreEvento { get; set; } = new List<EstatisticaPreEventoDto>();

        /// <summary>
        /// Lista de categorias disponíveis no torneio
        /// </summary>
        public ICollection<CategoriaDto> Categorias { get; set; } = new List<CategoriaDto>();
    }
}