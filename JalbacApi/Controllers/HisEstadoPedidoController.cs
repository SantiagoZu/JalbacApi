using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
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
    public class HisEstadoPedidoController : ControllerBase
    {
        private readonly IHisEstadoPedidoRepositorio _hisPedidoRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public HisEstadoPedidoController(IHisEstadoPedidoRepositorio hisPedidoRepositorio, IMapper mapper)
        {
            _hisPedidoRepositorio = hisPedidoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetHisPedidos()
        {
            try
            {
                IEnumerable<HisEstadoPedido> pedidosList = await _hisPedidoRepositorio.ObtenerTodos(tracked: false, incluirPropiedades: "IdEstadoNavigation,IdPedidoNavigation");


                _response.Resultado = _mapper.Map<IEnumerable<HisEstadoPedidoDto>>(pedidosList);
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

        [HttpGet("{id:int}", Name = "GetHisPedido")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetHisPedido(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var pedido = await _hisPedidoRepositorio.Obtener(c => c.IdHisEstadoPedido == id, tracked: false, incluirPropiedades: "IdEstadoNavigation,IdPedidoNavigation");

                if (pedido == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<HisEstadoPedidoDto>(pedido);
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