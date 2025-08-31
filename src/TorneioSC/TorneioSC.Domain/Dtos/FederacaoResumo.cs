namespace TorneioSC.Domain.Dtos
{
    public class FederacaoResumo
    {
        public int FederacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
