using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.DetallePedidoDtos;
using JalbacApi.Models.Dto.EmpleadoDtos;
using JalbacApi.Models.Dto.EstadoDtos;
using JalbacApi.Models.Dto.PedidoDtos;
using JalbacApi.Models.Dto.RolDtos;
using JalbacApi.Models.Dto.UsuarioDtos;

namespace JalbacApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            #region ClientesDtos
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Cliente, ClienteCreateDto>().ReverseMap();
            CreateMap<Cliente, ClienteUpdateDto>().ReverseMap();
            #endregion
            
            #region EstadosDtos
            CreateMap<Estado, EstadoDto>().ReverseMap();
            #endregion

            #region PedidosDtos
            CreateMap<Pedido, PedidoDto>().ReverseMap();
            CreateMap<Pedido, PedidoCreateDto>().ReverseMap();
            CreateMap<Pedido, PedidoUpdateDto>().ReverseMap();
            #endregion

            #region RolesDto
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Rol, RolCreateDto>().ReverseMap();
            CreateMap<Rol, RolUpdateDto>().ReverseMap();
            #endregion

            #region UsuariosDto
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioCreateDto>().ReverseMap();
            CreateMap<Usuario, UsuarioUpdateDto>().ReverseMap();
            #endregion

            #region DetallesPedidoDto
            CreateMap<DetallePedido, DetallePedidoDto>().ReverseMap();
            CreateMap<DetallePedido, DetalleCreateDto>().ReverseMap();
            CreateMap<DetallePedido, DetalleUpdateDto>().ReverseMap();
            #endregion

            #region EmpleadosDto
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoCreateDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoUpdateDto>().ReverseMap();
            #endregion

        }
    }
}
