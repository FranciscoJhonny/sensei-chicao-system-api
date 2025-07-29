namespace TorneioSC.Exception.ExceptionBase.ExceptionPerfil
{
    public class PerfilException : TorneioExceptionBase
    {
        public PerfilException(string message) : base(message) { }
        public PerfilException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando um perfil não é encontrado
    public class PerfilNaoEncontradoException : PerfilException
    {
        public PerfilNaoEncontradoException(int perfilID)
            : base($"Perfil com ID {perfilID} não encontrado") { }

    }
}
