using JalbacApi.Models.Dto.RolDtos;

namespace JalbacApi.Models.Dto.UsuarioDtos
{
    public class UsuarioUpdateDto
    {
        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        public string Correo { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public bool? Estado { get; set; }
    }
}
