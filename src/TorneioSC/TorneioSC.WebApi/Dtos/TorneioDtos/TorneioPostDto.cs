using TorneioSC.WebApi.Dtos.CategoriaDtos;

namespace TorneioSC.WebApi.Dtos.TorneioDtos
{
    /// <summary>
    /// DTO para criação de um novo torneio
    /// </summary>
    public class TorneioPostDto
    {
        /// <summary>
        /// Nome do torneio
        /// </summary>
        public string NomeTorneio { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do torneio (ex: "Municipal", "Estadual", "Nacional", "Internacional")
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de início do torneio
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término do torneio
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// ID do município onde o torneio será realizado
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// Nome do contratante/organizador do torneio (opcional)
        /// </summary>
        public string? Contratante { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que está criando o torneio
        /// </summary>
        public int UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Lista de categorias que serão disponibilizadas no torneio
        /// </summary>
        public ICollection<CategoriaPostDto> Categorias { get; set; } = new List<CategoriaPostDto>();
    }
}