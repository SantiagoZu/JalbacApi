using System;
using System.Collections.Generic;

namespace JalbacApi.Models;

public partial class HisEstadoPedido
{
    public int IdHisEstadoPedido { get; set; }

    public int IdEstado { get; set; }

    public int IdPedido { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
