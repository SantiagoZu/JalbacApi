using JalbacApi.Models.Dto.UsuarioDtos;

namespace JalbacApi.Models.Dto.EmpleadoDtos
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }

        public int IdUsuario { get; set; }

        public virtual UsuarioDto IdUsuarioNavigation { get; set; } = null!;

        public bool Estado { get; set; }

        public string Documento { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string Cargo { get; set; } = null!;

    }
}
