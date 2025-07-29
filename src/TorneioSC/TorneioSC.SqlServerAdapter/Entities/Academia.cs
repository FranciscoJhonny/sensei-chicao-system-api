using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Academia
{
    public int AcademiaID { get; set; }

    public string Nome { get; set; } = null!;

    public int? FederacaoID { get; set; }

    public int EstadoID { get; set; }

    public bool Ativo { get; set; }

    public int UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string NaturezaOperacao { get; set; } = null!;

    public int UsuarioOperacao { get; set; }

    public DateTime DataOperacao { get; set; }

    public virtual ICollection<AcademiaTorneio> AcademiaTorneios { get; set; } = new List<AcademiaTorneio>();

    public virtual ICollection<Atleta> Atleta { get; set; } = new List<Atleta>();

    public virtual ICollection<EquipePontuacao> EquipePontuacaos { get; set; } = new List<EquipePontuacao>();

    public virtual Estado Estado { get; set; } = null!;

    public virtual Federacao? Federacao { get; set; }
}
