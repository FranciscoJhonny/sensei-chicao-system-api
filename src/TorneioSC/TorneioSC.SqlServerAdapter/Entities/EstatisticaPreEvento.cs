using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class EstatisticaPreEvento
{
    public int EstatisticaID { get; set; }

    public int TorneioID { get; set; }

    public int TotalAtletas { get; set; }

    public int TotalAcademias { get; set; }

    public int TotalCategorias { get; set; }

    public int TotalModalidades { get; set; }

    public int EstimativaMedalhas { get; set; }

    public DateTime GeradoEm { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Torneio Torneio { get; set; } = null!;
}
