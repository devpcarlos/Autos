namespace WebApplication1.Services
{
    public class UsuarioContextoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioContextoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Extrae el Id del cliente autenticado desde el token JWT
        /// </summary>
        public int ObtenerClienteId()
        {
            var claim = _httpContextAccessor.HttpContext?
                            .User.FindFirst("id")?.Value;
            return int.Parse(claim ?? "0");
        }
    }
}
