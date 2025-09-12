public class FiltroEventoDto
{
    public string? Nome { get; set; }
    public DateTime? DataInicioDe { get; set; }
    public DateTime? DataInicioAte { get; set; }
    public DateTime? DataFimDe { get; set; }
    public DateTime? DataFimAte { get; set; }
    public string? Local { get; set; }
    public string? Responsavel { get; set; }
    public bool? Ativo { get; set; } = true;
    public int? MunicipioId { get; set; }
    public int? ModalidadeId { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
}