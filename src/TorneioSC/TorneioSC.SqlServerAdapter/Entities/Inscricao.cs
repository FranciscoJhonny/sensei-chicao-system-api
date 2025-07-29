using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Inscricao
{
    public int InscricaoID { get; set; }

    public int AtletaID { get; set; }

    public int TorneioID { get; set; }

    public int CategoriaID { get; set; }

    public int ModalidadeID { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Atleta Atleta { get; set; } = null!;

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    public virtual Modalidade Modalidade { get; set; } = null!;

    public virtual ICollection<Resultado> Resultados { get; set; } = new List<Resultado>();

    public virtual Torneio Torneio { get; set; } = null!;
}
