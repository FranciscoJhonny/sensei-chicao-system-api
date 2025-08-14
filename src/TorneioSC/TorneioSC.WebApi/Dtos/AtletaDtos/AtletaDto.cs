using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.AtletaDtos
{
    public class AtletaDto
    {
        public int AtletaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public decimal Peso { get; set; }
        public int AcademiaId { get; set; }
        public string? CPF { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public AcademiaDto Academia { get; set; } = new AcademiaDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
        public ICollection<InscricaoDto> Inscricoes { get; set; } = new List<InscricaoDto>();
    }
}
