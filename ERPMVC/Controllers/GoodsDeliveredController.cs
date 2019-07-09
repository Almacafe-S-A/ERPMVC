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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [EnableCors("AllowAllOrigins")]
    public class GoodsDeliveredController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsDeliveredController(ILogger<GoodsDeliveredController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwGoodsDelivered([FromBody]GoodsDeliveredDTO _GoodsDeliveredDTO)
        {
            GoodsDeliveredDTO _GoodsDelivered = new GoodsDeliveredDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredById/" + _GoodsDeliveredDTO.GoodsDeliveredId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDeliveredDTO>(valorrespuesta);

                }

                if (_GoodsDelivered == null)
                {
                    _GoodsDelivered = new GoodsDeliveredDTO { DocumentDate=DateTime.Now, ExpirationDate = DateTime.Now, OrderDate=DateTime.Now, editar=1 };
                }
                else
                {
                    _GoodsDelivered.editar = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsDelivered);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsDelivered> _GoodsDelivered = new List<GoodsDelivered>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDelivered");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<List<GoodsDelivered>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsDelivered.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDelivered>> SaveGoodsDelivered([FromBody]GoodsDelivered _GoodsDelivered)
        {

            try
            {
                GoodsDelivered _listGoodsDelivered = new GoodsDelivered();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredById/" + _GoodsDelivered.GoodsDeliveredId);
                string valorrespuesta = "";
                _GoodsDelivered.FechaModificacion = DateTime.Now;
                _GoodsDelivered.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

                if (_listGoodsDelivered == null) { _listGoodsDelivered = new GoodsDelivered(); }


                if (_listGoodsDelivered.GoodsDeliveredId == 0)
                {
                    _GoodsDelivered.FechaCreacion = DateTime.Now;
                    _GoodsDelivered.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GoodsDelivered);
                }
                else
                {
                    var updateresult = await Update(_GoodsDelivered.GoodsDeliveredId, _GoodsDelivered);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_GoodsDelivered);
        }

        // POST: GoodsDelivered/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsDelivered>> Insert(GoodsDelivered _GoodsDelivered)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsDelivered.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsDelivered.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDelivered/Insert", _GoodsDelivered);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_GoodsDelivered);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDelivered }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsDelivered>> Update(Int64 id, GoodsDelivered _GoodsDelivered)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsDelivered/Update", _GoodsDelivered);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDelivered }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDelivered>> Delete([FromBody]GoodsDelivered _GoodsDelivered)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDelivered/Delete", _GoodsDelivered);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDelivered }, Total = 1 });
        }





    }
}