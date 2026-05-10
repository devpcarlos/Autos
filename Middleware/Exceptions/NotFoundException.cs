namespace WebApplication1.Middleware.Exceptions
{
       // Se lanza cuando un recurso no existe en la BD
        public class NotFoundException : Exception
        {
            public NotFoundException(string mensaje) : base(mensaje) { }
        }
    }

