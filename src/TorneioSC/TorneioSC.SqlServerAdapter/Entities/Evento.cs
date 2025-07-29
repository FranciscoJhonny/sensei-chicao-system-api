using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Evento
{
    public int EventoID { get; set; }

    public string Nome { get; set; } = null!;

    public DateOnly DataInicio { get; set; }

    public DateOnly DataFim { get; set; }

    public string Local { get; set; } = null!;

    public string Responsavel { get; set; } = null!;

    public string? EmailResponsavel { get; set; }

    public string? TelefoneResponsavel { get; set; }

    public string? Observacoes { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual ICollection<EventoTorneio> EventoTorneios { get; set; } = new List<EventoTorneio>();
}
