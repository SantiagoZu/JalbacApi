using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class EmpleadoRepositorio : Repositorio<Empleado>, IEmpleadoRepositorio
    {
        public EmpleadoRepositorio(BdJalbacContext db) : base(db)
        {
        }
    }
}
