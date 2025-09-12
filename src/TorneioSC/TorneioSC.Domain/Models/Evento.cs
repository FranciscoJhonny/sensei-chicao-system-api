namespace TorneioSC.Domain.Models
{
    /// <summary>
    /// Representa um evento esportivo que pode conter múltiplos torneios.
    /// Um evento é uma organização maior, como um campeonato estadual ou festival de iniciação.
    /// </summary>
    /// <remarks>
    /// A entidade <see cref="Evento"/> serve como agrupador de torneios relacionados.
    /// Por exemplo:
    /// - Um "Campeonato Gaúcho de Karatê" pode ser um evento com torneios separados para Kata e Kumite.
    /// - Um "Festival de Iniciação" pode conter torneios por faixa etária e estilo.
    ///
    /// O evento centraliza informações como:
    /// - Nome, datas e local do evento
    /// - Responsável pelo organização
    /// - Observações gerais (regras, horários, uniformes)
    /// - Vínculo com múltiplos torneios via <see cref="EventoTorneio"/>
    ///
    /// É útil para relatórios, certificados e gestão de inscrições em massa.
    /// </remarks>    
    public class Evento
    {
        /// <summary>
        /// ID único do evento.
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Nome do evento (ex: Campeonato Estadual, Festival de Iniciação).
        /// </summary>
        public string NomeEvento { get; set; } = string.Empty;

        /// <summary>
        /// Data de início do evento.
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de término do evento.
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Local onde o evento será realizado (nome do ginásio, cidade, etc.).
        /// </summary>
        public string Local { get; set; } = string.Empty;

        /// <summary>
        /// Nome da pessoa responsável pela organização do evento.
        /// </summary>
        public string Responsavel { get; set; } = string.Empty;

        /// <summary>
        /// E-mail de contato do responsável pelo evento.
        /// </summary>
        public string EmailResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato do responsável pelo evento.
        /// </summary>
        public string TelefoneResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Observações adicionais sobre o evento (regras, horários, restrições).
        /// </summary>
        public string? Observacoes { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o evento está ativo (1) ou inativo (0).
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que cadastrou o evento.
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data e hora do cadastro do evento.
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Tipo de operação realizada (ex: 'I' para Insert, 'U' para Update).
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última alteração.
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data e hora da última alteração no registro.
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Usuário responsável pela inclusão do evento.
        /// </summary>
        public Usuario? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário responsável pela última alteração no evento.
        /// </summary>
        public Usuario? UsuarioOperacao { get; set; }
        /// <summary>        
        /// Permite acesso direto aos dados do torneio (nome, data, local, etc.).
        /// </summary>
        public Torneio Torneio{ get; set; } = new Torneio();
    }
}