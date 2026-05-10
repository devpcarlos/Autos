using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication1.Helpers
{
    /// <summary>
    /// Centraliza los eventos JWT para mantener Program.cs limpio
    /// Maneja las respuestas 401 y 403 en formato JSON estándar
    /// </summary>
    public class JwtEventsHelper
    {
        /// <summary>
        /// Retorna los eventos configurados para JWT
        /// Se registra en Program.cs con una sola línea
        /// </summary>
        public static JwtBearerEvents ObtenerEventos() => new JwtBearerEvents
        {
            // Intercepta cuando no hay token o es inválido → HTTP 401
            OnChallenge = context =>
            {
                context.HandleResponse(); // ← evita que ASP.NET escriba su respuesta vacía
                return ManejarRespuesta(context.HttpContext.Response, 401,
                    "No autorizado — token requerido");
            },

            // Intercepta cuando hay token válido pero sin permisos → HTTP 403
            OnForbidden = context =>
                ManejarRespuesta(context.HttpContext.Response, 403,
                    "No tienes permisos para realizar esta acción")
        };

        /// <summary>
        /// Construye la respuesta JSON estándar de error
        /// Mismo formato que ErrorResponse del middleware
        /// </summary>
        private static async Task ManejarRespuesta(
            HttpResponse response, int statusCode, string mensaje)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";

            await response.WriteAsJsonAsync(new
            {
                statusCode,
                mensaje,
                fechaHora = DateTime.UtcNow
            });
        }
    }
}
