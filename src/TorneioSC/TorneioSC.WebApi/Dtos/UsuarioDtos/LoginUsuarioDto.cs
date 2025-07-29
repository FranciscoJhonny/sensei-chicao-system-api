namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    public class LoginUsuarioDto
    {
        /// <summary>
        /// Usuario logado no sistema 
        /// </summary>
        public string Usuario { get; set; }= string.Empty;

        /// <summary>
        /// Nome do Usuario Logado no sistema
        /// </summary>
        public string Senha { get; set; } = string.Empty;
    }
}

