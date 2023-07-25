using JalbacApi.Models.Dto.PermisoDtos;

namespace JalbacApi.Models.Dto.RolDtos
{
    public class RolCreateDto
    {
        public string Nombre { get; set; }

        public bool Estado { get; set; }
        public List<PermisoDto> Permisos { get; set; }
    }
}
