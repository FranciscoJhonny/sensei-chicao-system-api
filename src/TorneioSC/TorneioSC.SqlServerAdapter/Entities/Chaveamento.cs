using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Chaveamento
{
    public int ChaveamentoID { get; set; }

    public int TorneioID { get; set; }

    public int CategoriaID { get; set; }

    public int ModalidadeID { get; set; }

    public string? DadosChave { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Modalidade Modalidade { get; set; } = null!;

    public virtual Torneio Torneio { get; set; } = null!;
}
