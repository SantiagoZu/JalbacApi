using JalbacApi.Models;

namespace JalbacApi.Repositorio.IRepositorio
{
    public interface IHisEstadoDetallePedidoRepositorio : IRepositorio<HisEstadoDetallePedido>
    {
        Task<HisEstadoDetallePedido> CrearHisDetallePedido(HisEstadoDetallePedido hisDetallePedido);

    }
}
