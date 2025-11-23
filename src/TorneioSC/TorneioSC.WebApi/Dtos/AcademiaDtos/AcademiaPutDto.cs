using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    /// <summary>
    /// DTO para atualização de uma academia existente
    /// </summary>
    public class AcademiaPutDto
    {
        /// <summary>
        /// ID único da academia a ser atualizada
        /// </summary>
        [Required(ErrorMessage = "ID é obrigatório")]
        public int AcademiaId { get; set; }

        /// <summary>
        /// Nome da academia
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome não pode exceder 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email de contato da academia
        /// </summary>
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// CNPJ da academia
        /// </summary>
        public string Cnpj { get; set; } = string.Empty;

        /// <summary>
        /// ID do município onde a academia está localizada
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// ID da federação à qual a academia está vinculada (opcional)
        /// </summary>
        public int? FederacaoId { get; set; }

        /// <summary>
        /// Nome do responsável pela academia
        /// </summary>
        public string ResponsavelNome { get; set; } = string.Empty;

        /// <summary>
        /// CPF do responsável pela academia
        /// </summary>
        public string ResponsavelCpf { get; set; } = string.Empty;

        /// <summary>
        /// URL da logo da academia
        /// </summary>
        public string LogoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da academia
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a academia está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que está realizando a alteração
        /// </summary>
        public int UsuarioAlteracaoId { get; set; }

        /// <summary>
        /// Lista de endereços da academia para sincronização
        /// </summary>
        public ICollection<EnderecoPutDto> Enderecos { get; set; } = new List<EnderecoPutDto>();

        /// <summary>
        /// Lista de telefones da academia para sincronização
        /// </summary>
        public ICollection<TelefonePutDto> Telefones { get; set; } = new List<TelefonePutDto>();

        /// <summary>
        /// Lista de redes sociais da academia para sincronização
        /// </summary>
        public ICollection<RedeSocialPutDto> RedesSociais { get; set; } = new List<RedeSocialPutDto>();
    }
}