using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.EmpleadoDtos;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JalbacApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public EmpleadoController(IEmpleadoRepositorio empleadoRepositorio, IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
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
                    _response.ErrorMessages.Add("No se ha proporcionado ningún Id");
                    return BadRequest(_response);
                }

                var empleado = await _empleadoRepositorio.Obtener(c => c.IdEmpleado == id, tracked: false, incluirPropiedades: "IdUsuarioNavigation");

                if (empleado == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    _response.ErrorMessages.Add("El empleado no existe");
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
                _response.ErrorMessages.Add("El empleado que se intenta editar no existe");
                return BadRequest(_response);
            }

            Empleado empleado = _mapper.Map<Empleado>(model);

            Usuario usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == model.IdUsuario);

            if (usuario != null)
            {
                // Actualizar el correo en el objeto de Usuario
                usuario.Correo = model.Correo;

                // Actualizar el objeto de Usuario en la base de datos
                await _usuarioRepositorio.Editar(usuario);
            }
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
                _response.ErrorMessages.Add("No se ha proporcionado ningún Id");
                return BadRequest(_response);
            }

            var empleado = await _empleadoRepositorio.Obtener(c => c.IdEmpleado == id, incluirPropiedades: "DetallePedidos");

            if (empleado == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("El empleado que se intenta eliminar no existe");
                return NotFound(_response);
            }

            try
            {
                if (empleado.DetallePedidos.Any())
                {
                    _response.IsExistoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("El empleado tiene detalles de pedido y no se puede eliminar");
                    return BadRequest(_response);
                }

                await _empleadoRepositorio.Remover(empleado);

                _response.IsExistoso = true;
                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (DbUpdateException)
            {
                // Manejar la excepción y devolver tu propia respuesta
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("No se puede eliminar el empleado debido a un error interno.");
                return BadRequest(_response);
            }

        }
    }
}
