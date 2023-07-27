using System;
using System.Collections.Generic;

namespace JalbacApi.Models;

public partial class HisEstadoDetallePedido
{
    public int IdHisEstadoDetallePedido { get; set; }

    public int IdEstado { get; set; }
    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public int IdDetallePedido { get; set; }
    public virtual DetallePedido IdDetallePedidoNavigation { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string MotivoDevolucion { get; set; }
}
