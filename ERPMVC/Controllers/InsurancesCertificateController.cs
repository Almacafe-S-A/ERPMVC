using System;
using System.Collections.Generic;
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
   /* public class InsurancesCertificateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }*/
    [Authorize]
    [CustomAuthorization]
    public class InsurancesCertificateController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        public InsurancesCertificateController(IHostingEnvironment hostingEnvironment
            , ILogger<InsurancesCertificateController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> InsurancesCertificate()
        {
            ViewData["Insurances"] = await ObtenerInsurances();

            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddInsurancesCertificate([FromBody]InsurancesCertificateDTO _InsurancesCertificatep)
        {
            InsurancesCertificateDTO _InsurancesCertificate = new InsurancesCertificateDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificate/GetInsurancesCertificateById/" + _InsurancesCertificatep.InsurancesCertificateId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificate = JsonConvert.DeserializeObject<InsurancesCertificateDTO>(valorrespuesta);

                }

                if (_InsurancesCertificate == null)
                {
                    _InsurancesCertificate = new InsurancesCertificateDTO();
                    _InsurancesCertificate.DateofInsurance = DateTime.Now;
                    _InsurancesCertificate.BeginDateofInsurance = DateTime.Now;
                    _InsurancesCertificate.EndDateofInsurance = DateTime.Now;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            //
            return PartialView(_InsurancesCertificate);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InsurancesCertificate> _InsurancesCertificate = new List<InsurancesCertificate>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificate/GetInsurancesCertificate");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificate = JsonConvert.DeserializeObject<List<InsurancesCertificate>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsurancesCertificate.ToDataSourceResult(request);

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

        public async Task<ActionResult<InsurancesCertificate>> SaveInsurancesCertificate([FromBody]InsurancesCertificateDTO _InsurancesCertificateP)
        {
           InsurancesCertificate _InsurancesCertificate = _InsurancesCertificateP;
            try
            {
                InsurancesCertificate _listInsurancesCertificate = new InsurancesCertificate();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificate/GetInsurancesCertificateById/" + _InsurancesCertificateP.InsurancesCertificateId);

                string valorrespuesta = "";
                _InsurancesCertificate.ModifiedDate = DateTime.Now;
                _InsurancesCertificate.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {



                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificate = JsonConvert.DeserializeObject<InsurancesCertificate>(valorrespuesta);
                }

                if (_InsurancesCertificate == null) { _InsurancesCertificate = new Models.InsurancesCertificate(); }

                if (_InsurancesCertificateP.InsurancesCertificateId == 0)
                {
                    InsurancesCertificateDTO _InsurancesCertificateDuplicated = new InsurancesCertificateDTO();
                    // string baseadress = config.Value.urlbase;
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado = await _client.GetAsync(baseadress + "api/Accounting/GetInsurancesCertificateByBeginDate/" + _InsurancesCertificateP.BeginDateofInsurance);
                    string valorrespuesta2 = "";

                    if (resultado.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                        _InsurancesCertificateDuplicated = JsonConvert.DeserializeObject<InsurancesCertificateDTO>(valorrespuesta2);

                    }
                    if (_InsurancesCertificateDuplicated != null)
                    {
                        if (_InsurancesCertificateDuplicated.InsurancesCertificateId != _InsurancesCertificateP.InsurancesCertificateId)
                        {
                            string error = await result.Content.ReadAsStringAsync();
                            return await Task.Run(() => BadRequest($"La fecha de incio de poliza ya esta ingresado..."));
                        }
                        /* return this.Json(new DataSourceResult
                         {
                             Errors = $"Ocurrio un error:{error} El codigo de cuenta ya esta ingresado."

                         });*/
                    }

                    _InsurancesCertificateP.CreatedDate = DateTime.Now;
                    _InsurancesCertificateP.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsurancesCertificateP);
                }
                else
                {
                    InsurancesCertificateDTO _InsurancesCertificateDuplicated = new InsurancesCertificateDTO();
                    // string baseadress = config.Value.urlbase;
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado = await _client.GetAsync(baseadress + "api/Accounting/GetInsurancesCertificateByBeginDate/" + _InsurancesCertificateP);
                    string valorrespuesta2 = "";

                    if (resultado.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                        _InsurancesCertificateDuplicated = JsonConvert.DeserializeObject<InsurancesCertificateDTO>(valorrespuesta2);

                    }
                    if (_InsurancesCertificateDuplicated != null)
                    {
                        if (_InsurancesCertificateDuplicated.InsurancesCertificateId != _InsurancesCertificateP.InsurancesCertificateId)
                        {
                            string error = await result.Content.ReadAsStringAsync();
                            return await Task.Run(() => BadRequest($"La poliza ya esta ingresado..."));
                        }

                        
                        /*    return this.Json(new DataSourceResult
                            {
                                Errors = $"Ocurrio un error:{error} El codigo de cuenta ya esta ingresado."

                            });*/
                    }

                    _InsurancesCertificateP.CreatedUser = _InsurancesCertificate.CreatedUser;
                    _InsurancesCertificateP.CreatedDate = _InsurancesCertificate.CreatedDate;
                    var updateresult = await Update(_InsurancesCertificate.InsurancesCertificateId, _InsurancesCertificateP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesCertificateP }, Total = 1 });

        }



        // POST: CustomerDocument/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsurancesCertificateDTO>> Insert(InsurancesCertificateDTO _InsurancesCertificateDTO)
        {
            InsurancesCertificateDTO _InsurancesCertificate = new InsurancesCertificateDTO();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsurancesCertificateDTO.CreatedUser = HttpContext.Session.GetString("user");
                _InsurancesCertificateDTO.ModifiedUser = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InsurancesCertificate/Insert", _InsurancesCertificateDTO);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificate = JsonConvert.DeserializeObject<InsurancesCertificateDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_InsurancesCertificate);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocument }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsurancesCertificateDTO>> Update(Int64 id, InsurancesCertificateDTO _InsurancesCertificateD)
        {
            InsurancesCertificateDTO _InsurancesCertificateDTO = new InsurancesCertificateDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InsurancesCertificate/Update", _InsurancesCertificateD);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateDTO = JsonConvert.DeserializeObject<InsurancesCertificateDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesCertificateD }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Insurances>> Delete([FromBody]InsurancesCertificateDTO _InsurancesCertificateDTO)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InsurancesCertificate/Delete", _InsurancesCertificateDTO);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateDTO = JsonConvert.DeserializeObject<InsurancesCertificateDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesCertificateDTO }, Total = 1 });
        }

        async Task<IEnumerable<Insurances>> ObtenerInsurances()
        {
            IEnumerable<Insurances> Insurances = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurances");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    Insurances = JsonConvert.DeserializeObject<IEnumerable<Insurances>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["defaultinsurance"] = Insurances.FirstOrDefault();
            return Insurances;

        }



    }
}