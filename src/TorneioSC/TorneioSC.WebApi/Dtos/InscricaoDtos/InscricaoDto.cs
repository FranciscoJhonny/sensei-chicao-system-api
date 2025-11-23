using TorneioSC.WebApi.Dtos.AtletaDtos;
using TorneioSC.WebApi.Dtos.CategoriaDtos;
using TorneioSC.WebApi.Dtos.CertificadoDtos;
using TorneioSC.WebApi.Dtos.ModalidadeDtos;
using TorneioSC.WebApi.Dtos.ResultadoDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.InscricaoDtos
{
    /// <summary>
    /// DTO que representa uma inscrição de atleta em um torneio
    /// </summary>
    public class InscricaoDto
    {
        /// <summary>
        /// ID único da inscrição
        /// </summary>
        public int InscricaoId { get; set; }

        /// <summary>
        /// ID do atleta inscrito
        /// </summary>
        public int AtletaId { get; set; }

        /// <summary>
        /// ID do torneio no qual o atleta está inscrito
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// ID da categoria em que o atleta está inscrito
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// ID da modalidade em que o atleta está inscrito
        /// </summary>
        public int ModalidadeId { get; set; }

        /// <summary>
        /// Indica se a inscrição está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que realizou a inscrição
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de realização da inscrição
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
        /// Dados completos do atleta inscrito
        /// </summary>
        public AtletaDto Atleta { get; set; } = new AtletaDto();

        /// <summary>
        /// Dados completos do torneio
        /// </summary>
        public TorneioDto Torneio { get; set; } = new TorneioDto();

        /// <summary>
        /// Dados completos da categoria da inscrição
        /// </summary>
        public CategoriaDto Categoria { get; set; } = new CategoriaDto();

        /// <summary>
        /// Dados completos da modalidade da inscrição
        /// </summary>
        public ModalidadeDto Modalidade { get; set; } = new ModalidadeDto();

        /// <summary>
        /// Usuário que realizou a inscrição
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação na inscrição
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }

        /// <summary>
        /// Lista de certificados emitidos para esta inscrição
        /// </summary>
        public ICollection<CertificadoDto> Certificados { get; set; } = new List<CertificadoDto>();

        /// <summary>
        /// Lista de resultados alcançados nesta inscrição
        /// </summary>
        public ICollection<ResultadoDto> Resultados { get; set; } = new List<ResultadoDto>();
    }
}