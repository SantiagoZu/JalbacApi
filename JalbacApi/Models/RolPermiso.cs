using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JalbacApi.Models;

public partial class RolPermiso
{
    public int IdRolPermiso { get; set; }

    public int IdRol { get; set; }

    public int IdPermiso { get; set; }

    public virtual Permiso IdPermisoNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Rol IdRolNavigation { get; set; } = null!;
}
