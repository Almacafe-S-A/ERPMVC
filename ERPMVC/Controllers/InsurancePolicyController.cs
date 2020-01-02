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
    public class InsurancePolicyController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        public InsurancePolicyController(IHostingEnvironment hostingEnvironment
            , ILogger<InsurancePolicyController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult InsurancePolicy()
        {
            return View();
        }

        //[HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddInsurancePolicy([FromBody]InsurancePolicyDTO _InsurancePolicyDocumentp)
        {
            InsurancePolicyDTO _InsurancePolicyDocument = new InsurancePolicyDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancePolicy/GetInsurancePolicyById/" + _InsurancePolicyDocumentp.InsurancePolicyId);
                string valorrespuesta = "";
                //_InsurancePolicyDocument.PolicyDate = DateTime.Now;
                //_InsurancePolicyDocument.PolicyDueDate = DateTime.Now;
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancePolicyDocument = JsonConvert.DeserializeObject<InsurancePolicyDTO>(valorrespuesta);

                }

                if (_InsurancePolicyDocument == null)
                {
                    _InsurancePolicyDocument = new InsurancePolicyDTO();
                    _InsurancePolicyDocument.PolicyDate = DateTime.Now;
                    _InsurancePolicyDocument.PolicyDueDate = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            //
            return PartialView(_InsurancePolicyDocument);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InsurancePolicy> _InsurancePolicy = new List<InsurancePolicy>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancePolicy/GetInsurancePolicy");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancePolicy = JsonConvert.DeserializeObject<List<InsurancePolicy>>(valorrespuesta);
                    _InsurancePolicy = _InsurancePolicy.OrderByDescending(q => q.InsurancePolicyId).ToList();


                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsurancePolicy.ToDataSourceResult(request);

        }


        /*[HttpGet("[action]")]
        public async Task<DataSourceResult> GeDocumentByCustomerId([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<CustomerDocument> _CustomerDocument = new List<CustomerDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GeDocumentByCustomerId/" + CustomerId);
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

    */


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InsurancePolicy>> SaveInsurancePolicy(IEnumerable<IFormFile> files, InsurancePolicy _InsurancePolicyS)
        {
            //InsurancePolicy _InsurancePolicy = _InsurancePolicyS;
            try
            {
                InsurancePolicy _listInsurancePolicy = new InsurancePolicy();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancePolicy/GetInsurancePolicyById/" + _InsurancePolicyS.InsurancePolicyId);
                string valorrespuesta = "";
                IFormFile file = files.FirstOrDefault();
                _InsurancePolicyS.FechaModificacion = DateTime.Now;
                _InsurancePolicyS.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInsurancePolicy = JsonConvert.DeserializeObject<InsurancePolicy>(valorrespuesta);
                }

                if (_listInsurancePolicy == null) { _listInsurancePolicy = new Models.InsurancePolicy(); }
                if (file!=null)
                {

                    FileInfo info = new FileInfo(file.FileName);
                    var filePath = _hostingEnvironment.WebRootPath + "\\InsurancePolicy\\" + _InsurancePolicyS.InsurancePolicyId + "_"
                     + file.FileName.Replace(info.Extension, "") + "_" + file.FileName;
                    filePath =  filePath.Replace("\\", "\\\\");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        // MemoryStream mstream = new MemoryStream();
                        //mstream.WriteTo(stream);
                    }

                    _InsurancePolicyS.AttachmentURL = filePath;
                    _InsurancePolicyS.AttachementFileName = file.FileName;
                    _listInsurancePolicy = _InsurancePolicyS;
                    //var updateresult2 = await Update(_InsurancePolicyS.InsurancePolicyId, _listInsurancePolicy);

                }
                if (_listInsurancePolicy.InsurancePolicyId == 0)
                {
                    _InsurancePolicyS.FechaCreacion = DateTime.Now;
                    _InsurancePolicyS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsurancePolicyS);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _InsurancePolicyS = ((InsurancePolicy)(value));
                }
                else
                {
                    _InsurancePolicyS.FechaCreacion = _listInsurancePolicy.FechaCreacion;
                    _InsurancePolicyS.UsuarioCreacion = _listInsurancePolicy.UsuarioCreacion;
                    var updateresult = await Update(_InsurancePolicyS.InsurancePolicyId, _InsurancePolicyS);
                }      
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InsurancePolicyS);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsurancePolicy>> Insert(InsurancePolicy _InsurancePolicy)
        {
            InsurancePolicy _custo = new InsurancePolicy();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsurancePolicy.UsuarioCreacion = HttpContext.Session.GetString("user");
                _InsurancePolicy.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InsurancePolicy/Insert", _InsurancePolicy);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _custo = JsonConvert.DeserializeObject<InsurancePolicy>(valorrespuesta);
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
        public async Task<ActionResult<InsurancePolicy>> Update(Int64 id, InsurancePolicy _InsurancePolicyDocument)
        {
            InsurancePolicy _InsurancePolicy = new InsurancePolicy();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsurancePolicyDocument.FechaModificacion = DateTime.Now;
                _InsurancePolicyDocument.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/InsurancePolicy/Update", _InsurancePolicyDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancePolicy = JsonConvert.DeserializeObject<InsurancePolicy>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancePolicyDocument }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsurancePolicy>> Delete([FromBody]InsurancePolicy _InsurancePolicyDocument)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InsurancePolicy/Delete", _InsurancePolicyDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancePolicyDocument = JsonConvert.DeserializeObject<InsurancePolicy>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancePolicyDocument }, Total = 1 });
        }





    }
}