using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class EstadoRepositorio : Repositorio<Estado>, IEstadoRepositorio
    {
        public EstadoRepositorio(BdJalbacContext db) : base(db)
        {
        }
    }
}
