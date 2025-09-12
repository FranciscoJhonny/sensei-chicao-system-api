namespace TorneioSC.Domain.Models
{
    /// <summary>
    /// Representa a associação entre um torneio e uma categoria disponível.
    /// Define quais categorias estão habilitadas para um determinado torneio.
    /// </summary>
    /// <remarks>
    /// A entidade <see cref="TorneioCategoria"/> é fundamental para controlar 
    /// quais categorias podem ser usadas em um torneio específico.
    /// 
    /// Permite que o organizador do torneio selecione apenas as categorias 
    /// que deseja oferecer, evitando inscrições em categorias não autorizadas.
    /// 
    /// É usada em conjunto com as entidades <see cref="Torneio"/> e <see cref="Categoria"/>
    /// para garantir integridade nas inscrições e chaveamentos.
    /// </remarks>    
    public class TorneioCategoria
    {
        /// <summary>
        /// ID único do registro de vinculação entre torneio e categoria.
        /// </summary>
        public int TorneioCategoriaId { get; set; }

        /// <summary>
        /// ID do torneio ao qual a categoria está vinculada.
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// ID da categoria disponível no torneio.
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Indica se o vínculo está ativo (1) ou inativo (0).
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que incluiu o registro.
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data e hora de inclusão do registro.
        /// </summary>
        public DateTime DataInclusao { get; set; }

        /// <summary>
        /// Tipo de operação realizada (ex: 'I' para Insert, 'U' para Update).
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação sobre o registro.
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data e hora da última operação realizada (atualização ou exclusão).
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Referência ao objeto <see cref="Torneio"/> associado.
        /// Permite acesso direto aos dados do torneio (nome, data, local, etc.).
        /// </summary>
        public Torneio Torneio { get; set; } = new Torneio();

        /// <summary>
        /// Referência ao objeto <see cref="Categoria"/> associado.
        /// Permite acesso direto às regras da categoria (idade, peso, sexo, modalidade).
        /// </summary>
        public Categoria Categoria { get; set; } = new Categoria();
    }
}