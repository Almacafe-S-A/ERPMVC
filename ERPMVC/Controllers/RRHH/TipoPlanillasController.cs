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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class PlanillaTiposController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public PlanillaTiposController(ILogger<PlanillaTiposController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        // GET: Customer
        //[Authorize(Policy = "RRHH.Parametros Tipo de Planilla")]
        public ActionResult PlanillaTipos()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PlanillaTipo> _PlanillaTipos = new List<PlanillaTipo>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanillaTipos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<List<PlanillaTipo>>(valorrespuesta);
                    _PlanillaTipos = _PlanillaTipos.OrderByDescending(p => p.IdTipoPlanilla).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_PlanillaTipos.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<JsonResult> GetPlanillaTipos([DataSourceRequest]DataSourceRequest request)
        {
            List<PlanillaTipo> _PlanillaTipos = new List<PlanillaTipo>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanillaTipos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<List<PlanillaTipo>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_PlanillaTipos.ToDataSourceResult(request));

        }


        [HttpGet]
        public async Task<JsonResult> GetPlanillaTiposActivo([DataSourceRequest] DataSourceRequest request)
        {
            List<PlanillaTipo> _PlanillaTipos = new List<PlanillaTipo>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanillaTipos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<List<PlanillaTipo>>(valorrespuesta);
                    _PlanillaTipos = _PlanillaTipos.Where(e => e.EstadoId ==1).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return Json(_PlanillaTipos.ToDataSourceResult(request));

        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<PlanillaTipo> _PlanillaTipos = new List<PlanillaTipo>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanilla");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<List<PlanillaTipo>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_PlanillaTipos);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPlanillaTipos([FromBody]PlanillaTipo _sarpara)
        {
            PlanillaTipo _PlanillaTipos = new PlanillaTipo();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanillaTiposById/" + _sarpara.IdTipoPlanilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<PlanillaTipo>(valorrespuesta);

                }

                if (_PlanillaTipos == null)
                {
                    _PlanillaTipos = new PlanillaTipo();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_PlanillaTipos);

        }

        
        [HttpPost]
        public async Task<ActionResult<PlanillaTipo>> SavePlanillaTipos([FromBody]PlanillaTipo _PlanillaTiposP)
        {

            PlanillaTipo _PlanillaTipos = _PlanillaTiposP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanillaTiposById/" + _PlanillaTipos.IdTipoPlanilla);
                string valorrespuesta = "";
                _PlanillaTipos.FechaModificacion = DateTime.Now;
                _PlanillaTipos.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<PlanillaTipo>(valorrespuesta);
                }

                if (_PlanillaTipos == null)
                {
                    _PlanillaTipos = new Models.PlanillaTipo();
                }

                if (_PlanillaTiposP.IdTipoPlanilla == 0)
                {
                    _PlanillaTipos.FechaCreacion = DateTime.Now;
                    _PlanillaTipos.Usuariomodificacion = HttpContext.Session.GetString("user");
                    var ValidacionTipoPlanillaresult = await ValidacionPlanillaTipos(_PlanillaTiposP);
                    if ((ValidacionTipoPlanillaresult as ObjectResult).Value.ToString() == "Ya exíste un Tipo de Planilla creada con el mismo Nombre.")
                    {
                        return await Task.Run(() => BadRequest("Ya exíste un Tipo de Planilla creada con el mismo Nombre."));
                    }
                    var insertresult = await Insert(_PlanillaTiposP);
                }
                else
                {
                    _PlanillaTiposP.Usuariocreacion = _PlanillaTipos.Usuariocreacion;
                    _PlanillaTiposP.FechaCreacion = _PlanillaTipos.FechaCreacion;
                    var ValidacionTipoPlanillaresult = await ValidacionPlanillaTipos(_PlanillaTiposP);
                    if ((ValidacionTipoPlanillaresult as ObjectResult).Value.ToString() == "Ya exíste un Tipo de Planilla creada con el mismo Nombre.")
                    {
                        return await Task.Run(() => BadRequest("Ya exíste un Tipo de Planilla creada con el mismo Nombre."));
                    }
                    var updateresult = await Update(_PlanillaTipos.IdTipoPlanilla, _PlanillaTiposP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PlanillaTipos);
        }


        //--------------------------------------------------------------------------------------
        // POST: Planilla/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(PlanillaTipo _PlanillaTiposP)
        {
            PlanillaTipo _PlanillaTipos = _PlanillaTiposP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PlanillaTipos.Estado = null;
                _PlanillaTipos.Categoria = null;
                _PlanillaTipos.Usuariocreacion = HttpContext.Session.GetString("user");
                _PlanillaTipos.Usuariomodificacion = HttpContext.Session.GetString("user");
                _PlanillaTipos.FechaCreacion = DateTime.Now;
                _PlanillaTipos.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/PlanillaTipos/Insert", _PlanillaTipos);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<PlanillaTipo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PlanillaTipos }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, PlanillaTipo _PlanillaTiposP)
        {
            PlanillaTipo _PlanillaTipos = _PlanillaTiposP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PlanillaTipos.Estado = null;
                _PlanillaTipos.Categoria = null;
                _PlanillaTipos.FechaModificacion = DateTime.Now;
                _PlanillaTipos.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/PlanillaTipos/Update", _PlanillaTipos);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<PlanillaTipo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PlanillaTipos }, Total = 1 });
        }


        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<PlanillaTipo>> Delete([FromBody]PlanillaTipo _PlanillaTiposP)
        {
            PlanillaTipo _PlanillaTipos = _PlanillaTiposP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PlanillaTipos/Delete", _PlanillaTipos);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTipos = JsonConvert.DeserializeObject<PlanillaTipo>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _PlanillaTipos }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> ValidacionPlanillaTipos([FromBody]PlanillaTipo _PlanillaTipos)
        {
            List<PlanillaTipo> _PlanillaTiposList = new List<PlanillaTipo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PlanillaTipos/GetPlanillaTipos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PlanillaTiposList = JsonConvert.DeserializeObject<List<PlanillaTipo>>(valorrespuesta);
                    if (_PlanillaTipos.IdTipoPlanilla != 0)
                    {
                        _PlanillaTipos = _PlanillaTiposList.Where(q => q.TipoPlanilla == _PlanillaTipos.TipoPlanilla && q.IdTipoPlanilla != _PlanillaTipos.IdTipoPlanilla).FirstOrDefault();
                        if (_PlanillaTipos != null)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste un Tipo de Planilla creada con el mismo Nombre."));
                        }
                    }
                    else
                    {
                        _PlanillaTipos = _PlanillaTiposList.Where(q => q.TipoPlanilla == _PlanillaTipos.TipoPlanilla).FirstOrDefault();
                        if (_PlanillaTipos != null)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste un Tipo de Planilla creada con el mismo Nombre."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_PlanillaTiposList));
        }

    }
}