using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Dtos.DtoCliente;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entidad → DTO de salida (para mostrar al cliente)
            CreateMap<Auto, AutoResponseDto>();

            // DTO de entrada → Entidad (para guardar en BD)
            // Ignoramos campos que asigna el servidor
            CreateMap<AutoRequestDto, Auto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore());

            CreateMap<Cliente, ClienteResponseDto>();
            CreateMap<ClienteRequestDto, Cliente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Autos, opt => opt.Ignore());

        }
    }
}
