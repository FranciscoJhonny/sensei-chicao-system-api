using TorneioSC.WebApi.Dtos.EnderecoDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO que representa o relacionamento entre uma federação e um endereço
    /// </summary>
    public class FederacaoEnderecoDto
    {
        /// <summary>
        /// ID único do relacionamento federação-endereço
        /// </summary>
        public int Federacao_EnderecoId { get; set; }

        /// <summary>
        /// ID da federação à qual o endereço está vinculado
        /// </summary>
        public int FederacaoId { get; set; }

        /// <summary>
        /// ID do endereço vinculado à federação
        /// </summary>
        public int EnderecoId { get; set; }

        /// <summary>
        /// Indica se o relacionamento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que criou o relacionamento
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do relacionamento no sistema
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

        /// <summary>
        /// Dados completos do endereço vinculado à federação
        /// </summary>
        public EnderecoDto Endereco { get; set; } = new EnderecoDto();
    }
}