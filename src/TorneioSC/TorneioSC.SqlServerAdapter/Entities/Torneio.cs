using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Torneio
{
    public int TorneioID { get; set; }

    public string Nome { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public DateOnly DataInicio { get; set; }

    public DateOnly DataFim { get; set; }

    public int EstadoID { get; set; }

    public string? Contratante { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual ICollection<AcademiaTorneio> AcademiaTorneios { get; set; } = new List<AcademiaTorneio>();

    public virtual ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();

    public virtual ICollection<EquipePontuacao> EquipePontuacaos { get; set; } = new List<EquipePontuacao>();

    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<EstatisticaPosEvento> EstatisticaPosEventos { get; set; } = new List<EstatisticaPosEvento>();

    public virtual ICollection<EstatisticaPreEvento> EstatisticaPreEventos { get; set; } = new List<EstatisticaPreEvento>();

    public virtual ICollection<EventoTorneio> EventoTorneios { get; set; } = new List<EventoTorneio>();

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
