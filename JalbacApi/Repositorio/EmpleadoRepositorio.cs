using JalbacApi.Models;
using JalbacApi.Models.Dto.EmpleadoDtos;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class EmpleadoRepositorio : Repositorio<Empleado>, IEmpleadoRepositorio
    {
        private readonly BdJalbacContext _db;

        public EmpleadoRepositorio(BdJalbacContext db) : base(db)
        {
            _db = db;
        }

        
    }
}
