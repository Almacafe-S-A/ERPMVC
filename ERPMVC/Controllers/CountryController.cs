//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using ERPMVC.Helpers;
//using ERPMVC.Models;
//using ERPMVC.DTO;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;

//namespace ERPMVC.Controllers
//{
//    public class CountryController : Controller
//    {
//        // GET: Country
//        public ActionResult Country()
//        {
//            return View();
//        }

//    }
//}

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
    public class CountryController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public CountryController(ILogger<CountryController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;

        }

        public ActionResult Country()
        {
            return View();
        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Country> _Country = new List<Country>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_Country.ToDataSourceResult(request));

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetCountry([DataSourceRequest]DataSourceRequest request)
        {
            List<Country> _Country = new List<Country>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_Country.ToDataSourceResult(request));

        }


        [HttpPost]
        public async Task<ActionResult<Country>> SaveCountry([FromBody]CountryDTO _CountryS)
        {

            Country _Country = _CountryS;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountryById/" + _CountryS.Id);
                string valorrespuesta = "";
                _Country.FechaModificacion = DateTime.Now;
                _Country.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<CountryDTO>(valorrespuesta);
                }

                if (_Country == null) { _Country = new Models.Country(); }

                if (_CountryS.Id == 0)
                {
                    _Country.FechaCreacion = DateTime.Now;
                    _Country.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CountryS);
                }
                else
                {
                    _CountryS.Usuariocreacion = _Country.Usuariocreacion;
                    _CountryS.FechaCreacion = _Country.FechaCreacion;
                    var updateresult = await Update(_Country.Id, _CountryS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Country);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCountry([FromBody]CountryDTO _sarpara)
        {
            CountryDTO _Country = new CountryDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountryById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<CountryDTO>(valorrespuesta);

                }

                if (_Country == null)
                {
                    _Country = new CountryDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Country);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Country _Countryp)
        {
            Country _Country = _Countryp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Country.Usuariocreacion = HttpContext.Session.GetString("user");
                _Country.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Country.FechaCreacion = DateTime.Now;
                _Country.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Country/Insert", _Country);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<Country>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Country }, Total = 1 });
        }



        [HttpPost]
        public async Task<IActionResult> Update(Int64 Id, Country _Countryp)
        {
            Country _Country = _Countryp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Country.FechaModificacion = DateTime.Now;
                _Country.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Country/Update", _Country);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<Country>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Country }, Total = 1 });
        }


        [HttpDelete("CountryId")]
        public async Task<ActionResult<Country>> Delete(Int64 CountryId, Country _Countryp)
        {
            Country _Country = _Countryp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Country/Delete", _Country);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<Country>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Country }, Total = 1 });
        }


        public async Task<ActionResult> GetCountryP([DataSourceRequest]DataSourceRequest request)
        {
            List<Country> _cais = new List<Country>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_cais.ToDataSourceResult(request));

        }


    }
}