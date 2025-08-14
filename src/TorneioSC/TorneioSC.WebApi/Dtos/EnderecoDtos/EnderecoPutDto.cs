namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    public class EnderecoPutDto
    {
        public int EnderecoId { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public int UsuarioAlteracaoId { get; set; }
    }
}