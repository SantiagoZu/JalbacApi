using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class RolPermisoRepositorio : Repositorio<RolPermiso>, IRolPermisoRepositorio
    {
        public RolPermisoRepositorio(BdJalbacContext db) : base(db)
        {
        }
    }
}