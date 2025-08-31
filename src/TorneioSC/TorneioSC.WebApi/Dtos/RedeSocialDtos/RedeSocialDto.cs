namespace TorneioSC.WebApi.Dtos.RedeSocialDtos
{
    public class RedeSocialDto
    {
        public int RedeSocialId { get; set; }
        public string PerfilUrl { get; set; } = string.Empty;  // URL completa do perfil        
        public int? UsuarioInclusaoId { get; set; }
    }
}