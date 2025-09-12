namespace TorneioSC.Domain.Models
{
    /// <summary>
    /// Representa uma categoria de competição em um torneio de karatê.
    /// Define critérios como faixa etária, sexo, peso (para Kumite) e modalidade (Kata ou Kumite).
    /// </summary>
    /// <remarks>
    /// A categoria é fundamental para organizar os atletas em grupos equilibrados durante o torneio.
    /// Pode ser usada em uma ou ambas as modalidades (Kata e Kumite), dependendo da configuração do torneio.
    /// 
    /// Regras comuns:
    /// - Categorias de **Kata** geralmente não possuem limite de peso.
    /// - Categorias de **Kumite** possuem limites de peso (PesoMin e PesoMax).
    /// - Cada categoria está vinculada a uma modalidade específica.
    /// 
    /// A entidade é usada em inscrições, chaveamentos e resultados.
    /// </remarks>   
    public class Categoria
    {
        /// <summary>
        /// ID único da categoria.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// ID da modalidade associada (ex: 1 = Kata, 2 = Kumite).
        /// </summary>
        public int ModalidadeId { get; set; }

        /// <summary>
        /// Nome descritivo da categoria (ex: Sub 8 Masc, Adulto 75Kg Fem).
        /// </summary>
        public string NomeCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Idade mínima permitida para participar da categoria.
        /// </summary>
        public int IdadeMin { get; set; }

        /// <summary>
        /// Idade máxima permitida para participar da categoria.
        /// </summary>
        public int IdadeMax { get; set; }

        /// <summary>
        /// Sexo dos atletas na categoria ('M' = Masculino, 'F' = Feminino).
        /// </summary>
        public char Sexo { get; set; }

        /// <summary>
        /// Peso mínimo (em kg) para categorias de Kumite. Nulo em Kata.
        /// </summary>
        public decimal? PesoMin { get; set; }

        /// <summary>
        /// Peso máximo (em kg) para categorias de Kumite. Nulo em Kata.
        /// </summary>
        public decimal? PesoMax { get; set; }
        public string NomeModalidade { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a categoria está ativa (1) ou inativa (0).
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que cadastrou a categoria.
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data e hora do cadastro da categoria.
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Tipo de operação realizada (I=Insert, U=Update, D=Delete).
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
        /// Usuário responsável pela inclusão da categoria.
        /// </summary>
        public Usuario? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário responsável pela última operação sobre a categoria.
        /// </summary>
        public Usuario? UsuarioOperacao { get; set; }

        /// <summary>
        /// Modalidade associada à categoria (Kata ou Kumite).
        /// </summary>
        public Modalidade Modalidade { get; set; } = new Modalidade();

        /// <summary>
        /// Lista de inscrições vinculadas a esta categoria.
        /// Usado para acessar todos os atletas inscritos.
        /// </summary>
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();

        /// <summary>
        /// Lista de chaveamentos vinculados a esta categoria.
        /// Usado para gerar e acessar as chaves de competição.
        /// </summary>
        public ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();
    }
}