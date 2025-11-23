using TorneioSC.WebApi.Dtos.ChaveamentoDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.ModalidadeDtos
{
    /// <summary>
    /// DTO que representa uma modalidade esportiva no sistema
    /// </summary>
    public class ModalidadeDto
    {
        /// <summary>
        /// ID único da modalidade
        /// </summary>
        public int ModalidadeId { get; set; }

        /// <summary>
        /// Nome da modalidade (ex: "Kata", "Kumite", "Fighting", "Formas")
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a modalidade está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu a modalidade
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão da modalidade no sistema
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
        /// Usuário que incluiu a modalidade no sistema
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação na modalidade
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }

        /// <summary>
        /// Lista de inscrições realizadas nesta modalidade
        /// </summary>
        public ICollection<InscricaoDto> Inscricoes { get; set; } = new List<InscricaoDto>();

        /// <summary>
        /// Lista de chaveamentos criados para esta modalidade
        /// </summary>
        public ICollection<ChaveamentoDto> Chaveamentos { get; set; } = new List<ChaveamentoDto>();
    }
}