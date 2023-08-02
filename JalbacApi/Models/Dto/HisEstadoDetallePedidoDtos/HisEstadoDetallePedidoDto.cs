using JalbacApi.Models.Dto.DetallePedidoDtos;
using JalbacApi.Models.Dto.EstadoDtos;

namespace JalbacApi.Models.Dto.HisEstadoDetallePedidoDtos
{
    public class HisEstadoDetallePedidoDto
    {
        public int IdHisEstadoDetallePedido { get; set; }

        public int IdEstado { get; set; }
        public virtual EstadoDto IdEstadoNavigation { get; set; } = null!;

        public int IdDetallePedido { get; set; }
        public virtual DetallePedidoDto IdDetallePedidoNavigation { get; set; } = null!;

        public DateTime Fecha { get; set; }

        public string MotivoDevolucion { get; set; }
    }
}
