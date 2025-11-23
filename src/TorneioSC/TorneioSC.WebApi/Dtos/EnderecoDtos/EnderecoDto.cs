namespace TorneioSC.WebApi.Dtos.EnderecoDtos
{
    /// <summary>
    /// DTO que representa um endereço no sistema
    /// </summary>
    public class EnderecoDto
    {
        /// <summary>
        /// ID único do endereço
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
        public string? Complemento { get; set; }

        /// <summary>
        /// Código Postal (CEP) no formato XXXXX-XXX
        /// </summary>
        public string? Cep { get; set; }

        /// <summary>
        /// Bairro do endereço
        /// </summary>
        public string? Bairro { get; set; }

        /// <summary>
        /// Indica se o endereço está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que incluiu o endereço
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão do endereço no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; }

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime? DataOperacao { get; set; }
    }
}