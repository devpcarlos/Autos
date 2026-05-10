using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos
{
    public class AutoRequestDto
    {
        // Lo que el cliente ENVÍA para crear o actualizar un auto

        [Required(ErrorMessage = "La marca es obligatoria")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Modelo { get; set; }

        [Range(1900, 2100, ErrorMessage = "Año no válido")]
        public int Año { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

    }
}
