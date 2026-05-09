namespace WebApplication1.Dtos.DtoAuth
{
    /// <summary>
    /// Respuesta del servidor después de un login exitoso
    /// </summary>
    public class LoginResponseDto
    {
        // El token JWT que el cliente debe guardar y enviar en cada petición
        public string Token { get; set; }

        // Información básica del cliente autenticado
        public string Nombre { get; set; }
        public string Email { get; set; }

        // Cuándo expira el token
        public DateTime Expiracion { get; set; }
    }
}
