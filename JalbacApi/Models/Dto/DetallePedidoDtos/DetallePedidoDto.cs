using JalbacApi.Models.Dto.EmpleadoDtos;
using JalbacApi.Models.Dto.EstadoDtos;
using JalbacApi.Models.Dto.PedidoDtos;

namespace JalbacApi.Models.Dto.DetallePedidoDtos
{
    public class DetallePedidoDto
    {
        public int IdDetallePedido { get; set; }

        public int IdPedido { get; set; }
        public virtual PedidoDto IdPedidoNavigation { get; set; } = null!;

        public int IdEmpleado { get; set; }
        public virtual EmpleadoDto IdEmpleadoNavigation { get; set; } = null!;

        public int IdEstado { get; set; }
        public virtual EstadoDto IdEstadoNavigation { get; set; } = null!;

        public string NombreAnillido { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public string Peso { get; set; } = null!;

        public string TamanoAnillo { get; set; } = null!;

        public string TamanoPiedra { get; set; } = null!;

        public string Material { get; set; } = null!;

        public string Detalle { get; set; } = null!;
        public int Cantidad { get; set; }


        public string MotivoDevolucion { get; set; } = null!;


    }
}
