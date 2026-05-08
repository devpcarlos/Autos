using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Auto
    {
     
            // Clave primaria, SQL Server la genera automáticamente
            public int Id { get; set; }

            // Datos del auto
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public int Año { get; set; }
            public decimal Precio { get; set; }

            // Campos internos que el cliente no debe enviar ni ver
            public DateTime FechaCreacion { get; set; }
            public bool Activo { get; set; }
        
         // LLAVE FORÁNEA — referencia al Cliente dueño del auto
        public int ClienteId { get; set; }
        // Propiedad de navegación — para acceder al Cliente desde el Auto
        public Cliente Cliente { get; set; }
        }
    }


