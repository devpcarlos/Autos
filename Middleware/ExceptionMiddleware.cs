using System.Net;
using System.Text.Json;
using WebApplication1.Middleware.Exceptions;

namespace WebApplication1.Middleware
{
    // Middleware = intercepta TODAS las peticiones HTTP
    // Si cualquier parte del código lanza una excepción,
    // este middleware la captura antes de llegar al cliente
    public class ExceptionMiddleware
    {
        // Delegate que representa el siguiente middleware en el pipeline
        private readonly RequestDelegate _next;

        // Logger para registrar errores en la consola
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
                                   ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Este método se ejecuta en CADA petición HTTP
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pasa la petición al siguiente componente (controlador, etc.)
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                // Recurso no encontrado → HTTP 404
                _logger.LogWarning(ex.Message);
                await EscribirRespuesta(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                // Datos inválidos → HTTP 400
                _logger.LogWarning(ex.Message);
                await EscribirRespuesta(context, HttpStatusCode.BadRequest, ex.Message);
            }catch (UnAuthorizedException ex)
            {
                // No autenticado → HTTP 401
                _logger.LogWarning(ex.Message);
                await EscribirRespuesta(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                // Cualquier otro error → HTTP 500
                _logger.LogError(ex, "Error inesperado");
                await EscribirRespuesta(context, HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno en el servidor");
            }
        }

        // Construye la respuesta JSON estándar de error
        private async Task EscribirRespuesta(HttpContext context,
                                              HttpStatusCode statusCode,
                                              string mensaje)
        {
            // Indica al cliente que la respuesta es JSON
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Mensaje = mensaje
            };

            // Serializa el objeto a JSON y lo escribe en la respuesta
            var json = JsonSerializer.Serialize(respuesta,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(json);
        }
    }
}
