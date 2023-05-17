using System.ComponentModel.DataAnnotations;

namespace JalbacApi.Models.Dto.PedidoDtos
{
    public class PedidoUpdateDto
    {
        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdEstado { get; set; }

        public DateTime FechaPedido { get; set; }

        [Required]
        public DateTime FechaEntrega { get; set; }
    }
}