namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    /// <summary>
    /// DTO para atualização de um telefone existente
    /// </summary>
    public class TelefonePutDto
    {
        /// <summary>
        /// ID único do telefone a ser atualizado
        /// </summary>
        public int TelefoneId { get; set; }

        /// <summary>
        /// Número do telefone (formato: (XX) XXXXX-XXXX ou variações)
        /// </summary>
        public string NumeroTelefone { get; set; } = string.Empty;

        /// <summary>
        /// ID do tipo de telefone (ex: 1=Celular, 2=Residencial, 3=Comercial)
        /// </summary>
        public int TipoTelefoneId { get; set; }

        /// <summary>
        /// Indica se o telefone está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que está realizando a atualização
        /// </summary>
        public int UsuarioAlteracaoId { get; set; }
    }
}