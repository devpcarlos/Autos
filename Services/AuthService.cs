using Microsoft.Extensions.Options;
using WebApplication1.Dtos.DtoAuth;
using WebApplication1.Helpers;
using WebApplication1.Middleware.Exceptions;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class AuthService
    {
        private readonly IClienteRepositorio _repo;
        private readonly JwtSettings _settings;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IClienteRepositorio repo, 
            JwtHelper jwtHelper,
            IOptions<JwtSettings> options)
        {
            _repo = repo;
            _jwtHelper = jwtHelper;
            _settings = options.Value; // El objeto JwtSettings ya bindeado y validado
        }

        /// <summary>
        /// Autentica un cliente y genera un token JWT
        /// </summary>
        /// <param name="dto">Email y contraseña del cliente</param>
        /// <returns>Token JWT y datos básicos del cliente</returns>
        public LoginResponseDto Login(LoginRequestDto dto)
        {
            // 1. Busca el cliente por email
            var cliente = _repo.ObtenerPorEmail(dto.Email);

            // 2. Si no existe lanza excepción
            if (cliente == null)
                throw new NotFoundException("Email o contraseña incorrectos");

            // 3. Compara la contraseña con el hash guardado
            //    Usa el helper reutilizable
            var contrasenaValida = PasswordHelper.Comparar(
                dto.Contrasena,
                cliente.Contrasena
            );

            // 4. Si la contraseña no coincide lanza excepción
            //    Mismo mensaje que el anterior — no revelar si el email existe
            if (!contrasenaValida)
                throw new NotFoundException("Email o contraseña incorrectos");

            // 5. Genera el token JWT con los datos del cliente
            var token = _jwtHelper.GenerarToken(cliente);

            return new LoginResponseDto
            {
                Token = token,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
                Expiracion = DateTime.UtcNow.AddHours(_settings.ExpirationHours)
            };
        }
    }
}
