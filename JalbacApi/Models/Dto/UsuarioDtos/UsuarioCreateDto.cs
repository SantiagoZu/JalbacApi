namespace JalbacApi.Models.Dto.UsuarioDtos
{
    public class UsuarioCreateDto
    {
        public int IdRol { get; set; }

        public string Correo { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public bool? Estado { get; set; }
    }
}
