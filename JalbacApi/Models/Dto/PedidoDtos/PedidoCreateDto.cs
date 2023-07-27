using JalbacApi.Models.Dto.DetallePedidoDtos;

namespace JalbacApi.Models.Dto.PedidoDtos
{
    public class PedidoCreateDto
    {
        public int IdCliente { get; set; }

        public int IdEstado { get; set; }

        public string FechaEntrega { get; set; }
        public List<DetalleCreateDto> DetallesPedido { get; set; }

    }
}