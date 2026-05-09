using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.DtoAuth
{
    /// <summary>
    /// Datos que el cliente envía para hacer login
    /// </summary>
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; }
    }
}
