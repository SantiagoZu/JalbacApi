using JalbacApi.Models.Dto.EstadoDtos;
using JalbacApi.Models.Dto.PedidoDtos;

namespace JalbacApi.Models.Dto.HisEstadoPedidoDtos
{
    public class HisEstadoPedidoDto
    {
        public int IdHisEstadoPedido { get; set; }

        public int IdEstado { get; set; }
        public virtual EstadoDto IdEstadoNavigation { get; set; } = null!;

        public int IdPedido { get; set; }
        public virtual PedidoDto IdPedidoNavigation { get; set; } = null!;

        public DateTime Fecha { get; set; }
    }
}
