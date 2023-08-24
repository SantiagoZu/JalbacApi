using JalbacApi.Models.Dto.PedidoDtos;

namespace JalbacApi.Models.Dto.HisEstadoPedidoDtos
{
    public class HistorialesDePedidoDto
    {
        public int IdPedido { get; set; }
        public virtual PedidoDto IdPedidoNavigation { get; set; } = null!;
        public IEnumerable<HisEstadoPedido> HistorialesDePedido { get; set; }
    }
}
