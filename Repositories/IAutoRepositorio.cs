using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    // Interfaz — define el contrato de operaciones disponibles
    // Programar contra interfaces facilita cambios futuros
    public interface IAutoRepositorio
    {
        // Método nuevo con filtros y paginación
        (List<Auto> autos, int totalRegistros) ObtenerTodos(
         AutoQueryDto query, int clienteId);
        Auto? ObtenerPorId(int id);
        void Insertar(Auto auto);
        void Actualizar(Auto auto);
        void Eliminar(int id);

    }
}
