namespace TorneioSC.Domain.Models
{
    /// <summary>
    /// Representa a associação entre um evento e um torneio.
    /// Define quais torneios estão vinculados a um determinado evento.
    /// </summary>
    /// <remarks>
    /// A entidade <see cref="EventoTorneio"/> é usada para agrupar múltiplos torneios dentro de um mesmo evento.
    /// Por exemplo, um "Campeonato Estadual" (evento) pode conter torneios separados para:
    /// - Kata Individual
    /// - Kumite por categoria
    /// - Kata em Equipe
    /// 
    /// Essa estrutura permite gerenciar inscrições, estatísticas e resultados de forma organizada por torneio,
    /// enquanto mantém uma visão geral do evento principal.
    /// 
    /// É comumente usada em painéis de gestão, relatórios e geração de certificados.
    /// </remarks>   
    public class EventoTorneio
    {
        /// <summary>
        /// ID único do registro de vinculação entre evento e torneio.
        /// </summary>
        public int EventoTorneioId { get; set; }

        /// <summary>
        /// ID do evento ao qual o torneio está vinculado.
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// ID do torneio que faz parte do evento.
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// Indica se o vínculo está ativo (1) ou inativo (0).
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que incluiu o registro.
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data e hora de inclusão do registro.
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Tipo de operação realizada (ex: 'I' para Insert, 'U' para Update).
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação sobre o registro.
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data e hora da última operação realizada (atualização ou exclusão).
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Referência ao objeto <see cref="Evento"/> associado.
        /// Permite acesso direto aos dados do evento (nome, datas, responsável).
        /// </summary>
        public Evento Evento { get; set; } = new Evento();

        /// <summary>
        /// Referência ao objeto <see cref="Torneio"/> associado.
        /// Permite acesso direto às informações do torneio (categoria, modalidade, local).
        /// </summary>
        public Torneio Torneio { get; set; } = new Torneio();

        /// <summary>
        /// Usuário responsável pela inclusão do vínculo.
        /// </summary>
        public Usuario? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário responsável pela última alteração no vínculo.
        /// </summary>
        public Usuario? UsuarioOperacao { get; set; }
    }
}