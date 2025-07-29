namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    public class EnderecoDto
    {
        public int EnderecoId { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }
    }
}