using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos.DtoCliente;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // → api/clientes
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _servicio;

        public ClientesController(ClienteService servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// GET api/clientes — retorna todos los clientes con sus autos
        /// </summary>
        [HttpGet]
        public IActionResult GetTodos()
        {
            var clientes = _servicio.ObtenerTodos();
            return Ok(clientes);
        }

        /// <summary>
        /// GET api/clientes/1 — retorna un cliente con sus autos
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetPorId(int id)
        {
            var cliente = _servicio.ObtenerPorId(id);
            return Ok(cliente);
        }

        /// <summary>
        /// POST api/clientes — registra un nuevo cliente
        /// </summary>
        [HttpPost]
        public IActionResult Crear([FromBody] ClienteRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clienteCreado = _servicio.Crear(dto);
            return CreatedAtAction(nameof(GetPorId),
                new { id = clienteCreado.Nombre }, clienteCreado);
        }
    }
}
