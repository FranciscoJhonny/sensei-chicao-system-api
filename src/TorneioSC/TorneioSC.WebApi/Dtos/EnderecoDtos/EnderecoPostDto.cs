namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    public class EnderecoPostDto
    {
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public int UsuarioInclusaoId { get; set; }
    }
}