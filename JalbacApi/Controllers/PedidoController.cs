using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.PedidoDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IDetallePedidoRepositorio _detallePedidoRepositorio;
        private readonly IHisEstadoPedidoRepositorio _hisEstadoPedidoRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public PedidoController(IPedidoRepositorio pedidoRepositorio, IDetallePedidoRepositorio detallePedidoRepositorio, IHisEstadoPedidoRepositorio hisEstadoPedido, IMapper mapper)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _detallePedidoRepositorio = detallePedidoRepositorio;
            _hisEstadoPedidoRepositorio = hisEstadoPedido;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetPedidos()
        {
            try
            {
                IEnumerable<Pedido> pedidosList = await _pedidoRepositorio.ObtenerTodos(incluirPropiedades: "IdClienteNavigation,IdEstadoNavigation");


                _response.Resultado = _mapper.Map<IEnumerable<PedidoDto>>(pedidosList);
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

        [HttpGet("{id:int}", Name = "GetPedido")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetPedido(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var pedido = await _pedidoRepositorio.Obtener(c => c.IdPedido == id, tracked: false, incluirPropiedades: "IdClienteNavigation,IdEstadoNavigation");

                if (pedido == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<PedidoDto>(pedido);
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
        public async Task<ActionResult<APIResponse>> CrearPedido([FromBody] PedidoCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            Pedido pedido = new()
            {
                IdCliente = model.IdCliente,
                IdEstado = model.IdEstado,
                FechaEntrega = DateTime.Parse(model.FechaEntrega),
            };

            await _pedidoRepositorio.CrearPedido(pedido);

            foreach (var item in model.DetallesPedido)
            {
                DetallePedido detallePedido = new()
                {
                    IdPedido = pedido.IdPedido,
                    IdEmpleado = item.IdEmpleado,
                    IdEstado = item.IdEstado,
                    NombreAnillido = item.NombreAnillido,
                    Tipo = item.Tipo,
                    Peso = item.Peso,
                    TamanoAnillo = item.TamanoAnillo,
                    TamanoPiedra = item.TamanoPiedra,
                    Material = item.Material,
                    Detalle = item.Detalle,
                    Cantidad = item.Cantidad,
                };

                await _detallePedidoRepositorio.Crear(detallePedido);
            }
            HisEstadoPedido hisEstadoPedido = new()
            {
                IdEstado = pedido.IdEstado,
                IdPedido = pedido.IdPedido,

            };

            await _hisEstadoPedidoRepositorio.CrearHisPedido(hisEstadoPedido);


            _response.IsExistoso = true;
            _response.Resultado = pedido;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetPedido", new { id = pedido.IdPedido }, _response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> EditarPedido(int id, [FromBody] PedidoUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            // Paso 1: Obtener el pedido original de la base de datos
            var pedidoOriginal = await _pedidoRepositorio.Obtener(p => p.IdPedido == id);

            // Paso 2: Comparar el estado original con el nuevo estado
            if (pedidoOriginal.IdEstado != model.IdEstado)
            {
                // Paso 3: Crear un nuevo registro en el historial de estados
                HisEstadoPedido hisEstadoPedido = new()
                {
                    IdPedido = pedidoOriginal.IdPedido,
                    IdEstado = model.IdEstado,
                    Fecha = DateTime.Now // Puedes ajustar la fecha según tu necesidad
                };

                await _hisEstadoPedidoRepositorio.CrearHisPedido(hisEstadoPedido);
            }

            // Paso 4: Actualizar el pedido con el nuevo estado y otros datos
            _mapper.Map(model, pedidoOriginal);
            await _pedidoRepositorio.Editar(pedidoOriginal);

            _response.IsExistoso = true;
            _response.Resultado = pedidoOriginal;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarPedido(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var pedido = await _pedidoRepositorio.Obtener(c => c.IdPedido == id);

            if (pedido == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _pedidoRepositorio.Remover(pedido);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}