using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // → la ruta será: api/autos
    public class AutosController : ControllerBase
    {
        // El servicio contiene toda la lógica de negocio
        private readonly AutoServicio _servicio;

        public AutosController(AutoServicio servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// GET api/autos — con filtros y paginación opcionales
        /// Ejemplos:
        /// GET api/autos
        /// GET api/autos?pagina=1&cantidad=10
        /// GET api/autos?marca=Toyota
        /// GET api/autos?marca=Toyota&precioMin=50000000&pagina=1&cantidad=5
        /// </summary>
        [HttpGet]
        public IActionResult GetTodos([FromQuery] AutoQueryDto query)
        {
            // [FromQuery] indica que los parámetros vienen en la URL
            // ASP.NET Core mapea automáticamente ?marca=Toyota → query.Marca
            var resultado = _servicio.ObtenerTodos(query);
            return Ok(resultado);
        }

        /// <summary>
        /// GET api/autos — Retorna todos los autos activos
        /// </summary>
       /* [HttpGet]
        public IActionResult GetTodos()
        {
            // Si falla, el middleware captura la excepción y retorna HTTP 500
            var autos = _servicio.ObtenerTodos();
            return Ok(autos); // HTTP 200 con lista en JSON
        }*/

        /// <summary>
        /// GET api/autos/1 — Retorna un auto por Id
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetPorId(int id)
        {
            // Si no existe, el servicio lanza NotFoundException
            // el middleware la captura y retorna HTTP 404
            var auto = _servicio.ObtenerPorId(id);
            return Ok(auto); // HTTP 200
        }

        /// <summary>
        /// POST api/autos — Crea un nuevo auto
        /// </summary>
        [HttpPost]
        public IActionResult Crear([FromBody] AutoRequestDto dto)
        {
            /* ModelState se sigue validando aquí porque es responsabilidad
            del controlador validar los datos de entrada, no del middleware*/
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // HTTP 400

            var autoCreado = _servicio.Crear(dto);

            // HTTP 201 — recurso creado exitosamente
            return CreatedAtAction(nameof(GetPorId),
                new { id = autoCreado.Id }, autoCreado);
        }

        /// <summary>
        /// PUT api/autos/1 — Actualiza un auto existente
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] AutoRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // HTTP 400

            // Si no existe, el servicio lanza NotFoundException
            // el middleware la captura y retorna HTTP 404
            var autoActualizado = _servicio.Actualizar(id, dto);
            return Ok(autoActualizado); // HTTP 200
        }

        /// <summary>
        /// DELETE api/autos/1 — Elimina lógicamente un auto
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            // Si no existe, el servicio lanza NotFoundException
            // el middleware la captura y retorna HTTP 404
            _servicio.Eliminar(id);
            return NoContent(); // HTTP 204 éxito sin contenido
        }
    }
}
