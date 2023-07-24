using AutoMapper;
using JalbacApi.Models;
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
        protected APIResponse _response;

        public PermisoController(IMapper mapper, IPermisoRepositorio permisoRepositorio)
        {
            _mapper = mapper;
            _permisoRepositorio = permisoRepositorio;
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
    }
}