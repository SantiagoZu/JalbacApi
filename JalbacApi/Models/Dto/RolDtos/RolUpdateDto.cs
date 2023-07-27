using JalbacApi.Models.Dto.PermisoDtos;

namespace JalbacApi.Models.Dto.RolDtos
{
    public class RolUpdateDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public List<PermisoDto> Permisos { get; set; }

    }
}
