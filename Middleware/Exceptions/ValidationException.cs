namespace WebApplication1.Middleware.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string mensaje) : base(mensaje) { }

    }
}
