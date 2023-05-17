using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.EmpleadoDtos;
using JalbacApi.Models.Dto.PedidoDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JalbacApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public EmpleadoController(IEmpleadoRepositorio empleadoRepositorio, IMapper mapper)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetEmpleados()
        {
            try
            {
                IEnumerable<Empleado> empleadosList = await _empleadoRepositorio.ObtenerTodos(incluirPropiedades: "IdUsuarioNavigation");


                _response.Resultado = _mapper.Map<IEnumerable<EmpleadoDto>>(empleadosList);
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

        [HttpGet("{id:int}", Name = "GetEmpleado")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var empleado = await _empleadoRepositorio.Obtener(c => c.IdEmpleado == id, tracked: false, incluirPropiedades: "IdUsuarioNavigation");

                if (empleado == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<EmpleadoDto>(empleado);
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
        public async Task<ActionResult<APIResponse>> CrearEmpleado([FromBody] EmpleadoCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            Empleado empleado = _mapper.Map<Empleado>(model);

            await _empleadoRepositorio.Crear(empleado);
            _response.IsExistoso = true;
            _response.Resultado = empleado;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetEmpleado", new { id = empleado.IdEmpleado }, _response);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> EditarEmpleado(int id, [FromBody] EmpleadoUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Empleado empleado = _mapper.Map<Empleado>(model);

            await _empleadoRepositorio.Editar(empleado);

            _response.IsExistoso = true;
            _response.Resultado = empleado;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarEmpleado(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var pedido = await _empleadoRepositorio.Obtener(c => c.IdEmpleado == id);

            if (pedido == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _empleadoRepositorio.Remover(pedido);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
