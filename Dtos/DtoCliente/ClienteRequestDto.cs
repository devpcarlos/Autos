using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.DtoCliente
{
    public class ClienteRequestDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email no válido")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Contrasena { get; set; }

        [MaxLength(15, ErrorMessage = "Máximo 15 caracteres")]
        public string Telefono { get; set; }
    }
}
