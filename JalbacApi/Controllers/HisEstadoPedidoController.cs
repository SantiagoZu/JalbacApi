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
        private readonly IPedidoRepositorio _pedidoRepositorio;
        protected APIResponse _response;
        public HisEstadoPedidoController(IHisEstadoPedidoRepositorio hisPedidoRepositorio, IMapper mapper, IPedidoRepositorio pedidoRepositorio)
        {
            _hisPedidoRepositorio = hisPedidoRepositorio;
            _mapper = mapper;
            _pedidoRepositorio = pedidoRepositorio;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetHisPedidos()
        {
            try
            {
                List<HistorialesDePedidoDto> historialesDePedidoDtoList = new List<HistorialesDePedidoDto>();
                IEnumerable<Pedido> pedidos = await _pedidoRepositorio.ObtenerTodos(tracked: false, incluirPropiedades: "IdClienteNavigation,IdEstadoNavigation");
                foreach (var pedido in pedidos)
                {
                    IEnumerable<HisEstadoPedido> historialesDePedido = await _hisPedidoRepositorio.ObtenerTodos( hp => hp.IdPedido == pedido.IdPedido, incluirPropiedades: "IdEstadoNavigation", tracked: false);

                    HistorialesDePedidoDto hist = new()
                    {
                        IdPedido = pedido.IdPedido,
                        IdPedidoNavigation = _mapper.Map<PedidoDto>(pedido),
                        HistorialesDePedido = historialesDePedido.ToList(),
                    };

                    historialesDePedidoDtoList.Add(hist);
                };
                historialesDePedidoDtoList = historialesDePedidoDtoList.OrderByDescending(h => h.IdPedido).ToList();

                _response.Resultado = historialesDePedidoDtoList;
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

        

        [HttpGet("{idPedido:int}", Name = "GetHistorialesPedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetHistorialesDePedido(int idPedido)
        {
            try
            {
                var pedido = await _pedidoRepositorio.Obtener(p => p.IdPedido == idPedido, tracked: false);
                IEnumerable<HisEstadoPedido> historialesDePedido = await _hisPedidoRepositorio.ObtenerTodos(hp => hp.IdPedido == pedido.IdPedido, tracked: false);

                HistorialesDePedidoDto historialesDePedidoDto = new()
                {
                    IdPedido = pedido.IdPedido,
                    HistorialesDePedido = historialesDePedido
                };

                _response.IsExistoso = true;
                _response.statusCode = HttpStatusCode.OK;
                _response.Resultado = historialesDePedidoDto;
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