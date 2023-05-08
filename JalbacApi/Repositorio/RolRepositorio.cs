using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class RolRepositorio : Repositorio<Rol>, IRolRepositorio
    {
        public RolRepositorio(BdJalbacContext db) : base(db)
        {
        }
    }
}
