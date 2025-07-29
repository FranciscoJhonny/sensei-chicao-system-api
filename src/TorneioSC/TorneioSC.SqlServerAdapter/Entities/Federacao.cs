using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Federacao
{
    public int FederacaoID { get; set; }

    public string Nome { get; set; } = null!;

    public int EstadoID { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual ICollection<Academia> Academia { get; set; } = new List<Academia>();

    public virtual Estado Estado { get; set; } = null!;
}
