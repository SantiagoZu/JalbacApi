using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.PermisoDtos;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JalbacApi.Repositorio
{
    public class PermisoRepositorio : Repositorio<Permiso>, IPermisoRepositorio
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IRolPermisoRepositorio _rolPermisoRepositorio;
        private readonly BdJalbacContext _db;

        public PermisoRepositorio(IMapper mapper, IUsuarioRepositorio usuarioRepositorio, IRolPermisoRepositorio rolPermisoRepositorio, BdJalbacContext db) : base(db)
        {
            _mapper = mapper;
            _usuarioRepositorio = usuarioRepositorio;
            _rolPermisoRepositorio = rolPermisoRepositorio;
            _db = db;
        }

        public async Task<List<PermisoDto>> ListaPermisos(int idUsuario)
        {
            var usuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            var rolPermiso = await _rolPermisoRepositorio.Consultar();
            var permiso = await Consultar();

            try
            {
                IQueryable<Permiso> tbResultado = (from u in usuario
                                                   join rp in rolPermiso on u.IdRol equals rp.IdRol
                                                   join p in permiso on rp.IdPermiso equals p.IdPermiso
                                                   select p).AsQueryable();

                var listaPermisos = tbResultado.ToList();
                return _mapper.Map<List<PermisoDto>>(listaPermisos);
            }
            catch
            {
                throw;
            }
        }



    }
}