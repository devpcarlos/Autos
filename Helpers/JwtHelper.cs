using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    /// <summary>
    /// Helper para generación y validación de tokens JWT
    /// </summary>
    public class JwtHelper
    {
       
         private readonly IConfiguration _configuration;

            public JwtHelper(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            /// <summary>
            /// Genera un token JWT con los datos del cliente
            /// El token contiene: Id, Email y Nombre del cliente
            /// </summary>
            /// <param name="cliente">Cliente autenticado</param>
            /// <returns>Token JWT como string</returns>
            public string GenerarToken(Cliente cliente)
            {
                // Lee configuración desde appsettings.json — nunca hardcodeado
                var secretKey = _configuration["JwtSettings:SecretKey"];
                var issuer = _configuration["JwtSettings:Issuer"];
                var audience = _configuration["JwtSettings:Audience"];
                var expirationHours = int.Parse(_configuration["JwtSettings:ExpirationHours"]);

                // Clave de firma — convierte el string a bytes
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Claims — datos que viajan dentro del token
                // Cualquiera puede leerlos pero no modificarlos sin la clave
                var claims = new[]
                {
                new Claim("id",    cliente.Id.ToString()),
                new Claim("email", cliente.Email),
                new Claim("nombre", cliente.Nombre),
                // Rol del usuario — útil para autorización por roles
                new Claim(ClaimTypes.Role, "Cliente")
            };

                // Construye el token con todos los parámetros
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(expirationHours),
                    signingCredentials: creds
                );

                // Serializa el token a string (formato: xxxxx.yyyyy.zzzzz)
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
}
