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
    public class BlackListCustomersController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public BlackListCustomersController(ILogger<BlackListCustomersController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult BlackListCustomers()
        {
            return View();
        }


        public IActionResult BlackListFind()
        {
            return PartialView();
        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<BlackListCustomers> _BlackListCustomers = new List<BlackListCustomers>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BlackListCustomers/GetBlackListCustomers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<List<BlackListCustomers>>(valorrespuesta);
                    _BlackListCustomers = _BlackListCustomers.OrderByDescending(q => q.BlackListId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _BlackListCustomers.ToDataSourceResult(request);

        }

        [HttpPost]
        public async Task<ActionResult> GetBlackListByRTN([FromBody]BlackListCustomers _BlackListp)
        {
            BlackListCustomers _BlackList = new BlackListCustomers();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/BlackListCustomers/GetBlackListByRTN", _BlackListp);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackList = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);
                }
                if (_BlackList == null)
                {
                    _BlackList = new BlackListCustomers();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_BlackList);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetBlackListByParams([DataSourceRequest]DataSourceRequest request, BlackListCustomers BlackListCustomersP)
        {
            List<BlackListCustomers> _BlackListCustomers = new List<BlackListCustomers>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/BlackListCustomers/GetBlackListByParams", BlackListCustomersP);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<List<BlackListCustomers>>(valorrespuesta);
                    _BlackListCustomers = _BlackListCustomers.OrderByDescending(q => q.BlackListId).ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _BlackListCustomers.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddBlackList([FromBody]BlackListCustomersDTO _sar)
        {
            BlackListCustomersDTO _BlackListCustomers = new BlackListCustomersDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BlackListCustomers/GetBlackListCustomersById/" + _sar.BlackListId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomersDTO>(valorrespuesta);

                }

                if (_BlackListCustomers == null)
                {
                    _BlackListCustomers = new BlackListCustomersDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_BlackListCustomers);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<BlackListCustomers>> SaveBlackListCustomers([FromBody]BlackListCustomersDTO _BlackListCustomersP)
        {
            BlackListCustomers _BlackListCustomers = _BlackListCustomersP;
            try
            {
                BlackListCustomers _listBlackListCustomers = new BlackListCustomers();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BlackListCustomers/GetBlackListCustomersById/" + _BlackListCustomers.BlackListId);
                string valorrespuesta = "";
                _BlackListCustomers.FechaModificacion = DateTime.Now;
                _BlackListCustomers.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);
                }

                if (_BlackListCustomers == null) { _BlackListCustomers = new Models.BlackListCustomers(); }

                if (_BlackListCustomersP.BlackListId == 0)
                {
                    _BlackListCustomersP.FechaCreacion = DateTime.Now;
                    _BlackListCustomersP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BlackListCustomersP);
                }
                else
                {
                    _BlackListCustomersP.UsuarioCreacion = _BlackListCustomers.UsuarioCreacion;
                    _BlackListCustomersP.FechaCreacion = _BlackListCustomers.FechaCreacion;
                    var updateresult = await Update(_BlackListCustomersP.BlackListId, _BlackListCustomersP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_BlackListCustomers);
        }

        // POST: BlackListCustomers/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<BlackListCustomers>> Insert(BlackListCustomers _BlackListCustomers)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _BlackListCustomers.UsuarioCreacion = HttpContext.Session.GetString("user");
                _BlackListCustomers.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/BlackListCustomers/Insert", _BlackListCustomers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_BlackListCustomers);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _BlackListCustomers }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BlackListCustomers>> Update(Int64 id, BlackListCustomers _BlackListCustomers)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/BlackListCustomers/Update", _BlackListCustomers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _BlackListCustomers }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<BlackListCustomers>> Delete(Int64 BlackListId, BlackListCustomers _BlackListCustomers)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/BlackListCustomers/Delete", _BlackListCustomers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _BlackListCustomers }, Total = 1 });
        }





    }
}