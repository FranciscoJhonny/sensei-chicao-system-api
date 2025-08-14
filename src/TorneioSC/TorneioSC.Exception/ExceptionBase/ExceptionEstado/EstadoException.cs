using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorneioSC.Exception.ExceptionBase.ExceptionEstado
{
    public class EstadoException : TorneioExceptionBase
    {
        public EstadoException(string message) : base(message) { }
        public EstadoException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando há erro na operação de estado no banco
    public class OperacaoEstadoException : EstadoException
    {
        public OperacaoEstadoException(string operation)
            : base($"Erro durante a operação de {operation} da estado") { }

        public OperacaoEstadoException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da estado", inner) { }
    }
}
