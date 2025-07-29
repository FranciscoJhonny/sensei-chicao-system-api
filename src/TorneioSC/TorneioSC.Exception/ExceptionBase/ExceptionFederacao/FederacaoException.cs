namespace TorneioSC.Exception.ExceptionBase.ExceptionFederacao
{
    // Exceção base para erros relacionados a federação
    public class FederacaoException : TorneioExceptionBase
    {
        public FederacaoException(string message) : base(message) { }
        public FederacaoException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando uma federação não é encontrada
    public class FederacaoNaoEncontradaException : FederacaoException
    {
        public FederacaoNaoEncontradaException(int federacaoId)
            : base($"Federação com ID {federacaoId} não encontrada") { }

        public FederacaoNaoEncontradaException(string nome)
            : base($"Federação com nome {nome} não encontrada") { }
    }

    // Quando um CNPJ já está em uso
    public class CnpjEmUsoException : FederacaoException
    {
        public CnpjEmUsoException(string cnpj)
            : base($"O CNPJ {cnpj} já está em uso por outra federação") { }
    }

    // Quando há erro na operação de federação no banco
    public class OperacaoFederacaoException : FederacaoException
    {
        public OperacaoFederacaoException(string operation)
            : base($"Erro durante a operação de {operation} da federação") { }

        public OperacaoFederacaoException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da federação", inner) { }
    }

    // Exceção para validação de federação
    public class ValidacaoFederacaoException : FederacaoException
    {
        public List<string> ErrosValidacao { get; }

        public ValidacaoFederacaoException(List<string> erros)
            : base("Erros de validação da federação: " + string.Join(", ", erros))
        {
            ErrosValidacao = erros;
        }
    }

    // Exceção para falha na atualização
    public class AtualizacaoFederacaoException : FederacaoException
    {
        public AtualizacaoFederacaoException(int federacaoId)
            : base($"Falha ao atualizar a federação com ID {federacaoId}") { }

        public AtualizacaoFederacaoException(int federacaoId, System.Exception inner)
            : base($"Falha ao atualizar a federação com ID {federacaoId}", inner) { }
    }

    // Exceção para falha na exclusão/inativação
    public class ExclusaoFederacaoException : FederacaoException
    {
        public ExclusaoFederacaoException(int federacaoId)
            : base($"Falha ao excluir/inativar a federação com ID {federacaoId}") { }

        public ExclusaoFederacaoException(int federacaoId, System.Exception inner)
            : base($"Falha ao excluir/inativar a federação com ID {federacaoId}", inner) { }
    }

    // Quando uma federação está inativa
    public class FederacaoInativaException : FederacaoException
    {
        public FederacaoInativaException(int federacaoId)
            : base($"Federação com ID {federacaoId} está inativa") { }
    }

    // Quando o CNPJ é inválido
    public class CnpjInvalidoException : FederacaoException
    {
        public CnpjInvalidoException(string cnpj)
            : base($"O CNPJ '{cnpj}' não está em um formato válido") { }
    }

    // Quando há erro ao vincular endereço
    public class VinculoEnderecoException : FederacaoException
    {
        public VinculoEnderecoException(int federacaoId, System.Exception inner)
            : base($"Falha ao vincular endereço à federação com ID {federacaoId}", inner) { }
    }

    // Quando há erro ao vincular telefone
    public class VinculoTelefoneException : FederacaoException
    {
        public VinculoTelefoneException(int federacaoId, System.Exception inner)
            : base($"Falha ao vincular telefone à federação com ID {federacaoId}", inner) { }
    }
}