using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EstatisticaPosEventoDtos
{
    /// <summary>
    /// DTO que representa estatísticas geradas após a conclusão de um evento/torneio
    /// </summary>
    public class EstatisticaPosEventoDto
    {
        /// <summary>
        /// ID único da estatística
        /// </summary>
        public int EstatisticaId { get; set; }

        /// <summary>
        /// ID do torneio ao qual as estatísticas se referem
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// Número total de medalhas distribuídas no torneio
        /// </summary>
        public int MedalhasDistribuidas { get; set; }

        /// <summary>
        /// Número total de certificados emitidos no torneio
        /// </summary>
        public int CertificadosEmitidos { get; set; }

        /// <summary>
        /// Número total de lutas realizadas no torneio
        /// </summary>
        public int TotalLutas { get; set; }

        /// <summary>
        /// Data e hora em que as estatísticas foram geradas
        /// </summary>
        public DateTime GeradoEm { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se o registro de estatística está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que gerou as estatísticas
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão das estatísticas no sistema
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
        /// Torneio ao qual as estatísticas se referem
        /// </summary>
        public TorneioDto Torneio { get; set; } = new TorneioDto();

        /// <summary>
        /// Usuário que gerou as estatísticas
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação nas estatísticas
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}