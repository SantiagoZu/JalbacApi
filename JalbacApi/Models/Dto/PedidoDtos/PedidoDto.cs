using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.EstadoDtos;

namespace JalbacApi.Models.Dto.PedidoDtos
{
    public class PedidoDto
    {
        public int IdPedido { get; set; }

        public int IdCliente { get; set; }

        public ClienteDto IdClienteNavigation { get; set; }

        public int IdEstado { get; set; }
        public EstadoDto IdEstadoNavigation { get; set; }

        public DateTime FechaPedido { get; set; }

        public DateTime FechaEntrega { get; set; }
        public bool IsActivo { get; set; }
        public string MotivoInactivacion { get; set; } = null;
    }
}