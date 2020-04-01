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
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class PresupuestoController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public PresupuestoController(ILogger<PresupuestoController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        
     public async Task<IActionResult> Presupuesto()
      
        {
            ViewData["permisos"] = _principal;
            ViewData["Cuentas"] = await ObtenerCuentas();
            Presupuesto presupuesto = new Presupuesto();
            
            return View(presupuesto);
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

        [HttpPost("savepresupuesto")]
        public async Task<ActionResult<Presupuesto>> savepresupuesto(PresupuestoDTO _PresupuestoS)
        {
            try
            {
               
                if(_PresupuestoS.AccountigId == 0)
                {



                    //return await Task.Run(() => BadRequest("Seleccione una cuenta"));
                    return await Task.Run(() => BadRequest($"Por favor seleccione una cuenta."));

                }

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PresupuestoS.FechaCreacion = DateTime.Now;
                _PresupuestoS.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PresupuestoS.FechaModificacion = DateTime.Now;
                _PresupuestoS.UsuarioModificacion = HttpContext.Session.GetString("user");
                //_PresupuestoS.AccountigId = _PresupuestoS.Accounting.AccountId;
                //_PresupuestoS.AccountName = _PresupuestoS.Accounting.AccountName;
                //_PresupuestoS.Accounting = null;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Presupuesto/Insert", _PresupuestoS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PresupuestoS = JsonConvert.DeserializeObject<PresupuestoDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PresupuestoS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Presupuesto>> Insert(Presupuesto _Presupuestop)
        {
            Presupuesto _Presupuesto = _Presupuestop;
            try
            {
                if (_Presupuesto.AccountName == "")
                {
                    return await Task.Run(() => BadRequest($"Ya éxiste una bodega registrada con este nombre esta Sucursal."));
                }


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


        [HttpPost("Update")]
        public async Task<ActionResult<Presupuesto>> Update(/*Int64 Id,*/ PresupuestoDTO _PresupuestoS)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _PresupuestoS.FechaCreacion = DateTime.Now;
                _PresupuestoS.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PresupuestoS.FechaModificacion = DateTime.Now;
                _PresupuestoS.UsuarioModificacion = HttpContext.Session.GetString("user");
                //_PresupuestoS.AccountigId = _PresupuestoS.Accounting.AccountId;
                //_PresupuestoS.AccountName = _PresupuestoS.Accounting.AccountName;
                //_PresupuestoS.Accounting = null;
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Presupuesto/Update", _PresupuestoS);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PresupuestoS = JsonConvert.DeserializeObject<PresupuestoDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PresupuestoS }, Total = 1 });

        }

        //[HttpPost("Delete")]
        [AcceptVerbs("Post")]
        public async Task<ActionResult<Presupuesto>> Delete(Presupuesto _Presupuesto)
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
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountstartwith5y6");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    cuentas = JsonConvert.DeserializeObject<IEnumerable<Accounting>>(valorrespuesta);

                    cuentas = cuentas.Select(c => new Accounting
                    {
                        AccountId = c.AccountId,
                        AccountName = c.AccountCode + "--" + c.AccountName,
                        AccountCode = c.AccountCode,
                        Description = c.Description,
                        Estado = c.Estado,
                        IdEstado = c.IdEstado,
                    }).ToList();

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