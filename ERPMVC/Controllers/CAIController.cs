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
    public class CAIController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public CAIController(ILogger<CAIController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        // GET: Customer
        public ActionResult Index()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Administracion.CAI.Agregar CAI", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Administracion.CAI.Editar CAI", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Administracion.CAI.Eliminar CAI", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Administracion.CAI.Exportar CAI", "true");
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CAI> _cais = new List<CAI>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAI");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                   _cais  = JsonConvert.DeserializeObject<List<CAI>>(valorrespuesta);
                   _cais = _cais.OrderByDescending(q => q.IdCAI).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


             return Json(_cais.ToDataSourceResult(request));

        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetCAIById([DataSourceRequest]DataSourceRequest request, [FromBody] CAI _CAI)
        {
            CAI _CertificadoDeposito = new CAI();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAIById/" + _CAI.IdCAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                   
                }

                if (_CertificadoDeposito == null)
                {
                    _CertificadoDeposito = new CAI();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CertificadoDeposito);
        }



        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<CAI> _cais = new List<CAI>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAI");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<CAI>>(valorrespuesta);
                    _cais = _cais.OrderByDescending(q => q.IdCAI).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_cais);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCAI([FromBody]CAIDTO _sarpara)
        {
            CAIDTO _CAI = new CAIDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAIById/" + _sarpara.IdCAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAIDTO>(valorrespuesta);

                }

                if (_CAI == null)
                {
                    _CAI = new CAIDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CAI);

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CAI>> SaveCAI([FromBody]CAIDTO _CAIS)
        //public async Task<ActionResult<Product>> SaveProduct([FromBody]ProductDTO _ProductS)
        {

            CAI _CAI = _CAIS;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAIById/" + _CAI.IdCAI);
                string valorrespuesta = "";
                _CAI.FechaModificacion = DateTime.Now;
                _CAI.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAIDTO>(valorrespuesta);
                }

                if (_CAI == null) { _CAI = new Models.CAI(); }
                _CAIS._cai = _CAIS.elcai;
                if (_CAIS.IdCAI == 0)
                {
                    _CAI.FechaCreacion = DateTime.Now;
                    _CAI.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CAIS);
                    var value = (insertresult.Result as ObjectResult).Value;
                    CAI resultado = new CAI();

                    try
                    {
                       resultado = ((CAI)(value));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                        return await Task.Run(() => BadRequest($"No se guardo el formulario, favor revisar: {value.ToString()}{ex.ToString()}"));
                    }
                 
                    if (resultado.IdCAI <= 0)
                    {
                        return await Task.Run(() => BadRequest("No se guardo el formulario, favor revisar"));
                    }

                }
                else
                {
                    _CAIS.UsuarioCreacion = _CAI.UsuarioCreacion;
                    _CAIS.FechaCreacion = _CAI.FechaCreacion;
                    var updateresult = await Update(_CAI.IdCAI, _CAIS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CAI);
        }


        //--------------------------------------------------------------------------------------
        // POST: CAI/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<CAI>> Insert(CAI _CAIp)
        {
            CAI _CAI = _CAIp;
            try
            {                          
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CAI.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CAI.UsuarioModificacion = HttpContext.Session.GetString("user");
                _CAI.FechaCreacion = DateTime.Now;
                _CAI.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Insert", _CAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_CAI);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _CAI }, Total = 1 });
        }
                     
        [HttpPost]
        public async Task<IActionResult> Update( Int64 IdCAI, CAI _CAIp)
        {
            CAI _CAI = _CAIp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CAI.FechaModificacion = DateTime.Now;
                _CAI.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Update", _CAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {                    
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CAI }, Total = 1 });
        }           

        [HttpDelete("{IdCAI}")]
        public async Task<ActionResult<CAI>> Delete(Int64 IdCAI, CAI _CAIp)
        {
            CAI _CAI = _CAIp;
            try   
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Delete", _CAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CAI }, Total = 1 });
        }






    }
}