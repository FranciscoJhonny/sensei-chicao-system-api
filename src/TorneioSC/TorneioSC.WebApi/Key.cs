namespace TorneioSC.WebApi
{
    /// <summary>
    /// Classe para armazenamento de chaves e segredos da aplicação
    /// </summary>
    /// <remarks>
    /// ATENÇÃO: Em ambiente de produção, recomenda-se armazenar segredos
    /// em variáveis de ambiente, Azure Key Vault ou outro serviço seguro.
    /// </remarks>
    public class Key
    {
        /// <summary>
        /// Chave secreta utilizada para assinatura de tokens JWT
        /// </summary>
        /// <remarks>
        /// Esta chave deve ter no mínimo 256 bits (32 caracteres) para segurança adequada.
        /// Em produção, NÃO armazene segredos hardcoded no código fonte.
        /// </remarks>
        public static string Secret = "RsyFMKHWs6J8hG9wuvY56Dw3EVELj6WjLNS89QDq9iGW29NW0QbF3yGb2CeDsAi9MJgQNd6wEgiBqlsHFp8RSWGXec8WHJD8LG93OSpAb8UjrUkSFCTimbuI3nfCCorYaBWvWxvsvFkw0j7soNi5Tfi0pNVzJNfvQ8znrqRhTEGUG7wCWSzWM7cZV0IgHQ6m";
    }
}