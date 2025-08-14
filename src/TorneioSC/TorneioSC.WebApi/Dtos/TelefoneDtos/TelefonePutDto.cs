namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    public class TelefonePutDto
    {
        public string NumeroTelefone { get; set; } = string.Empty;
        public int TipoTelefoneId { get; set; }
        public int UsuarioAlteracaoId { get; set; }
    }
}