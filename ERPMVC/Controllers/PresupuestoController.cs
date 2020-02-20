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
    public class PresupuestoController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PresupuestoController(ILogger<PresupuestoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        
     public async Task<IActionResult> Presupuesto()
      
        {
            ViewData["Cuentas"] = await ObtenerCuentas();
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPresupuesto([FromBody]PresupuestoDTO _Presupuesto)

        {
            PresupuestoDTO _Presupuestoi = new PresupuestoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Presupuesto/GetPresupuestoById/" + _Presupuesto.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Presupuestoi = JsonConvert.DeserializeObject<PresupuestoDTO>(valorrespuesta);

                }

                if (_Presupuestoi == null)
                {
                    _Presupuestoi = new PresupuestoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Presupuestoi);

        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Presupuesto> _Presupuesto = new List<Presupuesto>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Presupuesto/GetPresupuesto");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Presupuesto = JsonConvert.DeserializeObject<List<Presupuesto>>(valorrespuesta);
                    _Presupuesto = _Presupuesto.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Presupuesto.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<PresupuestoDTO>> SaveProduct([FromBody]PresupuestoDTO _PresupuestoS)
        {
            Presupuesto _Presupuesto = _PresupuestoS;
            try
            {
                Presupuesto _listPresupuesto = new Presupuesto();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Product/GetProductById/" + _Presupuesto.Id);
                string valorrespuesta = "";
                _Presupuesto.FechaModificacion = DateTime.Now;
                _Presupuesto.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Presupuesto = JsonConvert.DeserializeObject<PresupuestoDTO>(valorrespuesta);
                }

                if (_Presupuesto == null) { _Presupuesto = new Models.Presupuesto(); }
                //_ProductS.UnitOfMeasureId = null;
                if (_PresupuestoS.Id == 0)
                {
                    _PresupuestoS.FechaCreacion = DateTime.Now;
                    _PresupuestoS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Presupuesto);
                }
                else
                {
                    _PresupuestoS.UsuarioCreacion = _Presupuesto.UsuarioCreacion;
                    _PresupuestoS.FechaCreacion = _Presupuesto.FechaCreacion;
                    var updateresult = await Update(_Presupuesto.Id, _PresupuestoS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Presupuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Presupuesto>> Insert(Presupuesto _Presupuestop)
        {
            Presupuesto _Presupuesto = _Presupuestop;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Presupuestop.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Presupuestop.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Presupuestop.FechaCreacion = DateTime.Now;
                _Presupuestop.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Presupuesto/Insert", _Presupuesto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Presupuesto = JsonConvert.DeserializeObject<Presupuesto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Presupuesto }, Total = 1 });
        }


        [HttpPut("{ProductId}")]
        public async Task<ActionResult<Presupuesto>> Update(Int64 Id, Presupuesto _Presupuestop)
        {
            Presupuesto _Presupuesto = _Presupuestop;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Presupuesto.FechaModificacion = DateTime.Now;
                _Presupuesto.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Presupuesto/Update", _Presupuesto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Presupuesto = JsonConvert.DeserializeObject<Presupuesto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Presupuesto }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<Presupuesto>> Delete(Int64 ProductId, Presupuesto _Presupuesto)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Presupuesto/Delete", _Presupuesto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Presupuesto = JsonConvert.DeserializeObject<Presupuesto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Presupuesto }, Total = 1 });
        }

        async Task<IEnumerable<Accounting>> ObtenerCuentas()
        {
            IEnumerable<Accounting> cuentas = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    cuentas = JsonConvert.DeserializeObject<IEnumerable<Accounting>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["defaultaccount"] = cuentas.FirstOrDefault();
            return cuentas;

        }


    }
}