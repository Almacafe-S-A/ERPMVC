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
    public class GoodsDeliveryAuthorizationController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsDeliveryAuthorizationController(ILogger<GoodsDeliveryAuthorizationController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwGoodsDeliveryAuthorization(Int64 Id = 0)
        {
            GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorizationDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationDTO>(valorrespuesta);

                }

                if (_GoodsDeliveryAuthorization == null)
                {
                    _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorizationDTO { AuthorizationDate = DateTime.Now, DocumentDate = DateTime.Now };
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsDeliveryAuthorization);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsDeliveryAuthorization> _GoodsDeliveryAuthorization = new List<GoodsDeliveryAuthorization>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorization");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorization>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsDeliveryAuthorization.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> SaveGoodsDeliveryAuthorization([FromBody]GoodsDeliveryAuthorization _GoodsDeliveryAuthorization)
        {

            try
            {
                GoodsDeliveryAuthorization _listGoodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationById/" + _GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId);
                string valorrespuesta = "";
                _GoodsDeliveryAuthorization.FechaModificacion = DateTime.Now;
                _GoodsDeliveryAuthorization.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);
                }

                if (_listGoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId == 0)
                {
                    _GoodsDeliveryAuthorization.FechaCreacion = DateTime.Now;
                    _GoodsDeliveryAuthorization.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GoodsDeliveryAuthorization);
                }
                else
                {
                    var updateresult = await Update(_GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId, _GoodsDeliveryAuthorization);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_GoodsDeliveryAuthorization);
        }

        // POST: GoodsDeliveryAuthorization/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> Insert(GoodsDeliveryAuthorization _GoodsDeliveryAuthorization)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsDeliveryAuthorization.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsDeliveryAuthorization.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorization/Insert", _GoodsDeliveryAuthorization);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_GoodsDeliveryAuthorization);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorization }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> Update(Int64 id, GoodsDeliveryAuthorization _GoodsDeliveryAuthorization)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorization/Update", _GoodsDeliveryAuthorization);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorization }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> Delete([FromBody]GoodsDeliveryAuthorization _GoodsDeliveryAuthorization)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorization/Delete", _GoodsDeliveryAuthorization);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorization }, Total = 1 });
        }





    }
}