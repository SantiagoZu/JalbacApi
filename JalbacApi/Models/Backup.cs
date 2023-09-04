using System;
using System.Collections.Generic;

namespace JalbacApi.Models;
public partial class Backup
{
    public int IdBackup { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime FechaBackup { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
