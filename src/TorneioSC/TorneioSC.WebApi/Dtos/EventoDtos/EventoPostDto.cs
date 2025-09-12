using TorneioSC.WebApi.Dtos.TorneioDtos;

namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    public class EventoPostDto
    {
        public string NomeEvento { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Local { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string EmailResponsavel { get; set; } = string.Empty;
        public string TelefoneResponsavel { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public TorneioPostDto Torneio { get; set; } = new TorneioPostDto();
    }
}
