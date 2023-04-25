using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class ClienteRepositorio: Repositorio<Cliente>, IClienteRepositorio
    {
        private readonly BdJalbacContext _db;

        public ClienteRepositorio(BdJalbacContext db) : base(db)
        {
            _db = db;
        }
    }
}
