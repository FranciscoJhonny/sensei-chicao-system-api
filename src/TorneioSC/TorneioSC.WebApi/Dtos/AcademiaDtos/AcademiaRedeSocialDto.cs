using TorneioSC.WebApi.Dtos.RedeSocialDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    public class AcademiaRedeSocialDto
    {
        public int AcademiaRedeSocialId { get; set; }
        public int AcademiaId { get; set; }
        public int RedeSocialId { get; set; }
        public string PerfilUrl { get; set; } = string.Empty;  // URL completa do perfil
        public bool Ativo { get; set; }

        // Campos de auditoria
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }
        public RedeSocialDto RedeSocial { get; set; } = new RedeSocialDto();
    }
}
