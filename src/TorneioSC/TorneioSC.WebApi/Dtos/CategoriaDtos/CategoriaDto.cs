namespace TorneioSC.WebApi.Dtos.CategoriaDtos
{
    /// <summary>
    /// DTO que representa uma categoria de competição no sistema
    /// </summary>
    public class CategoriaDto
    {
        /// <summary>
        /// Nome da categoria (ex: "Mirim", "Juvenil", "Adulto", "Peso Pesado")
        /// </summary>
        public string NomeCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Idade mínima permitida para a categoria
        /// </summary>
        public int IdadeMin { get; set; }

        /// <summary>
        /// Idade máxima permitida para a categoria
        /// </summary>
        public int IdadeMax { get; set; }

        /// <summary>
        /// Sexo permitido para a categoria (M=Masculino, F=Feminino, M/F=Misto)
        /// </summary>
        public char Sexo { get; set; }

        /// <summary>
        /// Peso mínimo permitido para a categoria (opcional)
        /// </summary>
        public decimal? PesoMin { get; set; }

        /// <summary>
        /// Peso máximo permitido para a categoria (opcional)
        /// </summary>
        public decimal? PesoMax { get; set; }

        /// <summary>
        /// Indica se a categoria está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu a categoria
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão da categoria no sistema
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
    }
}