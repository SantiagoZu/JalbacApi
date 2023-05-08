using JalbacApi.Models.Dto.RolDtos;

namespace JalbacApi.Models.Dto.UsuarioDtos
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }

        public int IdRol { get; set; }
        public RolDto IdRolNavigation { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public bool? Estado { get; set; }
    }
}
