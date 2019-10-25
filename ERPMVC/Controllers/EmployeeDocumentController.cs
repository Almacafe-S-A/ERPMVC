using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public EmployeeDocumentController(IHostingEnvironment hostingEnvironment
            , ILogger<EmployeeDocumentController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EmployeeDocument()
        {
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
        public async Task<ActionResult<EmployeeDocument>> SaveEmployeeDocument(IEnumerable<IFormFile> files, EmployeeDocumentDTO _EmployeeDocument)
        {

            try
            {

                EmployeeDocument _listEmployeeDocument = new EmployeeDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeDocument/GetEmployeeDocumentById/" + _EmployeeDocument.EmployeeDocumentId);
                string valorrespuesta = "";

                foreach (var file in files)
                {


                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpg")
                        || info.Extension.Equals(".png")
                       || info.Extension.Equals(".xls") || info.Extension.Equals(".xlsx"))
                    {

                        _EmployeeDocument.FechaModificacion = DateTime.Now;
                        _EmployeeDocument.UsuarioModificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listEmployeeDocument = JsonConvert.DeserializeObject<EmployeeDocument>(valorrespuesta);
                        }

                        if (_listEmployeeDocument == null) { _listEmployeeDocument = new Models.EmployeeDocument(); }
                        if (_listEmployeeDocument.EmployeeDocumentId == 0)
                        {
                            _EmployeeDocument.FechaCreacion = DateTime.Now;
                            _EmployeeDocument.DocumentName = file.FileName;
                            _EmployeeDocument.UsuarioCreacion = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_EmployeeDocument);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _EmployeeDocument = ((EmployeeDocumentDTO)(value));
                        }
                        else
                        {
                            _EmployeeDocument.DocumentName = file.FileName;
                            _EmployeeDocument.FechaCreacion = _listEmployeeDocument.FechaCreacion;
                            _EmployeeDocument.UsuarioCreacion = _listEmployeeDocument.UsuarioCreacion;
                            var updateresult = await Update(_EmployeeDocument.EmployeeDocumentId, _EmployeeDocument);
                        }



                        var filePath = _hostingEnvironment.WebRootPath + "/EmployeeDocuments/" + _EmployeeDocument.EmployeeDocumentId + "_"
                            + file.FileName.Replace(info.Extension, "") + "_" + _EmployeeDocument.DocumentTypeId + "_" + _EmployeeDocument.DocumentTypeName
                            + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // MemoryStream mstream = new MemoryStream();
                            //mstream.WriteTo(stream);
                        }

                        _EmployeeDocument.Path = filePath;
                        var updateresult2 = await Update(_EmployeeDocument.EmployeeDocumentId, _EmployeeDocument);
                    }
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

        [HttpPost("[action]")]
        public async Task<ActionResult<EmployeeDocument>> Delete([FromBody]EmployeeDocument _EmployeeDocument)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

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