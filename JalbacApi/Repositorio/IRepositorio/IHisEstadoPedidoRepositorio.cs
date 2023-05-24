using JalbacApi.Models;

namespace JalbacApi.Repositorio.IRepositorio
{
    public interface IHisEstadoPedidoRepositorio : IRepositorio<HisEstadoPedido>
    {
        Task<HisEstadoPedido> CrearHisPedido(HisEstadoPedido hisPedido);

    }
}
