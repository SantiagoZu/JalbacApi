using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.DetallePedidoDtos;
using JalbacApi.Models.Dto.PedidoDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace JalbacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoController : ControllerBase
    {
        private readonly IDetallePedidoRepositorio _detalleRepositorio;
        private readonly IHisEstadoDetallePedidoRepositorio _hisEstadoDetallePedidoRepositorio;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public DetallePedidoController(IDetallePedidoRepositorio detalleRepositorio, IHisEstadoDetallePedidoRepositorio hisEstadoDetallePedidoRepositorio, IEmpleadoRepositorio empleadoRepositorio, IMapper mapper)
        {
            _detalleRepositorio = detalleRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _hisEstadoDetallePedidoRepositorio = hisEstadoDetallePedidoRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetDetalles()
        {
            try
            {
                IEnumerable<DetallePedido> detallesList = await _detalleRepositorio.ObtenerTodos(tracked: false, incluirPropiedades: "IdEmpleadoNavigation,IdEstadoNavigation,IdPedidoNavigation");

                List<DetallePedidoDto> detallesWithDevolucion = new List<DetallePedidoDto>();
                foreach (var detalle in detallesList)
                {
                    var motivosDevolucion = await _hisEstadoDetallePedidoRepositorio.ObtenerTodos(dm => dm.IdDetallePedido == detalle.IdDetallePedido);

                    List<string> motivos = new List<string>();

                    foreach (var motivo in motivosDevolucion)
                    {
                        motivos.Add(motivo.MotivoDevolucion);

                    }

                    DetallePedidoDto detallePedidoDto = new()
                    {
                        IdDetallePedido = detalle.IdDetallePedido,
                        IdPedido = detalle.IdPedido,
                        IdEmpleado = detalle.IdEmpleado,
                        IdEstado = detalle.IdEstado,
                        IdPedidoNavigation = detalle.IdPedidoNavigation,
                        IdEmpleadoNavigation = detalle.IdEmpleadoNavigation,
                        IdEstadoNavigation = detalle.IdEstadoNavigation,
                        NombreAnillido = detalle.NombreAnillido,
                        Servicio = detalle.Tipo,
                        Peso = detalle.Peso,
                        TamanoAnillo = detalle.TamanoAnillo,
                        TamanoPiedra = detalle.TamanoPiedra,
                        Material = detalle.Material,
                        Detalle = detalle.Detalle,
                        Cantidad = detalle.Cantidad,
                        MotivoDevolucion = motivos
                    };
                    detallesWithDevolucion.Add(detallePedidoDto);
                }

                _response.Resultado = _mapper.Map<IEnumerable<DetallePedidoDto>>(detallesWithDevolucion);
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

        [HttpGet("{id:int}", Name = "GetDetalle")]

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetDetalle(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var detalle = await _detalleRepositorio.Obtener(c => c.IdDetallePedido == id, tracked: false);

                if (detalle == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<DetallePedidoDto>(detalle);
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
        public async Task<ActionResult<APIResponse>> CrearDetalle([FromBody] List<DetalleCreateDto> model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            List<int> listEmpleadoIds = new List<int>();

            foreach (var detalle in model)
            {
                var empleado = await _empleadoRepositorio.Obtener(empleado => empleado.Documento == detalle.DocumentoEmpleado, tracked: false);
                listEmpleadoIds.Add(empleado.IdEmpleado);
            }
            List<DetallePedido> detalles = _mapper.Map<List<DetallePedido>>(model);

            int indexEmpleadoIds = 0;

            foreach (DetallePedido detalle in detalles)
            {
                detalle.IdEmpleado = listEmpleadoIds[indexEmpleadoIds];
                await _detalleRepositorio.Crear(detalle);
                HisEstadoDetallePedido hisEstadoDetallePedido = new()
                {
                    IdEstado = detalle.IdEstado,
                    IdDetallePedido = detalle.IdDetallePedido,

                };

                await _hisEstadoDetallePedidoRepositorio.CrearHisDetallePedido(hisEstadoDetallePedido);
                indexEmpleadoIds++;
            }

            _response.IsExistoso = true;
            _response.Resultado = detalles;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetDetalle", new { id = detalles[0].IdDetallePedido }, _response);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> EditarDetalle(int id, [FromBody] DetalleUpdateDto model)
        {
            if (model == null || id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var detalleOriginal = await _detalleRepositorio.Obtener(d => d.IdDetallePedido == id);

            if (detalleOriginal.IdEstado != model.IdEstado)
            {
                HisEstadoDetallePedido hisEstadoDetallePedido = new()
                {
                    IdEstado = model.IdEstado,
                    IdDetallePedido = model.IdDetallePedido,
                    Fecha = DateTime.Now,
                    MotivoDevolucion = model.MotivoDevolucion,
                };
                await _hisEstadoDetallePedidoRepositorio.CrearHisDetallePedido(hisEstadoDetallePedido);
            };

            _mapper.Map(model, detalleOriginal);

            await _detalleRepositorio.Editar(detalleOriginal);

            IEnumerable<DetallePedido> detallesList = await _detalleRepositorio.ObtenerTodos(tracked: false, incluirPropiedades: "IdEmpleadoNavigation,IdEstadoNavigation,IdPedidoNavigation");
            _response.IsExistoso = true;
            _response.Resultado = new { detalleEdited = detalleOriginal, detallesActualizados = detallesList };
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> EliminarDetalle(int id)
        {
            if (id == 0)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var detalle = await _detalleRepositorio.Obtener(c => c.IdDetallePedido == id);
            var histDetalle = await _hisEstadoDetallePedidoRepositorio.Obtener(hist => hist.IdDetallePedido == detalle.IdDetallePedido);

            if (detalle == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _hisEstadoDetallePedidoRepositorio.Remover(histDetalle);

            await _detalleRepositorio.Remover(detalle);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}