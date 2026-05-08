namespace WebApplication1.Dtos
{
    // Contiene todos los parámetros opcionales de búsqueda y paginación
    // El cliente puede enviar uno, varios o ninguno
    public class AutoQueryDto
    {
        // ── Filtros ────────────────────────────────
        // Todos son opcionales — null significa "no filtrar por este campo"

        // Filtrar por marca: ?marca=Toyota
        public string? Marca { get; set; }

        // Filtrar por modelo: ?modelo=Corolla
        public string? Modelo { get; set; }

        // Filtrar por rango de precio: ?precioMin=50000000&precioMax=100000000
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }

        // Filtrar por año: ?año=2023
        public int? Año { get; set; }

        // ── Paginación ─────────────────────────────

        // Número de página actual — por defecto página 1
        public int Pagina { get; set; } = 1;

        // Cantidad de registros por página — por defecto 10
        public int Cantidad { get; set; } = 10;

     
    }
}
