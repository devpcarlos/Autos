namespace WebApplication1.Dtos
{
    // Respuesta estándar para cualquier lista paginada
    // T es genérico — sirve para Autos, Clientes, cualquier entidad
    public class PaginadoResponseDto<T>
    {
        // Los datos de la página actual
        public List<T> Data { get; set; }

        // Información de paginación para que el cliente sepa cuántas páginas hay
        public int PaginaActual { get; set; }
        public int CantidadPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }

        // El cliente sabe si puede pedir página anterior o siguiente
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
