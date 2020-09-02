
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
    public class VendorDocumentController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public VendorDocumentController(IHostingEnvironment hostingEnvironment
            , ILogger<VendorDocumentController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _principal = httpContextAccessor.HttpContext.User;
        }

        

        public IActionResult VendorDocument()
        {
            ViewData["permisos"] = _principal;
            return PartialView();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwVendorDocumentUpload([FromBody]VendorDocument _VendorDocumentp)
        {
            VendorDocument _VendorDocument = new VendorDocument();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorDocument/GetVendorDocumentById/" + _VendorDocumentp.VendorDocumentId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorDocument = JsonConvert.DeserializeObject<VendorDocument>(valorrespuesta);

                }

                if (_VendorDocument == null)
                {
                    _VendorDocument = new VendorDocument();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_VendorDocument);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<VendorDocument> _VendorDocument = new List<VendorDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorDocument/GetVendorDocument");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorDocument = JsonConvert.DeserializeObject<List<VendorDocument>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _VendorDocument.ToDataSourceResult(request);

        }


        public async Task<ActionResult> SFVendorDocument(Int64 id)
        {

            try
            {
                VendorDocument _VendorDocument = new VendorDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorDocument/GetVendorDocumentById/" + id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorDocument = JsonConvert.DeserializeObject<VendorDocument>(valorrespuesta);

                }
                
                ViewBag.pathcontrato = _VendorDocument.Path;
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return View();
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GeDocumentByVendorId([DataSourceRequest]DataSourceRequest request, Int64 VendorId)
        {
            List<VendorDocument> _VendorDocument = new List<VendorDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorDocument/GeDocumentByVendorId/" + VendorId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorDocument = JsonConvert.DeserializeObject<List<VendorDocument>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _VendorDocument.ToDataSourceResult(request);

        }




        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<VendorDocument>> SaveVendorDocument(IEnumerable<IFormFile> files, VendorDocumentDTO _VendorDocumentd)
        {

            try
            {

                VendorDocument _listVendorDocument = new VendorDocument();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorDocument/GetVendorDocumentById/" + _VendorDocumentd.VendorDocumentId);
                string valorrespuesta = "";

                foreach (var file in files)
                {


                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpg")
                        || info.Extension.Equals(".png")
                       || info.Extension.Equals(".xls") || info.Extension.Equals(".xlsx") || info.Extension.Equals(".txt"))
                    {

                        _VendorDocumentd.ModifiedDate = DateTime.Now;
                        _VendorDocumentd.ModifiedUser = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listVendorDocument = JsonConvert.DeserializeObject<VendorDocument>(valorrespuesta);
                        }

                        if (_listVendorDocument == null) { _listVendorDocument = new Models.VendorDocument(); }
                        if (_listVendorDocument.VendorDocumentId == 0)
                        {
                            _VendorDocumentd.CreatedDate = DateTime.Now;
                            _VendorDocumentd.DocumentName = file.FileName;
                            _VendorDocumentd.CreatedUser = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_VendorDocumentd);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _VendorDocumentd = ((VendorDocumentDTO)(value));
                        }
                        else
                        {
                            var updateresult = await Update(_VendorDocumentd.VendorDocumentId, _VendorDocumentd);
                        }



                        var filePath = _hostingEnvironment.WebRootPath + "/VendorDocuments/" + _VendorDocumentd.VendorDocumentId + "_"
                            + file.FileName.Replace(info.Extension, "") + "_" + _VendorDocumentd.DocumentTypeId + "_" + _VendorDocumentd.DocumentTypeName
                            + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // MemoryStream mstream = new MemoryStream();
                            //mstream.WriteTo(stream);
                        }

                        _VendorDocumentd.Path = filePath;
                        var updateresult2 = await Update(_VendorDocumentd.VendorDocumentId, _VendorDocumentd);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_VendorDocumentd);
        }

        // POST: PurchDocument/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<VendorDocumentDTO>> Insert(VendorDocumentDTO _VendorDocumentd)
        {
            VendorDocumentDTO _VendorDocumento = new VendorDocumentDTO();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _VendorDocumentd.CreatedUser = HttpContext.Session.GetString("user");
                _VendorDocumentd.ModifiedUser = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorDocument/Insert", _VendorDocumentd);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorDocumento = JsonConvert.DeserializeObject<VendorDocumentDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_VendorDocumento);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocument }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendorDocumentDTO>> Update(Int64 id, VendorDocumentDTO _VendorDocumentd)
        {
            VendorDocumentDTO _VendorDocumentDTO = new VendorDocumentDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/VendorDocument/Update", _VendorDocumentd);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorDocumentDTO = JsonConvert.DeserializeObject<VendorDocumentDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _VendorDocumentDTO }, Total = 1 });
        }

        [HttpDelete]
        public async Task<ActionResult<VendorDocument>> Delete(Int64 Id, VendorDocument _ContactP)
        {
            VendorDocument _Contact = _ContactP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorDocument/Delete", _Contact);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Contact = JsonConvert.DeserializeObject<VendorDocument>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Contact }, Total = 1 });
        }





    }
}