using System.ComponentModel.DataAnnotations;

namespace JalbacApi.Models.Dto.UsuarioDtos
{
    public class LoginRequestDto
    {
        [Required]
        public string Correo { get; set; } = null!;
        [Required]
        public string Contrasena { get; set; } = null!;

    }
}
