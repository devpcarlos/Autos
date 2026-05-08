using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class AutoRepositorio : IAutoRepositorio
    {
        // DbContext inyectado para acceder a la BD
        private readonly AppDbContext _context;

        public AutoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        // Retorna solo los autos activos
        public List<Auto> ObtenerTodos()
        {
            return _context.Autos
                           .Where(a => a.Activo)
                           .ToList();
        }

        // Busca por clave primaria
        public Auto ObtenerPorId(int id)
        {
            return _context.Autos.Find(id);
        }

        // Inserta y ejecuta el INSERT en SQL Server
        public void Insertar(Auto auto)
        {
            _context.Autos.Add(auto);
            _context.SaveChanges();
        }

        // Actualiza y ejecuta el UPDATE en SQL Server
        public void Actualizar(Auto auto)
        {
            _context.Autos.Update(auto);
            _context.SaveChanges();
        }

        // Eliminación lógica — no borra el registro, solo lo desactiva
        public void Eliminar(int id)
        {
            var auto = _context.Autos.Find(id);
            if (auto != null)
            {
                auto.Activo = false;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene autos con filtros opcionales y paginación
        /// </summary>
        public (List<Auto> autos, int totalRegistros) ObtenerTodos(AutoQueryDto query)
        {
            // IQueryable permite construir la consulta paso a paso
            // sin ejecutarla hasta el final — más eficiente
            var consulta = _context.Autos
                                   .Where(a => a.Activo)
                                   .AsQueryable();

            // ── Aplicar filtros solo si el cliente los envió ──

            // Filtro por marca — Contains es LIKE '%Toyota%' en SQL
            if (!string.IsNullOrEmpty(query.Marca))
                consulta = consulta.Where(a =>
                    a.Marca.Contains(query.Marca));

            // Filtro por modelo
            if (!string.IsNullOrEmpty(query.Modelo))
                consulta = consulta.Where(a =>
                    a.Modelo.Contains(query.Modelo));

            // Filtro por año exacto
            if (query.Año.HasValue)
                consulta = consulta.Where(a =>
                    a.Año == query.Año.Value);

            // Filtro por rango de precio mínimo
            if (query.PrecioMin.HasValue)
                consulta = consulta.Where(a =>
                    a.Precio >= query.PrecioMin.Value);

            // Filtro por rango de precio máximo
            if (query.PrecioMax.HasValue)
                consulta = consulta.Where(a =>
                    a.Precio <= query.PrecioMax.Value);

            // ── Paginación ──

            // Cuenta el total ANTES de paginar — para calcular TotalPaginas
            var totalRegistros = consulta.Count();

            // Skip salta los registros de páginas anteriores
            // Take toma solo los registros de la página actual
            // Ejemplo página 2, cantidad 10: Skip(10).Take(10)
            var autos = consulta
                .OrderBy(a => a.Id)
                .Skip((query.Pagina - 1) * query.Cantidad)
                .Take(query.Cantidad)
                .ToList();

            return (autos, totalRegistros);
        }
    }
}
