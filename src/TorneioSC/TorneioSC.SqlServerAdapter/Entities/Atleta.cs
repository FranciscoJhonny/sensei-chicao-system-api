using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Atleta
{
    public int AtletaID { get; set; }

    public string Nome { get; set; } = null!;

    public DateOnly DataNascimento { get; set; }

    public string Sexo { get; set; } = null!;

    public decimal Peso { get; set; }

    public int AcademiaID { get; set; }

    public string? CPF { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Academia Academia { get; set; } = null!;

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
