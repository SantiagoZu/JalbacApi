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
    public class DevolucionController : ControllerBase
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IHisEstadoPedidoRepositorio _hisEstadoPedido;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public DevolucionController(IPedidoRepositorio pedidoRepositorio, IHisEstadoPedidoRepositorio hisEstadoPedido, IMapper mapper)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _hisEstadoPedido = hisEstadoPedido;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetPedidos()
        {
            try
            {
                IEnumerable<Pedido> pedidosList = await _pedidoRepositorio.ObtenerTodos(
                    filtro: c => c.IdEstado == 3,
                    incluirPropiedades: "IdClienteNavigation,IdEstadoNavigation,DetallePedidos");


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

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<ActionResult<APIResponse>> EditarPedido(int id, [FromBody] PedidoUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Pedido pedido = _mapper.Map<Pedido>(model);

            await _pedidoRepositorio.Editar(pedido);

            _response.IsExistoso = true;
            _response.Resultado = pedido;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

    }
}
