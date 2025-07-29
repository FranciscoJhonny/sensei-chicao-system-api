namespace TorneioSC.Exception.ExceptionBase.ExceptionUsuario
{
    // Exceção base para erros relacionados a usuário
    public class UsuarioException : TorneioExceptionBase
    {
        public UsuarioException(string message) : base(message) { }
        public UsuarioException(string message, System.Exception innerException) : base(message, innerException) { }
    }
    // Quando um usuário não é encontrado
    public class UsuarioNaoEncontradoException : UsuarioException
    {
        public UsuarioNaoEncontradoException(int usuarioId)
            : base($"Usuário com ID {usuarioId} não encontrado") { }

        public UsuarioNaoEncontradoException(string email)
            : base($"Usuário com email {email} não encontrado") { }
    }

    // Quando credenciais são inválidas
    public class CredenciaisInvalidasException : UsuarioException
    {
        public CredenciaisInvalidasException()
            : base("Usuário ou senha invalido") { }
    }

    // Quando um usuário está inativo
    public class UsuarioInativoException : UsuarioException
    {
        public UsuarioInativoException(int usuarioId)
            : base($"Usuário com ID {usuarioId} está inativo") { }
    }

    // Quando um email já está em uso
    public class EmailEmUsoException : UsuarioException
    {
        public EmailEmUsoException(string email)
            : base($"O email {email} já está em uso por outro usuário") { }
    }

    // Quando há erro na operação de usuário no banco
    public class OperacaoUsuarioException : UsuarioException
    {
        public OperacaoUsuarioException(string operation)
            : base($"Erro durante a operação de {operation} do usuário") { }

        public OperacaoUsuarioException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} do usuário", inner) { }
    }
    public class ValidacaoUsuarioException : UsuarioException
    {
        public List<string> ErrosValidacao { get; }

        public ValidacaoUsuarioException(List<string> erros)
            : base("Erros de validação do usuário: " + string.Join(", ", erros))
        {
            ErrosValidacao = erros;
        }
    }
    // Exceção para falha na atualização
    public class AtualizacaoUsuarioException : UsuarioException
    {
        public AtualizacaoUsuarioException(int usuarioId)
            : base($"Falha ao atualizar o usuário com ID {usuarioId}") { }

        public AtualizacaoUsuarioException(int usuarioId, System.Exception inner)
            : base($"Falha ao atualizar o usuário com ID {usuarioId}", inner) { }
    }

    // Exceção para falha na exclusão/inativação
    public class ExclusaoUsuarioException : UsuarioException
    {
        public ExclusaoUsuarioException(int usuarioId)
            : base($"Falha ao excluir/inativar o usuário com ID {usuarioId}") { }

        public ExclusaoUsuarioException(int usuarioId, System.Exception inner)
            : base($"Falha ao excluir/inativar o usuário com ID {usuarioId}", inner) { }
    }
    public class EmailInvalidoException : UsuarioException
    {
        public EmailInvalidoException(string email)
            : base($"O email '{email}' não está em um formato válido") { }
    }
    public class SalvarTokenRedefinicaoException : UsuarioException
    {
        public SalvarTokenRedefinicaoException(int usuarioId, System.Exception inner)
            : base($"Falha ao atualizar o usuário com ID {usuarioId}", inner) { }
    }
    public class TokenNaoEncontradoException : UsuarioException
    {
        public TokenNaoEncontradoException(string token)
            : base($"Usuário com o token {token} não encontrado") { }

    }
}
