using TorneioSC.WebApi.Dtos.CategoriaDtos;
using TorneioSC.WebApi.Dtos.ModalidadeDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.ChaveamentoDtos
{
    /// <summary>
    /// DTO que representa um chaveamento de competição no sistema
    /// </summary>
    public class ChaveamentoDto
    {
        /// <summary>
        /// ID único do chaveamento
        /// </summary>
        public int ChaveamentoId { get; set; }

        /// <summary>
        /// ID do torneio ao qual o chaveamento pertence
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// ID da categoria do chaveamento
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// ID da modalidade do chaveamento
        /// </summary>
        public int ModalidadeId { get; set; }

        /// <summary>
        /// Dados do chaveamento em formato JSON ou estrutura específica (opcional)
        /// </summary>
        public string? DadosChave { get; set; }

        /// <summary>
        /// Indica se o chaveamento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que criou o chaveamento
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do chaveamento
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
        /// Torneio ao qual o chaveamento pertence
        /// </summary>
        public TorneioDto Torneio { get; set; } = new TorneioDto();

        /// <summary>
        /// Categoria do chaveamento
        /// </summary>
        public CategoriaDto Categoria { get; set; } = new CategoriaDto();

        /// <summary>
        /// Modalidade do chaveamento
        /// </summary>
        public ModalidadeDto Modalidade { get; set; } = new ModalidadeDto();

        /// <summary>
        /// Usuário que criou o chaveamento
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no chaveamento
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}