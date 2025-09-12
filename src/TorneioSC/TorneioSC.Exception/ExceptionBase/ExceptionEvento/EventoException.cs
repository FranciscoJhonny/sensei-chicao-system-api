using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;

namespace TorneioSC.Exception.ExceptionBase.ExceptionEvento
{
    public class EventoException : TorneioExceptionBase
    {
        public EventoException(string message) : base(message) { }
        public EventoException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Exceção para falha na atualização
    public class AtualizacaoEventoException : EventoException
    {
        public int EventoId { get; }

        public AtualizacaoEventoException(int eventoId)
            : base($"Falha ao atualizar o evento com ID {eventoId}")
        {
            EventoId = eventoId;
        }

        public AtualizacaoEventoException(int eventoId, System.Exception inner)
            : base($"Falha ao atualizar o evento com ID {eventoId}", inner)
        {
            EventoId = eventoId;
        }
    }

    // Exceção para falha na exclusão/inativação
    public class ExclusaoEventoException : EventoException
    {
        public int EventoId { get; }

        public ExclusaoEventoException(int eventoId)
            : base($"Falha ao excluir/inativar o evento com ID {eventoId}")
        {
            EventoId = eventoId;
        }

        public ExclusaoEventoException(int eventoId, System.Exception inner)
            : base($"Falha ao excluir/inativar o evento com ID {eventoId}", inner)
        {
            EventoId = eventoId;
        }
    }

    public class EventoNaoEncontradoException : EventoException
    {
        public EventoNaoEncontradoException(int eventoId)
            : base($"Evento com ID {eventoId} não foi encontrado.") { }
    }
    // Quando há erro na operação de evento no banco
    public class OperacaoEventoException : EventoException
    {
        public OperacaoEventoException(string operation)
            : base($"Erro durante a operação de {operation} da evento") { }

        public OperacaoEventoException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da evento", inner) { }
    }
    public class ValidacaoEventoException : EventoException
    {
        public ValidacaoEventoException(string message) : base(message) { }
    }    
}
