using System;
using System.Collections.Generic;

namespace JalbacApi.Models;

public partial class HisEstadoPedido
{
    public int IdHisEstadoPedido { get; set; }

    public int IdEstado { get; set; }
    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public int IdPedido { get; set; }
    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public DateTime Fecha { get; set; }


}
