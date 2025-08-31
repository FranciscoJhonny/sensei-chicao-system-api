namespace TorneioSC.Exception.ExceptionBase.ExceptionAcademia
{
    // Exceção base para erros relacionados a academia
    public class AcademiaException : TorneioExceptionBase
    {
        public AcademiaException(string message) : base(message) { }
        public AcademiaException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    // Quando uma academia não é encontrada
    public class AcademiaNaoEncontradaException : AcademiaException
    {
        public AcademiaNaoEncontradaException(int academiaId)
            : base($"academia com ID {academiaId} não encontrada") { }

        public AcademiaNaoEncontradaException(string nome)
            : base($"academia com nome {nome} não encontrada") { }
    }

    // Quando um CNPJ já está em uso
    public class CnpjEmUsoException : AcademiaException
    {
        public CnpjEmUsoException(string cnpj)
            : base($"O CNPJ {cnpj} já está em uso por outra academia") { }
    }

    // Quando há erro na operação de academia no banco
    public class OperacaoAcademiaException : AcademiaException
    {
        public OperacaoAcademiaException(string operation)
            : base($"Erro durante a operação de {operation} da academia") { }

        public OperacaoAcademiaException(string operation, System.Exception inner)
            : base($"Erro durante a operação de {operation} da academia", inner) { }
    }

    // Exceção para validação de academia
    public class ValidacaoAcademiaException : AcademiaException
    {
        public List<string> ErrosValidacao { get; }

        public ValidacaoAcademiaException(List<string> erros)
            : base("Erros de validação da academia: " + string.Join(", ", erros))
        {
            ErrosValidacao = erros;
        }
    }

    // Exceção para falha na atualização
    public class AtualizacaoAcademiaException : AcademiaException
    {
        public AtualizacaoAcademiaException(int academiaId)
            : base($"Falha ao atualizar a academia com ID {academiaId}") { }

        public AtualizacaoAcademiaException(int academiaId, System.Exception inner)
            : base($"Falha ao atualizar a academia com ID {academiaId}", inner) { }
    }

    // Exceção para falha na exclusão/inativação
    public class ExclusaoAcademiaException : AcademiaException
    {
        public ExclusaoAcademiaException(int academiaId)
            : base($"Falha ao excluir/inativar a academia com ID {academiaId}") { }

        public ExclusaoAcademiaException(int academiaId, System.Exception inner)
            : base($"Falha ao excluir/inativar a academia com ID {academiaId}", inner) { }
    }

    // Quando uma academia está inativa
    public class AcademiaInativaException : AcademiaException
    {
        public AcademiaInativaException(int academiaId)
            : base($"academia com ID {academiaId} está inativa") { }
    }

    // Quando o CNPJ é inválido
    public class CnpjInvalidoException : AcademiaException
    {
        public CnpjInvalidoException(string cnpj)
            : base($"O CNPJ '{cnpj}' não está em um formato válido") { }
    }

    // Quando há erro ao vincular endereço
    public class VinculoEnderecoException : AcademiaException
    {
        public VinculoEnderecoException(int academiaId, System.Exception inner)
            : base($"Falha ao vincular endereço à academia com ID {academiaId}", inner) { }
    }

    // Quando há erro ao vincular telefone
    public class VinculoTelefoneException : AcademiaException
    {
        public VinculoTelefoneException(int academiaId, System.Exception inner)
            : base($"Falha ao vincular telefone à academia com ID {academiaId}", inner) { }
    }
}