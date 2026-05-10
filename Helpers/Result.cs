namespace WebApplication1.Helpers
{
    public class Result<T> 
    {
        public bool Exito { get; private set; }
        public T? Valor { get; private set; }
        public string? Error { get; private set; }

        private Result() { }

        public static Result<T> Ok(T valor) => new()
        {
            Exito = true,
            Valor = valor
        };

        public static Result<T> Fallo(string error) => new()
        {
            Exito = false,
            Error = error
        };
    }
}
