namespace WebApplication1.Models
{
    public class Cliente
    {
        // PRIMARY KEY
        public int Id { get; set; }

        // Datos personales
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }

        // Propiedad de navegación — un Cliente tiene muchos Autos
        // EF Core usa esto para saber que existe la relación
        public ICollection<Auto> Autos { get; set; }
    }
}
