using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO que representa uma federação com todas as informações completas
    /// </summary>
    public class FederacaoCompletaDto
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
        /// Sigla da federação
        /// </summary>
        public string Sigla { get; set; } = string.Empty;

        /// <summary>
        /// CNPJ da federação
        /// </summary>
        public string Cnpj { get; set; } = string.Empty;

        /// <summary>
        /// Data de fundação da federação
        /// </summary>
        public DateTime DataFundacao { get; set; }

        /// <summary>
        /// Lista de endereços da federação
        /// </summary>
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();

        /// <summary>
        /// Lista de telefones da federação
        /// </summary>
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();

        /// <summary>
        /// Data de cadastro da federação no sistema
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Data da última atualização dos dados da federação (opcional)
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
    }
}