using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.DetallePedidoDtos;
using JalbacApi.Models.Dto.EstadoDtos;

namespace JalbacApi.Models.Dto.PedidoDtos
{
    public class PedidoDto
    {
        public int IdPedido { get; set; }

        public int IdCliente { get; set; }

        public ClienteDto IdClienteNavigation { get; set; }

        public int IdEstado { get; set; }
        public EstadoDto IdEstadoNavigation { get; set; }
        public ICollection<DetallePedidoDto> DetallePedidos { get; set; } = new List<DetallePedidoDto>();
        public DateTime FechaPedido { get; set; }

        public DateTime FechaEntrega { get; set; }
    }
}