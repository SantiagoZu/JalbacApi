using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.PermisoDtos;
using JalbacApi.Repositorio.IRepositorio;
using System.Linq.Expressions;

namespace JalbacApi.Repositorio
{
    public class PermisoRepositorio : Repositorio<Permiso>,IPermisoRepositorio
    {
        private readonly BdJalbacContext _db;

        public PermisoRepositorio( BdJalbacContext db) : base(db)
        {
            _db = db;
        }

    }
}
