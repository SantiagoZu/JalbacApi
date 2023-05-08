using JalbacApi.Models;
using JalbacApi.Models.Dto.UsuarioDtos;

namespace JalbacApi.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {
        Task<Usuario> CrearUsuario(Usuario usuario);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
