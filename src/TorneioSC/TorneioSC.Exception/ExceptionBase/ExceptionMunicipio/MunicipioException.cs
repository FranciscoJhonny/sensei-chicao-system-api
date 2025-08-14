namespace TorneioSC.Exception.ExceptionBase.ExceptionMunicipio
{
    public class MunicipioException : TorneioExceptionBase
    {
        public MunicipioException(string message) : base(message) { }
        public MunicipioException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando há erro na operação de município no banco
    public class OperacaoMunicipioException : MunicipioException
    {
        public OperacaoMunicipioException(string operation)
            : base($"Erro durante a operação de {operation} da município") { }

        public OperacaoMunicipioException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da município", inner) { }
    }

}
