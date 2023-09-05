using AutoMapper;
using JalbacApi.Models;
using JalbacApi.Models.Dto.BackupDtos;
using JalbacApi.Models.Dto.ClienteDtos;
using JalbacApi.Repositorio;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net;
using System.Net.Http.Headers;

namespace JalbacApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBackupRepositorio _backupRepositorio;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        protected APIResponse _response;
        private BdJalbacContext _context;

        public BackupController(IMapper mapper, IBackupRepositorio backupRepositorio, IEmpleadoRepositorio empleadoRepositorio, BdJalbacContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _mapper = mapper;
            _backupRepositorio = backupRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
            _response = new();
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetBackup()
        {
            try
            {
                IEnumerable<Backup> backupList = await _backupRepositorio.ObtenerTodos(incluirPropiedades: "IdEmpleadoNavigation", tracked: false);

                _response.Resultado = _mapper.Map<IEnumerable<BackupDto>>(backupList);
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

        [HttpGet("{id:int}", Name = "GetBackup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetBackup(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }

                var backup = await _backupRepositorio.Obtener(c => c.IdBackup == id);

                if (backup == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<BackupDto>(backup);
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

        [HttpGet("Download")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetBackupClient()
        {
            try
            {
                var backupFolderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Bak");

                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }

                var backupNombre = "Backup.bak";
                var BackupRuta = Path.Combine(backupFolderPath, backupNombre);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("defaultConnection")))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("GenerarBackup", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BackupRuta", BackupRuta);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                if (System.IO.File.Exists(BackupRuta))
                {
                    var stream = new FileStream(BackupRuta, FileMode.Open);
                    var response = File(stream, "application/octet-stream");
                    response.FileDownloadName = backupNombre;
                    _response.IsExistoso = true;
                    _response.statusCode = HttpStatusCode.OK;
                    //var result = new Dictionary<string, object>
                    //{
                    //    { "HttpResponse", response },
                    //    { "_response", _response }
                    //};

                    return response;
                }
                else
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
            //    try
            //    {
            //        byte[] archivoBackup = _context.RealizarBackup();

            //        if (archivoBackup != null)
            //        {
            //            // Configura la respuesta HTTP para descargar el archivo
            //            var contentDisposition = new ContentDispositionHeaderValue("attachment") //Esto le dice al navegador que debe tratar el contenido como un archivo adjunto que se debe descargar en lugar de mostrarlo en el navegador.
            //            {
            //                FileName = "Backup.bak"
            //            };
            //            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());  // Este header le indica al navegador que debe descargar el contenido con la configuración de "attachment" y el nombre de archivo especificado.

            //            return File(archivoBackup, "application/octet-stream");
            //        }
            //        else
            //        {
            //            _response.statusCode = HttpStatusCode.NotFound;
            //            _response.IsExistoso = false;
            //            return NotFound(_response);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _response.IsExistoso = false;
            //        _response.ErrorMessages = new List<string>() { ex.ToString() };
            //    }

            //    return _response;
            //}
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> CrearBackup([FromBody] BackupCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model is null)
            {
                return BadRequest(model);
            }

            Backup backup = _mapper.Map<Backup>(model);

            await _backupRepositorio.CrearBackup(backup);
            _response.IsExistoso = true;
            _response.Resultado = backup;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetBackup", new { id = backup.IdBackup }, _response);
        }
    }
}