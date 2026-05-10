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
        private readonly JwtSecurityTokenHandler _handler;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _handler = new JwtSecurityTokenHandler();
        }

        // ─── Privado: centraliza lectura de config ────────────────────────
        private TokenValidationParameters ObtenerParametrosValidacion()
        {
            var secretKey = _configuration["JwtSettings:SecretKey"] ?? "";
            var issuer = _configuration["JwtSettings:Issuer"] ?? "";
            var audience = _configuration["JwtSettings:Audience"] ?? "";

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                                               Encoding.UTF8.GetBytes(secretKey))
            };
        }

        // ─── 1. GENERAR ───────────────────────────────────────────────────
        /// <summary>
        /// Construye y firma el token JWT con los claims del cliente.
        /// No valida ni decodifica — solo genera.
        /// </summary>
        public string GenerarToken(Cliente cliente)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"] ?? "";
            var issuer = _configuration["JwtSettings:Issuer"] ?? "";
            var audience = _configuration["JwtSettings:Audience"] ?? "";
            var expirationHours = int.Parse(_configuration["JwtSettings:ExpirationHours"] ??  "8");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",            cliente.Id.ToString()),
                new Claim("email",         cliente.Email),
                new Claim("nombre",        cliente.Nombre),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(expirationHours),
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
