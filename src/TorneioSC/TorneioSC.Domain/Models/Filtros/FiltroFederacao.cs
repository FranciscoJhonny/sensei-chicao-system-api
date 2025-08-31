namespace TorneioSC.Domain.Models.Filtros
{
    public class FiltroFederacao
    {
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public int? MunicipioId { get; set; }
        public int? EstadoId { get; set; }
        public bool? Ativo { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
    }
}
