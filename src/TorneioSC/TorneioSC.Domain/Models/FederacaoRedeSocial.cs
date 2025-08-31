namespace TorneioSC.Domain.Models
{
    public class FederacaoRedeSocial
    {
        public int FederacaoRedeSocialId { get; set; }
        public int FederacaoId { get; set; }
        public int RedeSocialId { get; set; }
        public string PerfilUrl { get; set; } = string.Empty;  // URL completa do perfil
        public bool Ativo { get; set; }

        // Campos de auditoria
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        // Navegação
        public RedeSocial RedeSocial { get; set; } = new RedeSocial();
    }
}
