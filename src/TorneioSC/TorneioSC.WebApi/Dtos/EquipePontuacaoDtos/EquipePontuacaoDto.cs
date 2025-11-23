using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EquipePontuacaoDtos
{
    /// <summary>
    /// DTO que representa a pontuação de uma equipe/academia em um torneio
    /// </summary>
    public class EquipePontuacaoDto
    {
        /// <summary>
        /// ID único da pontuação da equipe
        /// </summary>
        public int EquipePontuacaoId { get; set; }

        /// <summary>
        /// ID do torneio no qual a equipe está participando
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// ID da academia/equipe que está recebendo a pontuação
        /// </summary>
        public int AcademiaId { get; set; }

        /// <summary>
        /// Pontuação total acumulada pela equipe no torneio
        /// </summary>
        public decimal PontuacaoTotal { get; set; } = 0;

        /// <summary>
        /// Posição final da equipe no ranking do torneio (opcional)
        /// </summary>
        public int? PosicaoFinal { get; set; }

        /// <summary>
        /// Indica se o registro de pontuação está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu a pontuação
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão da pontuação no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

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
        /// Torneio no qual a equipe está participando
        /// </summary>
        public TorneioDto Torneio { get; set; } = new TorneioDto();

        /// <summary>
        /// Academia/equipe que recebeu a pontuação
        /// </summary>
        public AcademiaDto Academia { get; set; } = new AcademiaDto();

        /// <summary>
        /// Usuário que incluiu a pontuação no sistema
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação na pontuação
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}