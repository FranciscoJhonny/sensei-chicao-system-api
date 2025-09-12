using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.CategoriaDtos;

namespace TorneioSC.WebApi.Dtos.TorneioDtos
{
    /// <summary>
    /// DTO para atualização de um torneio associado ao evento.
    /// </summary>
    public class TorneioPutDto
    {
        [Required(ErrorMessage = "O nome do torneio é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome do torneio não pode exceder 150 caracteres.")]
        public string NomeTorneio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo do torneio é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tipo do torneio não pode exceder 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O município é obrigatório.")]
        public int MunicipioId { get; set; }

        [StringLength(150, ErrorMessage = "O nome do contratante não pode exceder 150 caracteres.")]
        public string? Contratante { get; set; }

        public bool Ativo { get; set; }

        public List<CategoriaPutDto> Categorias { get; set; } = new();
    }
}