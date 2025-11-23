using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.AtletaDtos
{
    /// <summary>
    /// DTO que representa um atleta no sistema
    /// </summary>
    public class AtletaDto
    {
        /// <summary>
        /// ID único do atleta
        /// </summary>
        public int AtletaId { get; set; }

        /// <summary>
        /// Nome completo do atleta
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do atleta
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// Sexo do atleta (M=Masculino, F=Feminino)
        /// </summary>
        public char Sexo { get; set; }

        /// <summary>
        /// Peso do atleta em kg
        /// </summary>
        public decimal Peso { get; set; }

        /// <summary>
        /// ID da academia à qual o atleta está vinculado
        /// </summary>
        public int AcademiaId { get; set; }

        /// <summary>
        /// CPF do atleta (opcional)
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Indica se o atleta está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu o atleta
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão do atleta no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Academia à qual o atleta está vinculado
        /// </summary>
        public AcademiaDto Academia { get; set; } = new AcademiaDto();

        /// <summary>
        /// Usuário que incluiu o atleta no sistema
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no atleta
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }

        /// <summary>
        /// Lista de inscrições do atleta em torneios
        /// </summary>
        public ICollection<InscricaoDto> Inscricoes { get; set; } = new List<InscricaoDto>();
    }
}