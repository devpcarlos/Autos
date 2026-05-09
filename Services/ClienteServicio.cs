using AutoMapper;
using WebApplication1.Dtos.DtoCliente;
using WebApplication1.Middelware.Exceptions;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Helpers;

namespace WebApplication1.Services
{
    public class ClienteService
    {
        private readonly IClienteRepositorio _repo;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepositorio repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los clientes con sus autos
        /// </summary>
        public List<ClienteResponseDto> ObtenerTodos()
        {
            var clientes = _repo.ObtenerTodos();
            return _mapper.Map<List<ClienteResponseDto>>(clientes);
        }

        /// <summary>
        /// Obtiene un cliente por Id con sus autos
        /// </summary>
        public ClienteResponseDto ObtenerPorId(int id)
        {
            var cliente = _repo.ObtenerPorId(id);
            if (cliente == null)
                throw new NotFoundException($"No se encontró el cliente con Id {id}");

            return _mapper.Map<ClienteResponseDto>(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// La contraseña se hashea antes de guardar
        /// </summary>
        public ClienteResponseDto Crear(ClienteRequestDto dto)
        {
            var cliente = _mapper.Map<Cliente>(dto);

            // Hashear la contraseña — nunca guardar en texto plano
            // BCrypt genera un hash seguro con salt automático
            cliente.Contrasena = PasswordHelper.Encriptar(dto.Contrasena);
            cliente.Activo = true;

            _repo.Insertar(cliente);
            return _mapper.Map<ClienteResponseDto>(cliente);
        }
    }
}
