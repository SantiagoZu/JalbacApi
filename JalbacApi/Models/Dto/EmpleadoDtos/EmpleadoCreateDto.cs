using JalbacApi.Models.Dto.UsuarioDtos;

namespace JalbacApi.Models.Dto.EmpleadoDtos
{
    public class EmpleadoCreateDto
    {

        public bool Estado { get; set; }

        public string Documento { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string Cargo { get; set; } = null!;

        public int IdRol { get; set; }

        public string Correo  { get; set; }

        public string Contrasena { get; set; } = null!;
    }
}
