using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Middleware.Exceptions;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class AutoService
    {

        // Repositorio para operaciones de base de datos
        private readonly IAutoRepositorio _repo;

        // IMapper convierte entre entidades y DTOs automáticamente
        private readonly IMapper _mapper;

        public AutoService(IAutoRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene autos con filtros y paginación
        /// </summary>
        public PaginadoResponseDto<AutoResponseDto> ObtenerTodos(AutoQueryDto query, int clienteId)
        {
            // 1. Consulta al repositorio con filtros
            var (autos, totalRegistros) = _repo.ObtenerTodos(query, clienteId);

            // 2. Convierte List<Auto> → List<AutoResponseDto>
            var autosDto = _mapper.Map<List<AutoResponseDto>>(autos);

            // 3. Construye la respuesta paginada
            return new PaginadoResponseDto<AutoResponseDto>
            {
                Data = autosDto,
                PaginaActual = query.Pagina,
                CantidadPorPagina = query.Cantidad,
                TotalRegistros = totalRegistros,
                // Math.Ceiling redondea hacia arriba
                // 25 registros / 10 por página = 2.5 → 3 páginas
                TotalPaginas = (int)Math.Ceiling(
                                    (double)totalRegistros / query.Cantidad)
            };
        }

        /// <summary>
        /// Busca un auto por Id y lo convierte a DTO
        /// </summary>
        public AutoResponseDto ObtenerPorId(int id, int clienteId)
        {
            // 1. Busca la entidad en la BD por clave primaria
            var auto = _repo.ObtenerPorId(id);

            // 2. Si no existe lanza NotFoundException
            //    el middleware la captura y retorna HTTP 404
            if (auto == null || auto.ClienteId != clienteId)
                throw new NotFoundException($"No se encontró el auto con Id {id}");

            return _mapper.Map<AutoResponseDto>(auto);
        }

        /// <summary>
        /// Crea un nuevo auto a partir del DTO enviado por el cliente
        /// </summary>
        public AutoResponseDto Crear(AutoRequestDto dto, int clienteId)
        {
            // 1. Convierte AutoRequestDto → Auto (entidad)
            //    AutoMapper ignora Id, FechaCreacion y Activo
            var auto = _mapper.Map<Auto>(dto);

            // ClienteId viene del token — no del body
            auto.ClienteId = clienteId;
            // 2. Asigna campos que el cliente no debe enviar
            auto.FechaCreacion = DateTime.UtcNow; // fecha actual del servidor
            auto.Activo = true;          // activo por defecto

            // 3. Guarda en la BD — EF Core ejecuta el INSERT
            _repo.Insertar(auto);

            // 4. Convierte la entidad guardada → DTO
            //    En este punto auto.Id ya tiene el Id generado por SQL Server
            return _mapper.Map<AutoResponseDto>(auto);
        }

        /// <summary>
        /// Actualiza los datos de un auto existente
        /// </summary>
        public AutoResponseDto Actualizar(int id, AutoRequestDto dto, int clienteId)
        {
            // 1. Verifica que el auto existe en la BD
            var auto = _repo.ObtenerPorId(id);

            // 2. Si no existe lanza NotFoundException
            //    el middleware la captura y retorna HTTP 404
            if (auto == null || auto.ClienteId != clienteId)
                throw new NotFoundException($"No se encontró el auto con la marca {dto.Marca}");

            // 3. Mapea el DTO sobre la entidad existente
            //    Solo actualiza Marca, Modelo, Año y Precio
            //    Conserva Id, FechaCreacion y Activo intactos
            _mapper.Map(dto, auto);

            // 4. Guarda los cambios — EF Core ejecuta el UPDATE
            _repo.Actualizar(auto);

            // 5. Retorna el auto actualizado como DTO
            return _mapper.Map<AutoResponseDto>(auto);
        }

        /// <summary>
        /// Elimina lógicamente un auto (Activo = false)
        /// </summary>
        public void Eliminar(int id, int clienteId)
        {
            // 1. Verifica que el auto existe en la BD
            var auto = _repo.ObtenerPorId(id);

            // 2. Si no existe lanza NotFoundException
            //    el middleware la captura y retorna HTTP 404
            if (auto == null || auto.ClienteId != clienteId)
                throw new NotFoundException($"No se encontró el auto con Id {id}");

            // 3. El repositorio hace eliminación lógica (Activo = false)
            //    El registro sigue en la BD pero no aparece en consultas
            _repo.Eliminar(id);
        }
    }
}
