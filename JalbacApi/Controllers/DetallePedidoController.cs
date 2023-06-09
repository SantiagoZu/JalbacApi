﻿using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Models.Dto.DetallePedidoDtos;
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
    public class DetallePedidoController : ControllerBase
    {
        private readonly IDetallePedidoRepositorio _detalleRepositorio;
        private readonly IHisEstadoDetallePedidoRepositorio _hisEstadoDetallePedidoRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public DetallePedidoController(IDetallePedidoRepositorio detalleRepositorio,IHisEstadoDetallePedidoRepositorio hisEstadoDetallePedidoRepositorio, IMapper mapper)
        {
            _detalleRepositorio = detalleRepositorio;
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
                IEnumerable<DetallePedido> detallesList = await _detalleRepositorio.ObtenerTodos(incluirPropiedades: "IdEmpleadoNavigation,IdEstadoNavigation,IdPedidoNavigation");


                _response.Resultado = _mapper.Map<IEnumerable<DetallePedidoDto>>(detallesList);
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

            List<DetallePedido> detalles = _mapper.Map<List<DetallePedido>>(model);

            foreach (DetallePedido detalle in detalles)
            {
                await _detalleRepositorio.Crear(detalle);
                HisEstadoDetallePedido hisEstadoDetallePedido = new()
                {
                    IdEstado = detalle.IdEstado,
                    IdDetallePedido = detalle.IdDetallePedido,

                };

                await _hisEstadoDetallePedidoRepositorio.CrearHisDetallePedido(hisEstadoDetallePedido);
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

            DetallePedido detalle = _mapper.Map<DetallePedido>(model);

            await _detalleRepositorio.Editar(detalle);

            _response.IsExistoso = true;
            _response.Resultado = detalle;
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

            if (detalle == null)
            {
                _response.IsExistoso = false;
                _response.statusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _detalleRepositorio.Remover(detalle);

            _response.IsExistoso = true;
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
