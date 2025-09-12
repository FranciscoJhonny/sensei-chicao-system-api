using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.CategoriaDtos
{
    /// <summary>
    /// DTO para atualização de uma categoria de torneio.
    /// </summary>
    public class CategoriaPutDto
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da categoria não pode exceder 100 caracteres.")]
        public string NomeCategoria { get; set; } = string.Empty;

        public int? IdadeMin { get; set; }
        public int? IdadeMax { get; set; }

        [StringLength(1, ErrorMessage = "O sexo deve ser 'M', 'F' ou vazio.")]
        public string? Sexo { get; set; }

        public decimal? PesoMin { get; set; }
        public decimal? PesoMax { get; set; }

        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        public int ModalidadeId { get; set; }

        public bool Ativo { get; set; }
    }
}