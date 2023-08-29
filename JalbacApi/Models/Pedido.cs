using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JalbacApi.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public int IdEstado { get; set; }

    public DateTime FechaPedido { get; set; }

    public DateTime FechaEntrega { get; set; }

    public bool IsActivo { get; set; }
    public string MotivoInactivacion { get; set; } = null;

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();


    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;
}
