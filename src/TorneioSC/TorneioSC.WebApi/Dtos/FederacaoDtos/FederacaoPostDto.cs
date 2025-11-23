using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO para criação de uma nova federação
    /// </summary>
    public class FederacaoPostDto
    {
        /// <summary>
        /// Nome completo da federação
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// ID do município onde a federação está sediada
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// CNPJ da federação
        /// </summary>
        public string Cnpj { get; set; } = string.Empty;

        /// <summary>
        /// Email de contato da federação
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Site oficial da federação
        /// </summary>
        public string Site { get; set; } = string.Empty;

        /// <summary>
        /// Data de fundação da federação
        /// </summary>
        public DateTime DataFundacao { get; set; }

        /// <summary>
        /// Número da portaria de reconhecimento da federação
        /// </summary>
        public string Portaria { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a federação está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que está criando a federação
        /// </summary>
        public int UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Lista de endereços da federação (opcional - pode ser adicionado em endpoint separado)
        /// </summary>
        public ICollection<EnderecoPostDto> Enderecos { get; set; } = new List<EnderecoPostDto>();

        /// <summary>
        /// Lista de telefones da federação (opcional - pode ser adicionado em endpoint separado)
        /// </summary>
        public ICollection<TelefonePostDto> Telefones { get; set; } = new List<TelefonePostDto>();

        /// <summary>
        /// Lista de redes sociais da federação
        /// </summary>
        public ICollection<RedeSocialPostDto> RedesSociais { get; set; } = new List<RedeSocialPostDto>();
    }
}