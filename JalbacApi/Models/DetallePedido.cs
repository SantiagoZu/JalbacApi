using System;
using System.Collections.Generic;

namespace JalbacApi.Models;

public partial class DetallePedido
{
    public int IdDetallePedido { get; set; }

    public int IdPedido { get; set; }

    public int IdEmpleado { get; set; }

    public int IdEstado { get; set; }

    public string NombreAnillido { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Peso { get; set; } = null!;

    public string TamanoAnillo { get; set; } = null!;

    public string TamanoPiedra { get; set; } = null!;

    public string Material { get; set; } = null!;

    public string Detalle { get; set; } = null!;

    public string MotivoDevolucion { get; set; } = null!;


    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
