using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.ResultadoDtos
{
    /// <summary>
    /// DTO que representa o resultado de uma inscrição em competição
    /// </summary>
    public class ResultadoDto
    {
        /// <summary>
        /// ID único do resultado
        /// </summary>
        public int ResultadoId { get; set; }

        /// <summary>
        /// ID da inscrição à qual o resultado está vinculado
        /// </summary>
        public int InscricaoId { get; set; }

        /// <summary>
        /// Posição alcançada na competição (ex: 1=1º lugar, 2=2º lugar, 3=3º lugar)
        /// </summary>
        public int Posicao { get; set; }

        /// <summary>
        /// Pontuação obtida na competição (opcional)
        /// </summary>
        public decimal? Pontuacao { get; set; }

        /// <summary>
        /// Indica se o resultado está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que registrou o resultado
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de registro do resultado no sistema
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
        /// Dados completos da inscrição vinculada ao resultado
        /// </summary>
        public InscricaoDto Inscricao { get; set; } = new InscricaoDto();

        /// <summary>
        /// Usuário que registrou o resultado
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no resultado
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}