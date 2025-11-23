using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO para atualização de uma federação existente
    /// </summary>
    public class FederacaoPutDto
    {
        /// <summary>
        /// ID único da federação a ser atualizada
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
        /// Indica se a federação está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que está realizando a atualização
        /// </summary>
        public int UsuarioAlteracaoId { get; set; }

        /// <summary>
        /// Lista de endereços da federação para sincronização
        /// </summary>
        public ICollection<EnderecoPutDto> Enderecos { get; set; } = new List<EnderecoPutDto>();

        /// <summary>
        /// Lista de telefones da federação para sincronização
        /// </summary>
        public ICollection<TelefonePutDto> Telefones { get; set; } = new List<TelefonePutDto>();

        /// <summary>
        /// Lista de redes sociais da federação para sincronização
        /// </summary>
        public ICollection<RedeSocialPutDto> RedesSociais { get; set; } = new List<RedeSocialPutDto>();
    }
}