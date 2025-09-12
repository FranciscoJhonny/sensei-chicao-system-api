using TorneioSC.WebApi.Dtos.CategoriaDtos;

namespace TorneioSC.WebApi.Dtos.TorneioDtos
{
    public class TorneioPostDto
    {
        public string NomeTorneio { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int MunicipioId { get; set; }
        public string? Contratante { get; set; } = string.Empty;  
        public int UsuarioInclusaoId { get; set; }        
        public ICollection<CategoriaPostDto> Categorias { get; set; } = new List<CategoriaPostDto>();
    }
}
