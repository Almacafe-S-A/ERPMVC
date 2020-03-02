using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
        private readonly ClaimsPrincipal _principal;
        public GoodsDeliveredController(ILogger<GoodsDeliveredController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
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
                    _GoodsDelivered = new GoodsDeliveredDTO { DocumentDate=DateTime.Now, ExpirationDate = DateTime.Now, OrderDate=DateTime.Now, editar=1
                        ,
                        BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                else
                {
                    _GoodsDelivered.editar = 0;
                }
                ViewData["permisos"] = _principal;
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
        public async Task<ActionResult> GetGoodsDeliveredById([FromBody]GoodsDelivered _GoodsDeliveredp)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            GoodsDelivered _GoodsDelivered = new GoodsDelivered();
            if(_GoodsDeliveredp != null) { 
            try
            {

                //GoodsDelivered _GoodsDeliveredp = JsonConvert.DeserializeObject<GoodsDelivered>(dto);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredById/" + _GoodsDeliveredp.GoodsDeliveredId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);

                }

                if (_GoodsDelivered == null)
                {
                    _GoodsDelivered = new GoodsDelivered();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            }

            return Json(_GoodsDelivered);
        }

        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request)
        {
            var res = await GetGoodsDelivered();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetGoodsDelivered())
                {
                    if (values.Contains(order.GoodsDeliveredId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        [HttpGet("[controller]/[action]")]
        private async Task<List<GoodsDelivered>> GetGoodsDelivered()
        {
            List<GoodsDelivered> _GoodsDelivered = new List<GoodsDelivered>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredNoSelected");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<List<GoodsDelivered>>(valorrespuesta);
                    _GoodsDelivered = (from c in _GoodsDelivered
                                       select new GoodsDelivered
                                       {
                                                  GoodsDeliveredId = c.GoodsDeliveredId,
                                                       CustomerName = "Numero de recibo de entrega: " + c.GoodsDeliveredId + "  ||Nombre:" + c.CustomerName + " ||Fecha: "
                                             +           c.DocumentDate + " ||Fecha de documento:" + c.DocumentDate + " || Boleta de peso:" + c.WeightBallot + " || Cliente:" + c.CustomerName,
                                                       DocumentDate = c.DocumentDate,

                                        }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _GoodsDelivered;
        }



             [HttpPost("[controller]/[action]")]
             public async Task<ActionResult<GoodsDelivered>> SaveGoodsDelivered([FromBody]GoodsDelivered _GoodsDelivered)
            // public async Task<ActionResult<GoodsDelivered>> SaveGoodsDelivered([FromBody]dynamic dto)
        {

            //GoodsDelivered _GoodsDelivered = new GoodsDelivered();
            try
            {
              //  _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(dto.ToString());
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
                    var value = (insertresult.Result as ObjectResult).Value;
                    _GoodsDelivered = ((GoodsDelivered)(value));
                    if (_GoodsDelivered.GoodsDeliveredId == 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero el documento!"));
                    }

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


        [HttpGet]
        public ActionResult SFGoodsDelivered(Int64 id)
        {

            GoodsDeliveredDTO _GoodsDelivered = new GoodsDeliveredDTO { GoodsDeliveredId = id, };

            return View(_GoodsDelivered);
        }




    }
}