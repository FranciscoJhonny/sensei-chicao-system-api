namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    public class TelefonePutDto
    {
        public int TelefoneId { get; set; } // Adicionado!
        public string NumeroTelefone { get; set; } = string.Empty;
        public int TipoTelefoneId { get; set; }
        public bool Ativo { get; set; } = true;
        public int UsuarioAlteracaoId { get; set; }
    }
}