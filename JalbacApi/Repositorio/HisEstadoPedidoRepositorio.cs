using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class HisEstadoPedidoRepositorio : Repositorio<HisEstadoPedido>, IHisEstadoPedidoRepositorio
    {
        private readonly BdJalbacContext _db;

        public HisEstadoPedidoRepositorio(BdJalbacContext db) : base(db)
        {
            _db = db;
        }
        public async Task<HisEstadoPedido> CrearHisPedido(HisEstadoPedido hisPedido)
        {
            hisPedido.Fecha = DateTime.Now;
            await _db.AddAsync(hisPedido);
            await _db.SaveChangesAsync();

            return hisPedido;
        }
    }
}
