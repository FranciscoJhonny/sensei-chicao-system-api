namespace TorneioSC.Domain.Models
{
    public class RedeSocial
    {
        public int RedeSocialId { get; set; }
        public string Nome { get; set; } = string.Empty;  // Ex: "Facebook", "Instagram"
        public string Url { get; set; } = string.Empty;   // URL base da rede social       
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}
