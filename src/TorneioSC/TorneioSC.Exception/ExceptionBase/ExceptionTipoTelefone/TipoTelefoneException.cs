namespace TorneioSC.Exception.ExceptionBase.ExceptionTipoTelefone
{
    public class TipoTelefoneException : TorneioExceptionBase
    {
        public TipoTelefoneException(string message) : base(message) { }
        public TipoTelefoneException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando há erro na operação de TipoTelefone no banco
    public class OperacaoTipoTelefoneException : TipoTelefoneException
    {
        public OperacaoTipoTelefoneException(string operation)
            : base($"Erro durante a operação de {operation} da Tipo Telefone") { }

        public OperacaoTipoTelefoneException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da Tipo Telefone", inner) { }
    }
}
