using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TorneioSC.Domain.Models;

namespace TorneioSC.WebApi.Services
{
    public class TokenService
    {
        public static object GenerateToken(UsuarioLogadoVM usuario)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("usuarioId", usuario.UsuarioId.ToString()),
                    new Claim("descricaoPerfil", usuario.DescricaoPerfil),
                    new Claim(ClaimTypes.Role, usuario.DescricaoPerfil),
                    new Claim("email", usuario.Email),
                    new Claim("nome", usuario.Nome),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                access_token = tokenString
            };

        }
    }
}
