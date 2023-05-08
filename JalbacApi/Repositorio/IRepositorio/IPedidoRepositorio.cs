using JalbacApi.Models;

namespace JalbacApi.Repositorio.IRepositorio
{
    public interface IPedidoRepositorio: IRepositorio<Pedido>
    {
        Task<Pedido> CrearPedido(Pedido pedido);
    }
}
