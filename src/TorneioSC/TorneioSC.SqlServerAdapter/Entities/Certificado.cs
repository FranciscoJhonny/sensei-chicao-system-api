using System;
using System.Collections.Generic;

namespace TorneioSC.SqlServerAdapter.Entities;

public partial class Certificado
{
    public int CertificadoID { get; set; }

    public int InscricaoID { get; set; }

    public string Tipo { get; set; } = null!;

    public DateTime EmitidoEm { get; set; }

    public byte[]? ArquivoPDF { get; set; }

    public bool Ativo { get; set; }

    public int? UsuarioInclusao { get; set; }

    public DateTime DataInclusao { get; set; }

    public string? NaturezaOperacao { get; set; }

    public int? UsuarioOperacao { get; set; }

    public DateTime? DataOperacao { get; set; }

    public virtual Inscricao Inscricao { get; set; } = null!;
}
