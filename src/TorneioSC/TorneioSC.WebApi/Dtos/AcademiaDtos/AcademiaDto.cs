using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.AtletaDtos;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    /// <summary>
    /// DTO que representa uma academia no sistema
    /// </summary>
    public class AcademiaDto
    {
        /// <summary>
        /// ID único da academia
        /// </summary>
        public int AcademiaId { get; set; }

        /// <summary>
        /// Nome da academia
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// ID da federação à qual a academia está vinculada (opcional)
        /// </summary>
        public int? FederacaoId { get; set; }

        /// <summary>
        /// ID do município onde a academia está localizada
        /// </summary>
        public int MunicipioId { get; set; }

        /// <summary>
        /// CNPJ da academia
        /// </summary>
        public string Cnpj { get; set; } = string.Empty;

        /// <summary>
        /// Nome do responsável pela academia
        /// </summary>
        public string ResponsavelNome { get; set; } = string.Empty;

        /// <summary>
        /// CPF do responsável pela academia
        /// </summary>
        public string ResponsavelCpf { get; set; } = string.Empty;

        /// <summary>
        /// Email de contato da academia
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// URL da logo da academia
        /// </summary>
        public string LogoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da academia
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a academia está ativa no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu a academia
        /// </summary>
        public int UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão da academia no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string NaturezaOperacao { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime DataOperacao { get; set; }

        /// <summary>
        /// Federação à qual a academia está vinculada
        /// </summary>
        public FederacaoDto? Federacao { get; set; }

        /// <summary>
        /// Município onde a academia está localizada
        /// </summary>
        public MunicipioDto Municipio { get; set; } = new MunicipioDto();

        /// <summary>
        /// Usuário que incluiu a academia no sistema
        /// </summary>
        public UsuarioDto UsuarioInclusao { get; set; } = new UsuarioDto();

        /// <summary>
        /// Usuário que realizou a última operação na academia
        /// </summary>
        public UsuarioDto UsuarioOperacao { get; set; } = new UsuarioDto();

        /// <summary>
        /// Lista de endereços da academia
        /// </summary>
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();

        /// <summary>
        /// Lista de telefones da academia
        /// </summary>
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();

        /// <summary>
        /// Lista de torneios organizados pela academia
        /// </summary>
        public ICollection<TorneioDto> Torneios { get; set; } = new List<TorneioDto>();

        /// <summary>
        /// Lista de atletas vinculados à academia
        /// </summary>
        public ICollection<AtletaDto> Atletas { get; set; } = new List<AtletaDto>();

        /// <summary>
        /// Lista de redes sociais da academia
        /// </summary>
        public ICollection<AcademiaRedeSocialDto> AcademiaRedeSociais { get; set; } = new List<AcademiaRedeSocialDto>();

        /// <summary>
        /// Lista de pontuações da equipe em competições
        /// </summary>
        public ICollection<EquipePontuacao> Pontuacoes { get; set; } = new List<EquipePontuacao>();
    }
}