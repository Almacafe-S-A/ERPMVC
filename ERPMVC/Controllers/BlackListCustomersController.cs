using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class BlackListCustomersController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public BlackListCustomersController(ILogger<BlackListCustomersController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwBlackListCustomers(Int64 Id = 0)
        {
            BlackListCustomers _BlackListCustomers = new BlackListCustomers();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BlackListCustomers/GetBlackListCustomersById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);

                }

                if (_BlackListCustomers == null)
                {
                    _BlackListCustomers = new BlackListCustomers();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_BlackListCustomers);

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
        public async Task<ActionResult<BlackListCustomers>> SaveBlackListCustomers([FromBody]BlackListCustomers _BlackListCustomers)
        {

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
                    _listBlackListCustomers = JsonConvert.DeserializeObject<BlackListCustomers>(valorrespuesta);
                }

                if (_listBlackListCustomers.BlackListId == 0)
                {
                    _BlackListCustomers.FechaCreacion = DateTime.Now;
                    _BlackListCustomers.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BlackListCustomers);
                }
                else
                {
                    var updateresult = await Update(_BlackListCustomers.BlackListId, _BlackListCustomers);
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

        [HttpPost("[action]")]
        public async Task<ActionResult<BlackListCustomers>> Delete([FromBody]BlackListCustomers _BlackListCustomers)
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