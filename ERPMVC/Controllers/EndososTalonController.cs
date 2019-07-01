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
    public class EndososTalonController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EndososTalonController(ILogger<EndososTalonController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwEndososTalon(Int64 Id = 0)
        {
            EndososTalon _EndososTalon = new EndososTalon();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososTalon/GetEndososTalonById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalon = JsonConvert.DeserializeObject<EndososTalon>(valorrespuesta);

                }

                if (_EndososTalon == null)
                {
                    _EndososTalon = new EndososTalon();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EndososTalon);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EndososTalon> _EndososTalon = new List<EndososTalon>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososTalon/GetEndososTalon");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalon = JsonConvert.DeserializeObject<List<EndososTalon>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososTalon.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososTalon>> SaveEndososTalon([FromBody]EndososTalon _EndososTalon)
        {

            try
            {
                EndososTalon _listEndososTalon = new EndososTalon();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososTalon/GetEndososTalonById/" + _EndososTalon.EndososTalonId);
                string valorrespuesta = "";
                _EndososTalon.FechaModificacion = DateTime.Now;
                _EndososTalon.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEndososTalon = JsonConvert.DeserializeObject<EndososTalon>(valorrespuesta);
                }

                if (_listEndososTalon.EndososTalonId == 0)
                {
                    _EndososTalon.FechaCreacion = DateTime.Now;
                    _EndososTalon.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EndososTalon);
                }
                else
                {
                    var updateresult = await Update(_EndososTalon.EndososTalonId, _EndososTalon);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EndososTalon);
        }

        // POST: EndososTalon/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EndososTalon>> Insert(EndososTalon _EndososTalon)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EndososTalon.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EndososTalon.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososTalon/Insert", _EndososTalon);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalon = JsonConvert.DeserializeObject<EndososTalon>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EndososTalon);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososTalon }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndososTalon>> Update(Int64 id, EndososTalon _EndososTalon)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EndososTalon/Update", _EndososTalon);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalon = JsonConvert.DeserializeObject<EndososTalon>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososTalon }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososTalon>> Delete([FromBody]EndososTalon _EndososTalon)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososTalon/Delete", _EndososTalon);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalon = JsonConvert.DeserializeObject<EndososTalon>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososTalon }, Total = 1 });
        }





    }
}