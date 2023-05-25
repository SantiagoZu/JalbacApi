using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.UsuarioDtos;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        protected APIResponse _response;
        public UsuarioController(IMapper mapper, IUsuarioRepositorio usuarioRepositorio, IEmpleadoRepositorio empleadoRepositorio)
        {
            _mapper = mapper;
            _usuarioRepositorio = usuarioRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _response = new();
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetUsuarios()
        {
            try
            {
                IEnumerable<Usuario> usuariosList = await _usuarioRepositorio.ObtenerTodos(incluirPropiedades: "IdRolNavigation");


                _response.Resultado = _mapper.Map<IEnumerable<UsuarioDto>>(usuariosList);
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

        [HttpGet("{id:int}", Name = "GetUsuario")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetUsuario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var usuario = await _usuarioRepositorio.Obtener(c => c.IdUsuario == id, incluirPropiedades: "IdRolNavigation", tracked: false);

                if (usuario == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<UsuarioDto>(usuario);
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
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> CrearUsuario([FromBody] UsuarioCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            Usuario usuario = _mapper.Map<Usuario>(model);
            await _usuarioRepositorio.CrearUsuario(usuario);

            Empleado empleado = new()
            {
                IdUsuario = usuario.IdUsuario,
                Estado = true,
                Documento = usuario.IdUsuario.ToString(),
                Nombre = "",
                Apellido = "",
                Cargo = "",
            };
            await _empleadoRepositorio.Crear(empleado);

            _response.IsExistoso = true;
            _response.Resultado = usuario;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetUsuario", new { id = usuario.IdUsuario }, _response);

        }

        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> EditarUsuario(int id, [FromBody] UsuarioUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Usuario usuario = _mapper.Map<Usuario>(model);

            await _usuarioRepositorio.Editar(usuario);

            _response.IsExistoso = true;
            _response.Resultado = usuario;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarUsuario(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepositorio.Obtener(c => c.IdUsuario == id);

            if (usuario == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _usuarioRepositorio.Remover(usuario);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto modelo)
        {
            var loginResponse = await _usuarioRepositorio.Login(modelo, HttpContext);
            if (loginResponse.isExitoso == false)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExistoso = false;
                _response.ErrorMessages.Add("UserName o Password son incorrectos");

                return BadRequest(_response);
            }

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.OK;
            _response.Resultado = loginResponse;

            return Ok(_response);
        }
    }
}

