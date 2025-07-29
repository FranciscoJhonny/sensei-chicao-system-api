using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Estado
{
    public int EstadoID { get; set; }

    public string Nome { get; set; } = null!;

    public string UF { get; set; } = null!;

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual ICollection<Academia> Academia { get; set; } = new List<Academia>();

    public virtual ICollection<Federacao> Federacaos { get; set; } = new List<Federacao>();

    public virtual ICollection<Torneio> Torneios { get; set; } = new List<Torneio>();
}
