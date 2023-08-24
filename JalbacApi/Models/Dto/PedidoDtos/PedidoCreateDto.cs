using JalbacApi.Models.Dto.DetallePedidoDtos;

namespace JalbacApi.Models.Dto.PedidoDtos
{
    public class PedidoCreateDto
    {
        public string DocumentoCliente { get; set; }

        public int IdEstado { get; set; }

        public string FechaEntrega { get; set; }
        public List<DetalleCreateDto> DetallesPedido { get; set; }

    }
}