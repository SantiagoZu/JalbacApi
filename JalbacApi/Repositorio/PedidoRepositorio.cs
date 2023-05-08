using JalbacApi.Models;
using JalbacApi.Models.Dto.PedidoDto;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JalbacApi.Repositorio
{
    public class PedidoRepositorio : Repositorio<Pedido>, IPedidoRepositorio
    {
        private readonly BdJalbacContext _db;


        public PedidoRepositorio(BdJalbacContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Pedido> CrearPedido(Pedido pedido)
        {
            pedido.FechaPedido = DateTime.Now;
            await _db.AddAsync(pedido);
            await _db.SaveChangesAsync();

            return pedido;
        }

    }
}
