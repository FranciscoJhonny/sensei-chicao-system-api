using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Modalidade
{
    public int ModalidadeID { get; set; }

    public string Nome { get; set; } = null!;

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
