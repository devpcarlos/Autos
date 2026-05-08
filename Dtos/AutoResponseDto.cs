namespace WebApplication1.Dtos
{
    public class AutoResponseDto
    {
        // Lo que el cliente VE cuando consulta un auto
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public decimal Precio { get; set; }

        // Campo calculado, no existe en la BD
        public string NombreCompleto => $"{Marca} {Modelo} {Año}";
    }
}
