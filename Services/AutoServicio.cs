using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Middelware.Exceptions;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class AutoServicio
    {

        // Repositorio para operaciones de base de datos
        private readonly IAutoRepositorio _repo;

        // IMapper convierte entre entidades y DTOs automáticamente
        private readonly IMapper _mapper;

        public AutoServicio(IAutoRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene autos con filtros y paginación
        /// </summary>
        public PaginadoResponseDto<AutoResponseDto> ObtenerTodos(AutoQueryDto query)
        {
            // 1. Consulta al repositorio con filtros
            var (autos, totalRegistros) = _repo.ObtenerTodos(query);

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
        /// Obtiene todos los autos activos y los convierte a DTOs
        /// </summary>
        public List<AutoResponseDto> ObtenerTodos()
        {
            // 1. Trae todas las entidades activas de la BD
            var autos = _repo.ObtenerTodos();

            // 2. Convierte List<Auto> → List<AutoResponseDto>
            return _mapper.Map<List<AutoResponseDto>>(autos);
        }

        /// <summary>
        /// Busca un auto por Id y lo convierte a DTO
        /// </summary>
        public AutoResponseDto ObtenerPorId(int id)
        {
            // 1. Busca la entidad en la BD por clave primaria
            var auto = _repo.ObtenerPorId(id);

            // 2. Si no existe lanza NotFoundException
            //    el middleware la captura y retorna HTTP 404
            if (auto == null)
                throw new NotFoundException($"No se encontró el auto con Id {id}");

            // 3. Convierte Auto → AutoResponseDto
            return _mapper.Map<AutoResponseDto>(auto);
        }

        /// <summary>
        /// Crea un nuevo auto a partir del DTO enviado por el cliente
        /// </summary>
        public AutoResponseDto Crear(AutoRequestDto dto)
        {
            // 1. Convierte AutoRequestDto → Auto (entidad)
            //    AutoMapper ignora Id, FechaCreacion y Activo
            var auto = _mapper.Map<Auto>(dto);

            // 2. Asigna campos que el cliente no debe enviar
            auto.FechaCreacion = DateTime.Now; // fecha actual del servidor
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
        public AutoResponseDto Actualizar(int id, AutoRequestDto dto)
        {
            // 1. Verifica que el auto existe en la BD
            var auto = _repo.ObtenerPorId(id);

            // 2. Si no existe lanza NotFoundException
            //    el middleware la captura y retorna HTTP 404
            if (auto == null)
                throw new NotFoundException($"No se encontró el auto con Id {dto.Marca}");

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
        public void Eliminar(int id)
        {
            // 1. Verifica que el auto existe en la BD
            var auto = _repo.ObtenerPorId(id);

            // 2. Si no existe lanza NotFoundException
            //    el middleware la captura y retorna HTTP 404
            if (auto == null)
                throw new NotFoundException($"No se encontró el auto con Id {id}");

            // 3. El repositorio hace eliminación lógica (Activo = false)
            //    El registro sigue en la BD pero no aparece en consultas
            _repo.Eliminar(id);
        }
    }
}
