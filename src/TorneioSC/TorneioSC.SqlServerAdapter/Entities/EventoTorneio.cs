using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class EventoTorneio
{
    public int EventoTorneioID { get; set; }

    public int EventoID { get; set; }

    public int TorneioID { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Evento Evento { get; set; } = null!;

    public virtual Torneio Torneio { get; set; } = null!;
}
