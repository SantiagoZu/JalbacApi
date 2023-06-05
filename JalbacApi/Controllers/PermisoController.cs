using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.PermisoDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController
    {
        private readonly IMapper _mapper;
        private readonly IPermisoRepositorio _permisoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IRepositorio<RolPermiso> _rolPermisoRepositorio;
        protected APIResponse _response;

        public PermisoController(IMapper mapper, IPermisoRepositorio permisoRepositorio, IUsuarioRepositorio usuarioRepositorio, IRepositorio<RolPermiso> rolPermisoRepositorio)
        {
            _mapper = mapper;
            _permisoRepositorio = permisoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _rolPermisoRepositorio = rolPermisoRepositorio;
            _response = new ();
        }

        [HttpGet("{id:int}", Name = "GetPermiso")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<ActionResult<APIResponse>> GetPermiso(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                //var usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);
                IQueryable<Usuario> usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);
                var rolPermiso = await _rolPermisoRepositorio.Obtener();
                var permiso = await _permisoRepositorio.Obtener();

                IQueryable<Permiso> tbResultado = (
                    from u in usuario
                    join rp in rolPermiso on u.IdRol equals rp.IdRol
                    join p in permiso on mp.IdPermiso equals p.IdPermiso
                    select p).AsQueryable();

                var listaPermisos = tbResultado.ToListAsync();

                if (usuario == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<PermisoDto>(listaPermisos);
                _response.IsExistoso = true;
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
