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
    public class ElementoConfiguracionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ElementoConfiguracionController(ILogger<ElementoConfiguracionController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwElementoConfiguracion(Int64 Id = 0)
        {
            ElementoConfiguracion _ElementoConfiguracion = new ElementoConfiguracion();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);

                }

                if (_ElementoConfiguracion == null)
                {
                    _ElementoConfiguracion = new ElementoConfiguracion();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ElementoConfiguracion);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetElementoByIdConfiguracion([DataSourceRequest]DataSourceRequest request,Int64 Id)
        {
            List<Product> _clientes = new List<Product>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionByIdConfiguracion/"+Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<Product>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_clientes.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ElementoConfiguracion> _ElementoConfiguracion = new List<ElementoConfiguracion>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ElementoConfiguracion.ToDataSourceResult(request);

        }


        public async Task<ActionResult<ElementoConfiguracion>> SaveElementoConfiguracion([FromBody]ElementoConfiguracion _ElementoConfiguracion)
        {

            try
            {
                ElementoConfiguracion _listElementoConfiguracion = new ElementoConfiguracion();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + _ElementoConfiguracion.Id);
                string valorrespuesta = "";
                _ElementoConfiguracion.FechaModificacion = DateTime.Now;
                _ElementoConfiguracion.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

                if (_listElementoConfiguracion.Id == 0)
                {
                    _ElementoConfiguracion.FechaCreacion = DateTime.Now;
                    _ElementoConfiguracion.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ElementoConfiguracion);
                }
                else
                {
                    var updateresult = await Update(_ElementoConfiguracion.Id, _ElementoConfiguracion);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ElementoConfiguracion);
        }

        // POST: ElementoConfiguracion/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ElementoConfiguracion>> Insert(ElementoConfiguracion _ElementoConfiguracion)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ElementoConfiguracion.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ElementoConfiguracion.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ElementoConfiguracion/Insert", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ElementoConfiguracion }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ElementoConfiguracion>> Update(Int64 id, ElementoConfiguracion _ElementoConfiguracion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ElementoConfiguracion/Update", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ElementoConfiguracion }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ElementoConfiguracion>> Delete([FromBody]ElementoConfiguracion _ElementoConfiguracion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ElementoConfiguracion/Delete", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ElementoConfiguracion }, Total = 1 });
        }





    }
}