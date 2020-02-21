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
    public class CustomerDocumentController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        public CustomerDocumentController(IHostingEnvironment hostingEnvironment
            ,ILogger<CustomerDocumentController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomerDocument()
        {
            return PartialView();
        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerDocumentUpload([FromBody]CustomerDocument _CustomerDocumentp)
        {
            CustomerDocument _CustomerDocument = new CustomerDocument();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GetCustomerDocumentById/" + _CustomerDocumentp.CustomerDocumentId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocument = JsonConvert.DeserializeObject<CustomerDocument>(valorrespuesta);

                }

                if (_CustomerDocument == null)
                {
                    _CustomerDocument = new CustomerDocument();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerDocument);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerDocument> _CustomerDocument = new List<CustomerDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GetCustomerDocument");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocument = JsonConvert.DeserializeObject<List<CustomerDocument>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerDocument.ToDataSourceResult(request);

        }


        public async Task<ActionResult> SFCustomerDocument(Int64 id)
        {

            try
            {
                CustomerDocument _CustomerDocument = new CustomerDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GetCustomerDocumentById/" + id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocument = JsonConvert.DeserializeObject<CustomerDocument>(valorrespuesta);

                }

                ViewBag.pathcontrato = _CustomerDocument.Path;


                if (System.IO.File.Exists(_CustomerDocument.Path))
                {
                    var stream = new FileStream(_CustomerDocument.Path, FileMode.Open);
                    return new FileStreamResult(stream, "application/pdf");
                }
                else
                {
                    return await Task.Run(() => BadRequest($"No se encontro el archivo."));
                }
                
            }
            catch (Exception ex)
            {

                throw ex; 
            }
       

            return View();
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GeDocumentByCustomerId([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<CustomerDocument> _CustomerDocument = new List<CustomerDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GeDocumentByCustomerId/"+ CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocument = JsonConvert.DeserializeObject<List<CustomerDocument>>(valorrespuesta);
                    _CustomerDocument = _CustomerDocument.OrderByDescending(x => x.CustomerDocumentId).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerDocument.ToDataSourceResult(request);

        }


       

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CustomerDocument>> SaveCustomerDocument(IEnumerable<IFormFile> files,CustomerDocumentDTO _CustomerDocument)
        {            
            var Respuestasd = "TipoDocumentoRequerido";
            var ImagenRequerida = "ImagenRequerida";
            int existedocumentos = 0;
            
            if (_CustomerDocument.DocumentTypeId == 0){return Json(Respuestasd);}
            _CustomerDocument.CustomerId = _CustomerDocument.CustomerParametrosave;

            IFormFile filesDocument = files.FirstOrDefault();

            try
            {               

                CustomerDocument _listCustomerDocument = new CustomerDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GetCustomerDocumentById/" + _CustomerDocument.CustomerDocumentId);
                string valorrespuesta = "";


                if (_CustomerDocument.DocumentName != null && filesDocument == null)
                {
                    

                        _CustomerDocument.FechaModificacion = DateTime.Now;
                        _CustomerDocument.UsuarioModificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listCustomerDocument = JsonConvert.DeserializeObject<CustomerDocument>(valorrespuesta);
                        }

                    var DocuemntoNames = await GetDocumentoDuplicate(_CustomerDocument);
                    existedocumentos = Convert.ToInt32(DocuemntoNames.Value);

                    if (existedocumentos > 0)
                    {
                        return Json(existedocumentos);
                    }
                    else
                    {
                        var updateresult = await Update(_CustomerDocument.CustomerDocumentId, _CustomerDocument);
                    }

                    return Json(_CustomerDocument);

                }
                else { 

                foreach (var file in files)
                {
                    ImagenRequerida = "ImagenRequeridaNo";

                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpeg") 
                        || info.Extension.Equals(".png") || info.Extension.Equals(".txt"))
                    {                      

                        _CustomerDocument.FechaModificacion = DateTime.Now;
                        _CustomerDocument.UsuarioModificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listCustomerDocument = JsonConvert.DeserializeObject<CustomerDocument>(valorrespuesta);
                        }

                        if (_listCustomerDocument == null) { _listCustomerDocument = new Models.CustomerDocument(); }
                        if (_listCustomerDocument.CustomerDocumentId == 0)
                        {
                            _CustomerDocument.FechaCreacion = DateTime.Now;
                            _CustomerDocument.DocumentName = file.FileName;
                            _CustomerDocument.UsuarioCreacion = HttpContext.Session.GetString("user");

                            var DocuemntoNames = await GetDocumentoDuplicate(_CustomerDocument);
                            existedocumentos = Convert.ToInt32(DocuemntoNames.Value);

                            if (existedocumentos > 0)
                            {
                                return Json(existedocumentos);
                            }
                            else
                            {
                                var insertresult = await Insert(_CustomerDocument);
                                var value = (insertresult.Result as ObjectResult).Value;
                                _CustomerDocument = ((CustomerDocumentDTO)(value));
                            }
                        }
                        else
                        {

                            if (System.IO.File.Exists(_listCustomerDocument.Path))
                                System.IO.File.Delete(_listCustomerDocument.Path);
                            _CustomerDocument.DocumentName = file.FileName;
                            _CustomerDocument.UsuarioCreacion = _listCustomerDocument.UsuarioCreacion;
                            _CustomerDocument.FechaCreacion = _listCustomerDocument.FechaCreacion;


                            var DocuemntoNames = await GetDocumentoDuplicate(_CustomerDocument);
                            existedocumentos = Convert.ToInt32(DocuemntoNames.Value);

                            if (existedocumentos > 0)
                            {
                                return Json(existedocumentos);
                            }
                            else
                            {
                                var updateresult = await Update(_CustomerDocument.CustomerDocumentId, _CustomerDocument);
                            }

                          
                        }

                       
                       
                        var filePath = _hostingEnvironment.WebRootPath + "/CustomerDocuments/" + _CustomerDocument.CustomerDocumentId+"_" 
                            + file.FileName.Replace(info.Extension,"") +"_"+ _CustomerDocument.DocumentTypeId+"_"+_CustomerDocument.DocumentTypeName
                            + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // MemoryStream mstream = new MemoryStream();
                            //mstream.WriteTo(stream);
                        }

                        _CustomerDocument.Path = filePath;
                        var updateresult2 = await Update(_CustomerDocument.CustomerDocumentId, _CustomerDocument);
                    }
                }
            }

                if (ImagenRequerida == "ImagenRequerida")
                {
                    return Json(ImagenRequerida);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerDocument);
        }



        // POST: CustomerDocument/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerDocumentDTO>> Insert(CustomerDocumentDTO _CustomerDocument)
        {
            CustomerDocumentDTO _custo = new CustomerDocumentDTO();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerDocument.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerDocument.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerDocument/Insert", _CustomerDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _custo = JsonConvert.DeserializeObject<CustomerDocumentDTO>(valorrespuesta);
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
        public async Task<ActionResult<CustomerDocumentDTO>> Update(Int64 id, CustomerDocumentDTO _CustomerDocument)
        {
            CustomerDocumentDTO _CustomerDocumentDTO = new CustomerDocumentDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               
                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerDocument/Update", _CustomerDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocumentDTO = JsonConvert.DeserializeObject<CustomerDocumentDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocumentDTO }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerDocument>> Delete([FromBody]CustomerDocument _CustomerDocument)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerDocument/Delete", _CustomerDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocument = JsonConvert.DeserializeObject<CustomerDocument>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocument }, Total = 1 });
        }


        [HttpGet]
        public async Task<JsonResult> GetDocumentoDuplicate(CustomerDocument Documento)
        {
            Int32 Existe = 0;

            CustomerDocument Contiene = new CustomerDocument();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerDocument/GetDocumento", Documento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    Existe = JsonConvert.DeserializeObject<Int32>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(Existe);
        }


    }
}