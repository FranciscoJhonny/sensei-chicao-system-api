public class EventoResumoDto
{
    public int EventoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Responsavel { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public int QuantidadeTorneios { get; set; }
}