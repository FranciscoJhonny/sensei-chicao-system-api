namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    public class TelefonePostDto
    {
        public string NumeroTelefone { get; set; } = string.Empty;
        public int TipoTelefoneId { get; set; }
        public int UsuarioInclusaoId { get; set; }
    }
}