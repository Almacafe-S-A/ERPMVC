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
    public class CityController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public CityController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult City()
        {
            return View();
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<City> _City = new List<City>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/City/GetCity");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<List<City>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _City.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<JsonResult> GetCityJson([DataSourceRequest]DataSourceRequest request,Int64 StateId)
        {
            List<City> _City = new List<City>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/City/GetCity");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<List<City>>(valorrespuesta);
                    _City = _City.Where(q => q.StateId == StateId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_City.ToDataSourceResult(request));

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetByProductType([DataSourceRequest]DataSourceRequest request)
        {
            List<City> _City = new List<City>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/City/GetCity");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<List<City>>(valorrespuesta);
                    _City = _City.Where(q => q.Id == 2).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _City.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<City>> GetCityoByTipo(Int64 ProductTypeId)
        {
            List<City> _Cityo = new List<City>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/City/GetCitybByProductTypeId/" + ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cityo = JsonConvert.DeserializeObject<List<City>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Cityo);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCity([FromBody]CityDTO _sarpara)
            
        {
            CityDTO _City = new CityDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/City/GetCityById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<CityDTO>(valorrespuesta);

                }

                if (_City == null)
                {
                    _City = new CityDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_City);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetProductoById(Int64 CityId)
        {
            City _Citys = new City();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/City/GetCityById/" + CityId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Citys = JsonConvert.DeserializeObject<City>(valorrespuesta);

                }

                if (_Citys == null) { _Citys = new City(); }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_Citys);
        }

        [HttpPost("[controller]/[action]")]
        //public async Task<ActionResult<City>> SaveCity([FromBody]dynamic dto)
        public async Task<ActionResult<City>> SaveCity([FromBody]CityDTO _CityS)
        {

            City _City = _CityS;
            //   City _City = new City();
            //  City _CityS = new City(); //JsonConvert.DeserializeObject<CityDTO>(dto.ToString());
            if (_CityS != null)
            //if (_City != null)
            {

                try
                {
                    //_City = JsonConvert.DeserializeObject<CityDTO>(dto.ToString());
                    City _listProduct = new City();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/City/GetCityById/" + _City.Id);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _City = JsonConvert.DeserializeObject<CityDTO>(valorrespuesta);
                    }

                    if (_City == null) { _City = new Models.City(); }

                    if (_City.Id == 0)
                    {
                        var insertresult = await Insert(_CityS);
                        var value = (insertresult.Result as ObjectResult).Value;
                        _City = ((City)(value));
                        if (_City.Id <= 0)
                        {
                            return await Task.Run(() => BadRequest("Ocurrio un error!"));
                        }
                    }
                    else
                    {
                        var updateresult = await Update(_City.Id, _CityS);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            else
            {
                return BadRequest("No llego correctamente el modelo!");
            }

            return Json(_City);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<City>> Insert(City _Cityp)
        {
            City _City = _Cityp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               
                var result = await _client.PostAsJsonAsync(baseadress + "api/City/Insert", _City);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<City>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _City }, Total = 1 });
        }


        [HttpPut("{CityId}")]
        public async Task<ActionResult<City>> Update(Int64 CityId, City _Cityp)
        {
            City _City = _Cityp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/City/Update", _City);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<City>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _City }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<City>> Delete(Int64 CityId, City _City)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/City/Delete", _City);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _City = JsonConvert.DeserializeObject<City>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _City }, Total = 1 });
        }






    }


}