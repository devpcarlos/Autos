using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IClienteRepositorio
    {
        List<Cliente> ObtenerTodos();
        Cliente ObtenerPorId(int id);
        void Insertar(Cliente cliente);
        Cliente? ObtenerPorEmail(string email);
    }
}
