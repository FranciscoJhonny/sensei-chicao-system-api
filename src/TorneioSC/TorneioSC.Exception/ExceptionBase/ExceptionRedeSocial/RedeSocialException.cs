namespace TorneioSC.Exception.ExceptionBase.ExceptionRedeSocial
{
    public class RedeSocialException : TorneioExceptionBase
    {
        public RedeSocialException(string message) : base(message) { }
        public RedeSocialException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando há erro na operação de RedeSocial no banco
    public class OperacaoRedeSocialException : RedeSocialException
    {
        public OperacaoRedeSocialException(string operation)
            : base($"Erro durante a operação de {operation} da Rede Social") { }

        public OperacaoRedeSocialException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da Rede Social", inner) { }
    }
}
