using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
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
    public class TaxController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public TaxController(ILogger<TaxController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Customer
        public ActionResult Tax()
        {
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetTaxes([DataSourceRequest]DataSourceRequest request)
        {
            List<Tax> _Taxes = new List<Tax>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTax");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Taxes = JsonConvert.DeserializeObject<List<Tax>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Taxes.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Tax> _tax = new List<Tax>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTax");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _tax = JsonConvert.DeserializeObject<List<Tax>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_tax.ToDataSourceResult(request));

        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<Tax>> GetTaxById([DataSourceRequest]DataSourceRequest request, Int64 TaxId)
        {
            Tax _Taxes = new Tax();
            try
            {


                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTaxById/" + TaxId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Taxes = JsonConvert.DeserializeObject<Tax>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Taxes;

        }


        //--------------------------------------------------------------------------------------
        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<Tax> _Tax = new List<Tax>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTax");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Tax = JsonConvert.DeserializeObject<List<Tax>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Tax);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddTax([FromBody]TaxDTO _sarpara)
        {
            TaxDTO _Tax = new TaxDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTaxById/" + _sarpara.TaxId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Tax = JsonConvert.DeserializeObject<TaxDTO>(valorrespuesta);

                }

                if (_Tax == null)
                {
                    _Tax = new TaxDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Tax);

        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwEditTax([FromBody]TaxDTO _sarpara)
        {
            TaxDTO _Tax = new TaxDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTaxById/" + _sarpara.TaxId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Tax = JsonConvert.DeserializeObject<TaxDTO>(valorrespuesta);

                }

              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Tax);

        }

        [HttpPost]
        public async Task<ActionResult<Tax>> SaveTax([FromBody]TaxDTO _TaxP)
        {

            Tax _Tax = _TaxP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTaxById/" + _Tax.TaxId);
                string valorrespuesta = "";
                _Tax.FechaModificacion = DateTime.Now;
                _Tax.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Tax = JsonConvert.DeserializeObject<TaxDTO>(valorrespuesta);
                }

                if (_Tax == null) { _Tax = new Models.Tax(); }

                if (_TaxP.TaxId == 0)
                {
                    _Tax.FechaCreacion = DateTime.Now;
                    _Tax.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TaxP);
                }
                else
                {
                    _TaxP.UsuarioCreacion = _Tax.UsuarioCreacion;
                    _TaxP.FechaCreacion = _Tax.FechaCreacion;
                    var updateresult = await Update(_Tax.TaxId, _TaxP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Tax);
        }


        //--------------------------------------------------------------------------------------
        // POST: Tax/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Tax _TaxP)
        {
            Tax _Tax = _TaxP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Tax.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Tax.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Tax.FechaCreacion = DateTime.Now;
                _Tax.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Tax/Insert", _Tax);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Tax = JsonConvert.DeserializeObject<Tax>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Tax }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, Tax _TaxP)
        {
            Tax _Tax = _TaxP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Tax.FechaModificacion = DateTime.Now;
                _Tax.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Tax/Update", _Tax);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Tax = JsonConvert.DeserializeObject<Tax>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Tax }, Total = 1 });
        }



    }
}