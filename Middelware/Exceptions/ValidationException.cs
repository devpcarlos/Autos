namespace WebApplication1.Middelware.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string mensaje) : base(mensaje) { }

    }
}
