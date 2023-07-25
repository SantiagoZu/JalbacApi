using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.RolDtos;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRolRepositorio _rolRepositorio;
        private readonly IRolPermisoRepositorio _rolPermisoRepositorio;
        protected APIResponse _response;
        public RolController(IMapper mapper, IRolRepositorio rolRepositorio, IRolPermisoRepositorio rolPermisoRepositorio)
        {
            _mapper = mapper;
            _rolRepositorio = rolRepositorio;
            _rolPermisoRepositorio = rolPermisoRepositorio;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetRoles()
        {
            try
            {
                IEnumerable<Rol> rolesList = await _rolRepositorio.ObtenerTodos();


                _response.Resultado = _mapper.Map<IEnumerable<RolDto>>(rolesList);
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

        [HttpGet("{id:int}", Name = "GetRol")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetRol(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var rol = await _rolRepositorio.Obtener(c => c.IdRol == id);

                if (rol == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<RolDto>(rol);
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

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> CrearRol([FromBody] RolCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            Rol crearRol = new()
            {
                Nombre = model.Nombre,
            };

            await _rolRepositorio.Crear(crearRol);

            foreach (var item in model.Permisos)
            {
                RolPermiso crearRolPermiso = new()
                {
                    IdRol = crearRol.IdRol,
                    IdPermiso = item.IdPermiso,
                };

                await _rolPermisoRepositorio.Crear(crearRolPermiso);
            }



            _response.IsExistoso = true;
            _response.Resultado = crearRol;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetRol", new { id = crearRol.IdRol }, _response);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> EditarRol(int id, [FromBody] RolUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Rol updateRol = new()
            {
                IdRol = id,
                Nombre = model.Nombre,
                Estado = model.Estado
            };

            await _rolRepositorio.Editar(updateRol);

            var rolPermiso = await _rolPermisoRepositorio.Consultar(rp => rp.IdRol == id);

            foreach (var item in rolPermiso)
            {
                await _rolPermisoRepositorio.Remover(item);
            }

            foreach (var item2 in model.Permisos)
            {
                RolPermiso crearRolPermiso = new()
                {
                    IdRol = id,
                    IdPermiso = item2.IdPermiso
                };

                await _rolPermisoRepositorio.Crear(crearRolPermiso);
            }


            _response.IsExistoso = true;
            _response.Resultado = updateRol;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarRol(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var rol = await _rolRepositorio.Obtener(c => c.IdRol == id);

            if (rol == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _rolRepositorio.Remover(rol);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }



    }
}
