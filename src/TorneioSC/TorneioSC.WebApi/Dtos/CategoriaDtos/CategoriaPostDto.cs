namespace TorneioSC.WebApi.Dtos.CategoriaDtos
{
    public class CategoriaPostDto
    {
        /// <summary>
        /// ID da modalidade associada (ex: 1 = Kata, 2 = Kumite).
        /// </summary>
        public int ModalidadeId { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;
        public int IdadeMin { get; set; }
        public int IdadeMax { get; set; }
        public char Sexo { get; set; }
        public decimal? PesoMin { get; set; }
        public decimal? PesoMax { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }
    }
}
