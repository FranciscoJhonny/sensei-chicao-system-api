namespace TorneioSC.Domain.Models
{
    /// <summary>
    /// Representa uma modalidade de competição no torneio de karatê.
    /// Define o tipo de prova, como Kata (forma) ou Kumite (combate).
    /// </summary>
    /// <remarks>
    /// A entidade <see cref="Modalidade"/> é essencial para diferenciar os tipos de competição em um torneio.
    /// Cada categoria e inscrição está vinculada a uma modalidade específica.
    ///
    /// Modalidades comuns:
    /// - <b>Kata</b>: Avaliação de técnica, postura e precisão em formas pré-definidas.
    /// - <b>Kumite</b>: Combate entre dois atletas, com pontuação por golpes válidos.
    /// - Outras podem incluir Team Kata, Kumite por equipes, etc.
    ///
    /// A modalidade influencia diretamente as regras de categorização, pontuação e chaveamento.
    /// </remarks>    
    public class Modalidade
    {
        /// <summary>
        /// ID único da modalidade.
        /// </summary>
        public int ModalidadeId { get; set; }

        /// <summary>
        /// Nome da modalidade (ex: Kata, Kumite, Team Kata).
        /// </summary>
        public string ModalidadeNome { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a modalidade está ativa (1) ou inativa (0).
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que cadastrou a modalidade.
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data e hora do cadastro da modalidade.
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Tipo de operação realizada (ex: 'I' para Insert, 'U' para Update).
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última alteração.
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data e hora da última alteração no registro.
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Usuário responsável pela inclusão da modalidade.
        /// </summary>
        public Usuario? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário responsável pela última operação sobre a modalidade.
        /// </summary>
        public Usuario? UsuarioOperacao { get; set; }

        /// <summary>
        /// Lista de categorias vinculadas a esta modalidade.
        /// Permite acessar todas as categorias de Kata ou Kumite associadas.
        /// </summary>
        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

        /// <summary>
        /// Lista de inscrições vinculadas a esta modalidade.
        /// Usado para identificar todos os atletas inscritos em competições dessa modalidade.
        /// </summary>
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();

        /// <summary>
        /// Lista de chaveamentos vinculados a esta modalidade.
        /// Usado para organizar as fases de competição (eliminatórias, finais, etc.).
        /// </summary>
        public ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();
    }
}