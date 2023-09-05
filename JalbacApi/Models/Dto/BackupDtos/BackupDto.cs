using JalbacApi.Models.Dto.EmpleadoDtos;

namespace JalbacApi.Models.Dto.BackupDtos
{
    public class BackupDto
    {
        public int IdBackup { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaBackup { get; set; }
        public virtual EmpleadoDto IdEmpleadoNavigation { get; set; }
    }
}
