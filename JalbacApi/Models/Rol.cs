using System;
using System.Collections.Generic;

namespace JalbacApi.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
