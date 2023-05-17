using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class DetallePedidoRepositorio : Repositorio<DetallePedido>, IDetallePedidoRepositorio
    {
        public DetallePedidoRepositorio(BdJalbacContext db) : base(db)
        {
        }
    }
}
