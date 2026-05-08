namespace WebApplication1.Middelware.Exceptions
{
    // Estructura estándar que retorna la API cuando hay un error
    // Todos los errores tendrán este mismo formato JSON
    public class ErrorResponse
    {
        // Código HTTP del error (404, 400, 500)
        public int StatusCode { get; set; }

        // Mensaje descriptivo del error
        public string Mensaje { get; set; }

        // Fecha y hora exacta del error
        public DateTime FechaHora { get; set; } = DateTime.Now;
    
}
}
