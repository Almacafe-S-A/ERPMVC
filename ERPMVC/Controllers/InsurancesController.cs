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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
     [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class InsurancesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public InsurancesController(ILogger<InsurancesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public ActionResult Insurances()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetInsurances([DataSourceRequest]DataSourceRequest request)
        {
            List<Insurances> _Insurances = new List<Insurances>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurances" );
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<List<Insurances>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_Insurances.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult> Insert(Insurances _Insurances)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Insurances.CreatedUser = HttpContext.Session.GetString("user");
                _Insurances.CreatedDate = DateTime.Now;
                _Insurances.ModifiedUser = HttpContext.Session.GetString("user");
                _Insurances.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Insurances/Insert", _Insurances);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Insurances }, Total = 1 });
        }


        [HttpPut("InsurancesId")]
        public async Task<IActionResult> Update(Int64 InsurancesId, Insurances _Insurances)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Insurances/Update", _Insurances);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Insurances }, Total = 1 });
        }

        public async Task<ActionResult<Insurances>> SaveInsurances([FromBody]InsurancesDTO _InsurancesP)
        {
            Insurances _Insurances = _InsurancesP;
            try
            {
                //JournalEntry _listItems = new JournalEntry();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _Insurances.InsurancesId);
                string valorrespuesta = "";
                _Insurances.ModifiedDate = DateTime.Now;
                _Insurances.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

                if (_Insurances == null) { _Insurances = new Models.Insurances(); }

                if (_InsurancesP.InsurancesId == 0)
                {
                    _Insurances.CreatedDate = DateTime.Now;
                    _Insurances.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsurancesP);
                }
                else
                {
                    _InsurancesP.CreatedUser = _Insurances.CreatedUser;
                    _InsurancesP.CreatedDate = _Insurances.CreatedDate;
                    var updateresult = await Update(_Insurances.InsurancesId, _InsurancesP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InsurancesP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddInsurances([FromBody]InsurancesDTO _sarpara)
        {
            InsurancesDTO _Insurances = new InsurancesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _sarpara.InsurancesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<InsurancesDTO>(valorrespuesta);

                }

                if (_Insurances == null)
                {
                    _Insurances = new InsurancesDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Insurances);

        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetInsurancesById(Int64 InsurancesId)
        {
            Insurances _Insurances = new Insurances();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + InsurancesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_Insurances));
        }

    }
}