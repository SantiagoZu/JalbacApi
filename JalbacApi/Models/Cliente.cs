using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JalbacApi.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Documento { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
