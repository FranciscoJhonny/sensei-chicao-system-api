namespace TorneioSC.Exception.ExceptionBase
{
    // Exceção base personalizada para o torneio
    public class TorneioExceptionBase : System.Exception
    {
        public TorneioExceptionBase() { }
        public TorneioExceptionBase(string message) : base(message) { }
        public TorneioExceptionBase(string message, System.Exception innerException)
            : base(message, innerException) { }

        // Se precisar de serialização (para cenários mais avançados)
        protected TorneioExceptionBase(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

//namespace TorneioSC.Exception.ExceptionBase
//{
//    // Exceção base personalizada para o torneio
//    public class TorneioExceptionBase : System.Exception
//    {
//        public TorneioExceptionBase() { }
//        public TorneioExceptionBase(string message) : base(message) { }
//        public TorneioExceptionBase(string message, System.Exception innerException)
//            : base(message, innerException) { }

//        // Construtor de serialização removido - não é mais necessário na maioria dos casos
//        // A serialização moderna é tratada automaticamente pelo .NET
//    }
//}