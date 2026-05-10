using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Helpers
{
    // <summary>
    /// Fuente única de configuración JWT.
    /// Se bindea desde appsettings.json una sola vez en Program.cs.
    /// Se inyecta como IOptions<JwtSettings> donde se necesite.
    /// </summary>
    public class JwtSettings
    {
        // [Required] hace que ValidateOnStart() falle si el valor
        // está vacío o ausente — la app NO arranca sin esto configurado
        [Required(ErrorMessage = "JwtSettings:SecretKey es obligatorio")]
        public string SecretKey { get; set; }

        [Required(ErrorMessage = "JwtSettings:Issuer es obligatorio")]
        public string Issuer { get; set; }

        [Required(ErrorMessage = "JwtSettings:Audience es obligatorio")]
        public string Audience { get; set; }

        // ExpirationHours sí tiene valor por defecto seguro (8 horas)
        // No lleva [Required] porque 8 es un default razonable
        public int ExpirationHours { get; set; } = 8;
    }
}
