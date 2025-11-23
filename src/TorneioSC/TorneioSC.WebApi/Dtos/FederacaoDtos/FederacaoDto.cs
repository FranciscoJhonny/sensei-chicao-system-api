using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO que representa uma federação no sistema
    /// </summary>
    public class FederacaoDto
    {
        /// <summary>
        /// ID único da federação
        /// </summary>
        public int FederacaoId { get; set; }

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
        /// ID do usuário que incluiu a federação
        /// </summary>
        public int UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão da federação no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string NaturezaOperacao { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime DataOperacao { get; set; }

        /// <summary>
        /// Usuário que incluiu a federação no sistema
        /// </summary>
        public Usuario UsuarioInclusao { get; set; } = new Usuario();

        /// <summary>
        /// Usuário que realizou a última operação na federação
        /// </summary>
        public Usuario UsuarioOperacao { get; set; } = new Usuario();

        /// <summary>
        /// Município onde a federação está sediada
        /// </summary>
        public FederacaoMunicipioDto Municipio { get; set; } = new FederacaoMunicipioDto();

        /// <summary>
        /// Lista de endereços da federação
        /// </summary>
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();

        /// <summary>
        /// Lista de telefones da federação
        /// </summary>
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();

        /// <summary>
        /// Lista de redes sociais da federação
        /// </summary>
        public ICollection<FederacaoRedeSocialDto> FederacaoRedeSociais { get; set; } = new List<FederacaoRedeSocialDto>();
    }
}