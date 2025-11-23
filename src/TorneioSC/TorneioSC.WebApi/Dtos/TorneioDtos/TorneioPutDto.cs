using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.CategoriaDtos;

namespace TorneioSC.WebApi.Dtos.TorneioDtos
{
    /// <summary>
    /// DTO para atualização de um torneio associado ao evento
    /// </summary>
    public class TorneioPutDto
    {
        /// <summary>
        /// Nome do torneio
        /// </summary>
        [Required(ErrorMessage = "O nome do torneio é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome do torneio não pode exceder 150 caracteres.")]
        public string NomeTorneio { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do torneio (ex: "Municipal", "Estadual", "Nacional", "Internacional")
        /// </summary>
        [Required(ErrorMessage = "O tipo do torneio é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tipo do torneio não pode exceder 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// ID do município onde o torneio será realizado
        /// </summary>
        [Required(ErrorMessage = "O município é obrigatório.")]
        public int MunicipioId { get; set; }

        /// <summary>
        /// Nome do contratante/organizador do torneio (opcional)
        /// </summary>
        [StringLength(150, ErrorMessage = "O nome do contratante não pode exceder 150 caracteres.")]
        public string? Contratante { get; set; }

        /// <summary>
        /// Indica se o torneio está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Lista de categorias do torneio para sincronização
        /// </summary>
        public List<CategoriaPutDto> Categorias { get; set; } = new();
    }
}