namespace TorneioSC.SqlServerAdapter.Entities
{
    public class Perfil
    {
        public int PerfilId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}
