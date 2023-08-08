using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepositorio _clienteRepositorio;
        protected APIResponse _response;
        public ClienteController(IMapper mapper, IClienteRepositorio clienteRepositorio)
        {
            _mapper = mapper;
            _clienteRepositorio = clienteRepositorio;
            _response = new ();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetClientes()
        {
            try
            {
                IEnumerable<Cliente> clientesList = await _clienteRepositorio.ObtenerTodos();


                _response.Resultado = _mapper.Map<IEnumerable<ClienteDto>>(clientesList);
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

        [HttpGet("{id:int}", Name = "GetCliente")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetCliente(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var cliente = await _clienteRepositorio.Obtener(c => c.IdCliente == id);

                if (cliente == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<ClienteDto>(cliente);
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
        public async Task<ActionResult<APIResponse>> CrearCliente([FromBody] ClienteCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            Cliente cliente = _mapper.Map<Cliente>(model);

            await _clienteRepositorio.Crear(cliente);
            _response.IsExistoso = true;
            _response.Resultado = cliente;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetCliente", new { id = cliente.IdCliente }, _response);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> EditarCliente(int id, [FromBody] ClienteUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Cliente cliente = _mapper.Map<Cliente>(model);

            await _clienteRepositorio.Editar(cliente);

            _response.IsExistoso = true;
            _response.Resultado = cliente;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarCliente(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var cliente = await _clienteRepositorio.Obtener(c => c.IdCliente == id);

            if (cliente == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            try
            {
                if (cliente.Pedidos.Any())
                {
                    _response.IsExistoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("El cliente tiene un pedido a su nombre");
                    return BadRequest(_response);
                }
            }
            catch (Exception)
            {

                throw;
            }

            await _clienteRepositorio.Remover(cliente);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
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

            var cliente = await _clienteRepositorio.Obtener(c => c.Documento == documento);

            if (cliente != null)
            {
                _response.IsExistoso = true;
                _response.statusCode = HttpStatusCode.Accepted;
                _response.ErrorMessages.Add("Ya existe un cliente con el mismo documento");
                return Ok(_response);
            }

            _response.IsExistoso = false;
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }
    }
}
