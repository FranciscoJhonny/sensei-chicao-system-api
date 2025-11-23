using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.CategoriaDtos
{
    /// <summary>
    /// DTO para atualização de uma categoria de torneio
    /// </summary>
    public class CategoriaPutDto
    {
        /// <summary>
        /// ID único da categoria a ser atualizada
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Nome da categoria (ex: "Mirim", "Juvenil", "Adulto", "Peso Pesado")
        /// </summary>
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da categoria não pode exceder 100 caracteres.")]
        public string NomeCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Idade mínima permitida para a categoria (opcional)
        /// </summary>
        public int? IdadeMin { get; set; }

        /// <summary>
        /// Idade máxima permitida para a categoria (opcional)
        /// </summary>
        public int? IdadeMax { get; set; }

        /// <summary>
        /// Sexo permitido para a categoria (M=Masculino, F=Feminino, null=Misto)
        /// </summary>
        [StringLength(1, ErrorMessage = "O sexo deve ser 'M', 'F' ou vazio.")]
        public string? Sexo { get; set; }

        /// <summary>
        /// Peso mínimo permitido para a categoria (opcional - usado para categorias por peso)
        /// </summary>
        public decimal? PesoMin { get; set; }

        /// <summary>
        /// Peso máximo permitido para a categoria (opcional - usado para categorias por peso)
        /// </summary>
        public decimal? PesoMax { get; set; }

        /// <summary>
        /// ID da modalidade associada (ex: 1 = Kata, 2 = Kumite)
        /// </summary>
        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        public int ModalidadeId { get; set; }

        /// <summary>
        /// Indica se a categoria está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; }
    }
}