using System;
using System.Collections.Generic;

namespace JalbacApi.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<HisEstadoDetallePedido> HisEstadoDetallePedidos { get; set; } = new List<HisEstadoDetallePedido>();

    public virtual ICollection<HisEstadoPedido> HisEstadoPedidos { get; set; } = new List<HisEstadoPedido>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
