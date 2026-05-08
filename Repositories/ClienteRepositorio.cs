using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ClienteRepositorio : IClienteRepositorio
    {

        private readonly AppDbContext _context;

        public ClienteRepositorio(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los clientes activos incluyendo sus autos
        /// Include hace el JOIN con la tabla Autos automáticamente
        /// </summary>
        public List<Cliente> ObtenerTodos()
        {
            return _context.Clientes
                           .Where(c => c.Activo)
                           // Include carga los Autos relacionados
                           // sin esto los Autos vendrían null
                           .Include(c => c.Autos)
                           .ToList();
        }

        /// <summary>
        /// Obtiene un cliente por Id incluyendo sus autos
        /// </summary>
        public Cliente ObtenerPorId(int id)
        {
            return _context.Clientes
                           .Include(c => c.Autos)
                           .FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Inserta un nuevo cliente en la BD
        /// </summary>
        public void Insertar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }
    }
}
