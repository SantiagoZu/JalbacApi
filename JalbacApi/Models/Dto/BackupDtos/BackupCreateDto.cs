using System.ComponentModel.DataAnnotations;

namespace JalbacApi.Models.Dto.BackupDtos
{
    public class BackupCreateDto
    {
        [Required]
        public int IdEmpleado { get; set; }
    }
}
