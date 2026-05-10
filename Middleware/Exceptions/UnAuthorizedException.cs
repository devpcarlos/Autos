namespace WebApplication1.Middleware.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string mensaje) : base(mensaje) { }
    }
}
