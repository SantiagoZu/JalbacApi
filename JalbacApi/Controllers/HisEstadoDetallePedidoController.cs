using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.HisEstadoDetallePedidoDtos;
using JalbacApi.Models.Dto.HisEstadoPedidoDtos;
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
    public class HisEstadoDetallePedidoController : ControllerBase
    {
        private readonly IHisEstadoDetallePedidoRepositorio _hisDetallePedidoRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public HisEstadoDetallePedidoController(IHisEstadoDetallePedidoRepositorio hisDetallePedidoRepositorio, IMapper mapper)
        {
            _hisDetallePedidoRepositorio = hisDetallePedidoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetHisDetallePedidos()
        {
            try
            {
                IEnumerable<HisEstadoDetallePedido> detallesList = await _hisDetallePedidoRepositorio.ObtenerTodos(tracked: false, incluirPropiedades: "IdEstadoNavigation,IdDetallePedidoNavigation");


                _response.Resultado = _mapper.Map<IEnumerable<HisEstadoDetallePedidoDto>>(detallesList);
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

        [HttpGet("{id:int}", Name = "GetHisDetallePedido")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetHisDetallePedido(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var detalle = await _hisDetallePedidoRepositorio.Obtener(c => c.IdHisEstadoDetallePedido == id, tracked: false, incluirPropiedades: "IdEstadoNavigation,IdDetallePedidoNavigation");

                if (detalle == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<HisEstadoDetallePedidoDto>(detalle);
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

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarHisDetallePedido(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var histDetallePedido = await _hisDetallePedidoRepositorio.Obtener(c => c.IdHisEstadoDetallePedido == id);

            if (histDetallePedido == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _hisDetallePedidoRepositorio.Remover(histDetallePedido);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}