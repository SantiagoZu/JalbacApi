using JalbacApi.Models.Dto.UsuarioDtos;

namespace JalbacApi.Models.Dto.EmpleadoDtos
{
    public class EmpleadoUpdateDto
    {
        public int IdEmpleado { get; set; }

        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        public bool Estado { get; set; }

        public string Documento { get; set; } = null!;

        public string Nombre { get; set; } = null!;
        public string Correo { get; set; }

        public string Apellido { get; set; } = null!;

        public string Cargo { get; set; } = null!;

    }
}
