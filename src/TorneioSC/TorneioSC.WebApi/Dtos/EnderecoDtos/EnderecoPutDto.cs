namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    /// <summary>
    /// DTO para atualização de um endereço existente
    /// </summary>
    public class EnderecoPutDto
    {
        /// <summary>
        /// ID único do endereço (0 para novo endereço, > 0 para endereço existente)
        /// </summary>
        public int EnderecoId { get; set; }

        /// <summary>
        /// Nome da rua, avenida, praça, etc.
        /// </summary>
        public string Logradouro { get; set; } = string.Empty;

        /// <summary>
        /// Número do endereço
        /// </summary>
        public string Numero { get; set; } = string.Empty;

        /// <summary>
        /// Complemento do endereço (apartamento, bloco, sala, etc.)
        /// </summary>
        public string Complemento { get; set; } = string.Empty;

        /// <summary>
        /// Código Postal (CEP) no formato XXXXX-XXX
        /// </summary>
        public string Cep { get; set; } = string.Empty;

        /// <summary>
        /// Bairro do endereço
        /// </summary>
        public string Bairro { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o endereço está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que está realizando a alteração
        /// </summary>
        public int UsuarioAlteracaoId { get; set; }
    }
}