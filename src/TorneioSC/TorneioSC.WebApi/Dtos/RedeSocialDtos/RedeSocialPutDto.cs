namespace TorneioSC.WebApi.Dtos.RedeSocialDtos
{
    public class RedeSocialPutDto
    {
        public int FederacaoRedeSocialId { get; set; }
        public int AcademiaRedeSocialId { get; set; }
        public int RedeSocialId { get; set; } // Tipo da rede (Instagram, Facebook...)
        public string PerfilUrl { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int UsuarioAlteracaoId { get; set; }
    }
}