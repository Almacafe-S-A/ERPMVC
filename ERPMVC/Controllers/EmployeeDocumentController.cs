using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class EmployeeDocumentController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public EmployeeDocumentController(IHostingEnvironment hostingEnvironment
            , ILogger<EmployeeDocumentController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EmployeeDocument()
        {
            ViewData["permisos"] = _principal;
            return PartialView();
        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwEmployeeDocumentUpload([FromBody]EmployeeDocument _EmployeeDocumentp)
        {
            EmployeeDocument _EmployeeDocument = new EmployeeDocument();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeDocument/GetEmployeeDocumentById/" + _EmployeeDocumentp.EmployeeDocumentId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeDocument = JsonConvert.DeserializeObject<EmployeeDocument>(valorrespuesta);

                }

                if (_EmployeeDocument == null)
                {
                    _EmployeeDocument = new EmployeeDocument();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EmployeeDocument);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EmployeeDocument> _EmployeeDocument = new List<EmployeeDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeDocument/GetEmployeeDocument");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeDocument = JsonConvert.DeserializeObject<List<EmployeeDocument>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EmployeeDocument.ToDataSourceResult(request);

        }


        public async Task<ActionResult> SFEmployeeDocument(Int64 id)
        {

            try
            {
                EmployeeDocument _EmployeeDocument = new EmployeeDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeDocument/GetEmployeeDocumentById/" + id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeDocument = JsonConvert.DeserializeObject<EmployeeDocument>(valorrespuesta);

                }

                ViewBag.pathcontrato = _EmployeeDocument.Path;
                var stream = new FileStream(_EmployeeDocument.Path, FileMode.Open);
                return new FileStreamResult(stream, "application/pdf");
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return View();
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetDocumentByEmployeeId([DataSourceRequest]DataSourceRequest request, Int64 EmployeeId)
        {
            List<EmployeeDocument> _EmployeeDocument = new List<EmployeeDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeDocument/GetDocumentByEmployeeId/" + EmployeeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeDocument = JsonConvert.DeserializeObject<List<EmployeeDocument>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EmployeeDocument.ToDataSourceResult(request);

        }




        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EmployeeDocument>> SaveEmployeeDocument(IEnumerable<IFormFile> Archivo, EmployeeDocumentDTO _EmployeeDocumentP)
        {
            EmployeeDocument _EmployeeDocument = _EmployeeDocumentP;
            try
            {

                EmployeeDocument _listEmployeeDocument = new EmployeeDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeDocument/GetEmployeeDocumentById/" + _EmployeeDocumentP.EmployeeDocumentId);
                string valorrespuesta = "";

                IFormFile file = Archivo.FirstOrDefault();
                if (file != null)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpg") || info.Extension.Equals(".jpeg")
                        || info.Extension.Equals(".png")
                       || info.Extension.Equals(".txt") || info.Extension.Equals(".gif"))
                    {
                        _EmployeeDocumentP.FechaModificacion = DateTime.Now;
                        _EmployeeDocumentP.UsuarioModificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _EmployeeDocument = JsonConvert.DeserializeObject<EmployeeDocument>(valorrespuesta);
                        }

                        if (_EmployeeDocument == null) { _EmployeeDocument = new Models.EmployeeDocument(); }
                        if (_EmployeeDocumentP.EmployeeDocumentId == 0)
                        {
                            _EmployeeDocumentP.FechaCreacion = DateTime.Now;
                            _EmployeeDocumentP.FechaIngreso = DateTime.Now;
                            _EmployeeDocumentP.DocumentName = file.FileName;
                            _EmployeeDocumentP.UsuarioCreacion = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_EmployeeDocumentP);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _EmployeeDocumentP = ((EmployeeDocumentDTO)(value));
                        }
                        else
                        {
                            _EmployeeDocumentP.DocumentName = file.FileName;
                            _EmployeeDocumentP.FechaCreacion = _EmployeeDocument.FechaCreacion;
                            _EmployeeDocumentP.UsuarioCreacion = _EmployeeDocument.UsuarioCreacion;
                            var updateresult = await Update(_EmployeeDocumentP.EmployeeDocumentId, _EmployeeDocumentP);
                        }

                        var filename = _EmployeeDocumentP.EmployeeDocumentId + "_"
                            + file.FileName.Replace(info.Extension, "") + "_" + _EmployeeDocumentP.DocumentTypeId + "_" + _EmployeeDocumentP.DocumentTypeName
                            + info.Extension;

                        var filePath = _hostingEnvironment.WebRootPath + "/EmployeeDocuments/" + filename;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        _EmployeeDocumentP.DocumentName = filename;
                        _EmployeeDocumentP.Path = filePath;
                        var updateresult2 = await Update(_EmployeeDocument.EmployeeDocumentId, _EmployeeDocumentP);
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest("Extensión de Imagen no válida"));
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("Seleccione un Archivo"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EmployeeDocument);
        }

        // POST: CustomerDocument/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EmployeeDocumentDTO>> Insert(EmployeeDocumentDTO _EmployeeDocument)
        {
            EmployeeDocumentDTO _custo = new EmployeeDocumentDTO();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EmployeeDocument.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EmployeeDocument.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeDocument/Insert", _EmployeeDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _custo = JsonConvert.DeserializeObject<EmployeeDocumentDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_custo);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocument }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDocumentDTO>> Update(Int64 id, EmployeeDocumentDTO _EmployeeDocument)
        {
            EmployeeDocumentDTO _EmployeeDocumentDTO = new EmployeeDocumentDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EmployeeDocument/Update", _EmployeeDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeDocumentDTO = JsonConvert.DeserializeObject<EmployeeDocumentDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeDocumentDTO }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDocument>> Delete(EmployeeDocument _EmployeeDocument)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                if (System.IO.File.Exists(_EmployeeDocument.Path))
                {
                    System.IO.File.Delete(_EmployeeDocument.Path);
                }

                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeDocument/Delete", _EmployeeDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeDocument = JsonConvert.DeserializeObject<EmployeeDocument>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeDocument }, Total = 1 });
        }





    }
}