using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.EmpleadoDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JalbacApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly IRolRepositorio _rolRepositorio;
        private readonly IDetallePedidoRepositorio _detallePedidoRepositorio;
        protected APIResponse _response;
        public EmpleadoController(IEmpleadoRepositorio empleadoRepositorio, IUsuarioRepositorio usuarioRepositorio, IMapper mapper, IRolRepositorio rolRepositorio, IDetallePedidoRepositorio detallePedidoRepositorio)
        {
            _empleadoRepositorio = empleadoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _rolRepositorio = rolRepositorio;
            _detallePedidoRepositorio = detallePedidoRepositorio;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetEmpleados()
        {
            try
            {
                IEnumerable<Empleado> empleadosList = await _empleadoRepositorio.ObtenerTodos(incluirPropiedades: "IdUsuarioNavigation");
                empleadosList = empleadosList.OrderBy(e => e.Estado ? 0 : 1).ThenByDescending(e => e.IdEmpleado);

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

            Usuario usuario = new()
            {
                Correo = model.Correo,
                Contrasena = model.Contrasena,
                IdRol = model.IdRol,
                Estado = model.Estado,
            };

            await _usuarioRepositorio.CrearUsuario(usuario);

            Empleado empleado = new()
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Cargo = model.Cargo,
                Documento = model.Documento,
                Estado = model.Estado,
                IdUsuario = usuario.IdUsuario,
            };

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

            var empleadoAEditar = await _empleadoRepositorio.Obtener(e => e.IdEmpleado == id, tracked: false);
            var detallesDeEmpleado = await _detallePedidoRepositorio.ObtenerTodos(de => de.IdEmpleado ==  model.IdEmpleado, tracked: false);

            if (model.Estado == false && detallesDeEmpleado.Count() > 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("El empleado tiene un pedido pendiente");
                return BadRequest(_response);
            }

            Empleado empleado = _mapper.Map<Empleado>(model);
            await _empleadoRepositorio.Editar(empleado);

            var usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == model.IdUsuario);

            if (usuario != null)
            {
                // Actualizar el correo en el objeto de Usuario
                usuario.IdRol = model.IdRol;
                usuario.Correo = model.Correo;
                usuario.Estado = model.Estado;

                // Actualizar el objeto de Usuario en la base de datos
                await _usuarioRepositorio.Editar(usuario);
            }
            

            if (model.Estado == true)
            {
                var rol = await _rolRepositorio.Obtener(r => r.IdRol == usuario.IdRol);

                rol.Estado = model.Estado;
                await _rolRepositorio.Editar(rol);
            }

            

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
                    _response.ErrorMessages.Add("El empleado tiene un pedido pendiente");
                    return BadRequest(_response);
                }

                Usuario usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == empleado.IdUsuario);

                await _empleadoRepositorio.Remover(empleado);
                await _usuarioRepositorio.Remover(usuario);

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

        [HttpPost("{documento}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> ValidarDocumento(string documento)
        {
            if (documento == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var empleado = await _empleadoRepositorio.Obtener(e => e.Documento == documento);

            if (empleado != null)
            {
                _response.IsExistoso = true;
                _response.statusCode = HttpStatusCode.Accepted;
                _response.ErrorMessages.Add("Ya existe un empleado con el mismo documento");
                return Ok(_response);
            }

            _response.IsExistoso = false;
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }

        
    }
}
