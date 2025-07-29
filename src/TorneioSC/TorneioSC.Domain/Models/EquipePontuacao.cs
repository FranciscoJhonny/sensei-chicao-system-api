namespace TorneioSC.Domain.Models
{
    public class EquipePontuacao
    {
        public int EquipePontuacaoId { get; set; }
        public int TorneioId { get; set; }
        public int AcademiaId { get; set; }
        public decimal PontuacaoTotal { get; set; } = 0;
        public int? PosicaoFinal { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Torneio Torneio { get; set; } = new Torneio();
        public Academia Academia { get; set; } = new Academia();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}