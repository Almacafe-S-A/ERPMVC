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
    public class InsurancePolicyController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public InsurancePolicyController(IHostingEnvironment hostingEnvironment
            , ILogger<InsurancePolicyController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
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

        [Authorize(Policy = "Contabilidad.Seguros.Polizas")]
        public IActionResult InsurancePolicy()
        {
            ViewData["permisos"] = _principal;
            return View();
        }


        public async Task<ActionResult> pvwInsuredAssets()
        {
            InsuredAssets insuredAssets = new InsuredAssets();
            //insuredAssets.InsurancePolicyId = Mod.InsurancePolicyId;

            return PartialView(insuredAssets);
        }

        public  ActionResult InsuranceEndorsement()
        {
            InsuranceEndorsement insuranceEndorsement = new InsuranceEndorsement();
            //insuranceEndorsement.InsurancePolicyId = insurancePolicy.InsurancePolicyId;

            return PartialView(insuranceEndorsement);
        }
        
        public ActionResult SFActivosAsegurados(InsurancePolicy insurancePolicy)
        {
            
            return  View(insurancePolicy);
        }


        [HttpGet]
        public async Task<ActionResult> SFInsuranceEndorsement(InsurancePolicy insurancePolicy)
        {
            try
            {
                
                return await Task.Run(() => View(insurancePolicy));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }

        public async Task<ActionResult> pvwAddInsuredAssets([FromBody] InsuredAssets _InsuredAssetsp) {
            
            

            InsuredAssets _InsuredAssets = new InsuredAssets();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuredAssets/GetInsuredAssetsById/" + _InsuredAssetsp.Id);
                string valorrespuesta = "";
                _InsuredAssets.FechaCreacion = DateTime.Now;
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);

                }

                if (_InsuredAssets == null)
                {
                    _InsuredAssets = new InsuredAssets();
                    _InsuredAssets.FechaCreacion = DateTime.Now;
                }
                else
                {
                    // _InsuredAssets.NumeroDEIString = $"{_InsuredAssets.Sucursal}-{_InsuredAssets.Caja}-01-{_InsuredAssets.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InsuredAssets);

        }

        public async Task<ActionResult> pvwAddInsuranceEndorsement([FromBody] InsuranceEndorsement _InsuranceEndorsementp)
        {
            InsuranceEndorsement _InsuranceEndorsement = new InsuranceEndorsement();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsement/GetInsuranceEndorsementById/" + _InsuranceEndorsementp.InsuranceEndorsementId);
                string valorrespuesta = "";
                _InsuranceEndorsement.DateGenerated = DateTime.Now;
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<InsuranceEndorsement>(valorrespuesta);

                }

                if (_InsuranceEndorsement == null)
                {
                    _InsuranceEndorsement = new InsuranceEndorsement();
                    _InsuranceEndorsement.DateGenerated = DateTime.Now;
                }
                else
                {
                    // _InsuranceEndorsement.NumeroDEIString = $"{_InsuranceEndorsement.Sucursal}-{_InsuranceEndorsement.Caja}-01-{_InsuranceEndorsement.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InsuranceEndorsement);

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


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetInsuracesPoliciesByInsuranceId([DataSourceRequest]DataSourceRequest request,int InsuranceId)
        {
            List<InsurancePolicy> _InsurancePolicy = new List<InsurancePolicy>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancePolicy/GetInsurancePolicyByInsuranceId/"+InsuranceId);
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




        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InsurancePolicy>> SaveInsurancePolicy(IEnumerable<IFormFile> files, InsurancePolicy _InsurancePolicyS)
        {
            //InsurancePolicy _InsurancePolicy = _InsurancePolicyS;
            if (_InsurancePolicyS.DollarAmount > 0 && _InsurancePolicyS.LpsAmount == 0)
            {
                return BadRequest("El valor en lempiras es requerido cuando el valor de la poliza es en dolares");
            }
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
                
                if (_listInsurancePolicy.InsurancePolicyId == 0)
                {
                    _InsurancePolicyS.FechaCreacion = DateTime.Now;
                    _InsurancePolicyS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsurancePolicyS);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return await Task.Run(() => BadRequest(((BadRequestObjectResult)insertresult.Result).Value));
                    }
                    
                    var value = (insertresult.Result as ObjectResult).Value;
                    _InsurancePolicyS = ((InsurancePolicy)(value));
                }
                else
                {
                    _InsurancePolicyS.FechaCreacion = _listInsurancePolicy.FechaCreacion;
                    _InsurancePolicyS.UsuarioCreacion = _listInsurancePolicy.UsuarioCreacion;
                    var updateresult = await Update( _InsurancePolicyS);
                }
                if (file != null)
                {

                    FileInfo info = new FileInfo(file.FileName);
                    var filePath = _hostingEnvironment.WebRootPath + "\\InsurancePolicy\\Poliza_" + _InsurancePolicyS.InsurancePolicyId + info.Extension;
                    filePath = filePath.Replace("\\", "\\\\");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        // MemoryStream mstream = new MemoryStream();
                        //mstream.WriteTo(stream);
                    }

                    _InsurancePolicyS.AttachmentURL = filePath;
                    _InsurancePolicyS.AttachementFileName = file.FileName;
                    _listInsurancePolicy = _InsurancePolicyS;
                    var updateresult2 = await Update(_InsurancePolicyS);

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
            InsurancePolicy insurancePolicy = new InsurancePolicy();
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
                    insurancePolicy = JsonConvert.DeserializeObject<InsurancePolicy>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    //throw  new Exception(d);
                    return await Task.Run(() => BadRequest($"{d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(insurancePolicy);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocument }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsurancePolicy>> Update( InsurancePolicy _InsurancePolicyDocument)
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