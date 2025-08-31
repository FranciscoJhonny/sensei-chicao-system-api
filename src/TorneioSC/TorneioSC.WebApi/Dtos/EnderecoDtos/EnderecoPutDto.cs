namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    public class EnderecoPutDto
    {
        public int EnderecoId { get; set; } // Agora pode ser > 0 (existente) ou 0 (novo)
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int UsuarioAlteracaoId { get; set; }
    }
}