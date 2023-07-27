using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.PermisoDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPermisoRepositorio _permisoRepositorio;
        private readonly IRolPermisoRepositorio _rolPermisoRepositorio;
        protected APIResponse _response;

        public PermisoController(IMapper mapper, IPermisoRepositorio permisoRepositorio, IRolPermisoRepositorio rolPermisoRepositorio)
        {
            _mapper = mapper;
            _permisoRepositorio = permisoRepositorio;
            _rolPermisoRepositorio = rolPermisoRepositorio;
            _response = new();
        }

        [HttpGet("{idUsuario:int}", Name = "GetPermisos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<ActionResult<APIResponse>> GetPermisos(int idUsuario)
        {
            try
            {
                if (idUsuario == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                _response.Resultado = await _permisoRepositorio.ListaPermisos(idUsuario);
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

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllPermisos()
        {
            try
            {
                IEnumerable<Permiso> permisosList = await _permisoRepositorio.ObtenerTodos();


                _response.Resultado = _mapper.Map<IEnumerable<PermisoDto>>(permisosList);
                _response.statusCode = HttpStatusCode.OK;
                _response.IsExistoso = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("PorRol/{idRol:int}", Name = "PermisosPorRol")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<ActionResult<APIResponse>> PermisosPorRol(int idRol)
        {
            try
            {
                if (idRol == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var rolPermiso = await _rolPermisoRepositorio.Consultar();
                var permiso = await _permisoRepositorio.Consultar();

                IQueryable<Permiso> permisos = (from rp in rolPermiso
                                                join p in permiso on rp.IdPermiso equals p.IdPermiso
                                                where rp.IdRol == idRol
                                                select p).AsQueryable();

                var listaPermisos = permisos.ToList();

                _response.Resultado = _mapper.Map<IEnumerable<PermisoDto>>(listaPermisos);
                _response.statusCode = HttpStatusCode.OK;
                _response.IsExistoso = true;

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