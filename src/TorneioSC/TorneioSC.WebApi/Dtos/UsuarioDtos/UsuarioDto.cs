using TorneioSC.WebApi.Dtos.PerfilDtos;

namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public int PerfilId { get; set; }
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }
        public PerfilDto Perfil { get; set; } = new PerfilDto();
    }
}
