using System.ComponentModel.DataAnnotations;

namespace JalbacApi.Models.Dto.ClienteDtos
{
    public class ClienteUpdateDto
    {
        [Required]
        public int IdCliente { get; set; }

        [Required]
        [MaxLength(50)]
        public string Documento { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; }

        [Required]
        [MaxLength(50)]
        public string Telefono { get; set; }

        [Required]
        public bool? Estado { get; set; }
    }
}
