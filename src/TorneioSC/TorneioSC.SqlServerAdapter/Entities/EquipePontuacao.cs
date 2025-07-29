using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class EquipePontuacao
{
    public int EquipePontuacaoID { get; set; }

    public int TorneioID { get; set; }

    public int AcademiaID { get; set; }

    public decimal PontuacaoTotal { get; set; }

    public int? PosicaoFinal { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Academia Academia { get; set; } = null!;

    public virtual Torneio Torneio { get; set; } = null!;
}
