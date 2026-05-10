using Microsoft.Extensions.Options;
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
        private readonly JwtSettings _settings;
        private readonly JwtSecurityTokenHandler _handler;

        public JwtHelper(IOptions<JwtSettings>options)
        {
            _handler = new JwtSecurityTokenHandler();
            // options.Value contiene el objeto JwtSettings ya bindeado
            // y validado por ValidateOnStart() en Program.cs
            _settings = options.Value;
        }

        // ─── Privado: centraliza lectura de config ────────────────────────
        private TokenValidationParameters ObtenerParametrosValidacion()
        {

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                                               Encoding.UTF8.GetBytes(_settings.SecretKey))
            };
        }

        // ─── 1. GENERAR ───────────────────────────────────────────────────
        /// <summary>
        /// Construye y firma el token JWT con los claims del cliente.
        /// No valida ni decodifica — solo genera.
        /// </summary>
        public string GenerarToken(Cliente cliente)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",            cliente.Id.ToString()),
                new Claim("email",         cliente.Email),
                new Claim("nombre",        cliente.Nombre),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours), //DateTime.Now.AddHours(expirationHours),
                signingCredentials: creds
            );

            return _handler.WriteToken(token);
        }

        // ─── 2. VALIDAR ───────────────────────────────────────────────────
        /// <summary>
        /// Verifica firma, issuer, audience y expiración.
        /// Retorna Result con los claims si es válido, o el motivo del fallo.
        /// </summary>
        public Result<ClaimsPrincipal> ValidarToken(string token)
        {
            try
            {
                var principal = _handler.ValidateToken(
                    token,
                    ObtenerParametrosValidacion(),
                    out SecurityToken _
                );

                return Result<ClaimsPrincipal>.Ok(principal);
            }
            catch (SecurityTokenExpiredException)
            {
                return Result<ClaimsPrincipal>.Fallo("El token ha expirado");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return Result<ClaimsPrincipal>.Fallo("La firma del token es inválida");
            }
            catch (SecurityTokenException ex)
            {
                return Result<ClaimsPrincipal>.Fallo($"Token inválido: {ex.Message}");
            }
        }

        // ─── 3. DECODIFICAR ───────────────────────────────────────────────
        /// <summary>
        /// Extrae los claims SIN validar firma ni expiración.
        /// Útil para refresh tokens: leer el id del cliente aunque el token expiró.
        /// </summary>
        public Result<ClaimsPrincipal> ObtenerClaims(string token)
        {
            try
            {
                var parametros = ObtenerParametrosValidacion();
                parametros.ValidateLifetime = false;
                parametros.ValidateIssuerSigningKey = true;

                var principal = _handler.ValidateToken(
                    token,
                    parametros,
                    out SecurityToken _
                );

                return Result<ClaimsPrincipal>.Ok(principal);
            }
            catch (SecurityTokenException ex)
            {
                return Result<ClaimsPrincipal>.Fallo(
                    $"No se pudieron extraer los claims: {ex.Message}");
            }
        }
    }
}
