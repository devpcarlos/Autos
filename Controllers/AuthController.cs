using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos.DtoAuth;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // → api/auth
    public class AuthController : ControllerBase
    {
        private readonly AuthService _servicio;

        public AuthController(AuthService servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// POST api/auth/login
        /// Autentica un cliente y retorna un token JWT
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = _servicio.Login(dto);
            return Ok(resultado);
        }
    }
}
