namespace TorneioSC.Domain.Dtos
{
    public class AcademiaResumo
    {
        public int AcademiaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
