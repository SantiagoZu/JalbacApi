using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class HisEstadoDetallePedidoRepositorio : Repositorio<HisEstadoDetallePedido>, IHisEstadoDetallePedidoRepositorio
    {
        private readonly BdJalbacContext _db;

        public HisEstadoDetallePedidoRepositorio(BdJalbacContext db) : base(db)
        {
            _db = db;
        }
        public async Task<HisEstadoDetallePedido> CrearHisDetallePedido(HisEstadoDetallePedido hisDetallePedido)
        {
            hisDetallePedido.Fecha = DateTime.Now;
            await _db.AddAsync(hisDetallePedido);
            await _db.SaveChangesAsync();

            return hisDetallePedido;
        }

    }
}
