using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.PerfilDtos
{
    public class PerfilDto
    {
        public int PerfilId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }
        public ICollection<UsuarioDto> Usuarios { get; set; } = new List<UsuarioDto>();
    }
}
