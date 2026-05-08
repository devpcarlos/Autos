namespace WebApplication1.Dtos.DtoCliente
{
    public class ClienteResponseDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        // Lista de autos del cliente — para el GET con relación
        public List<AutoResponseDto> Autos { get; set; }
    }
}
