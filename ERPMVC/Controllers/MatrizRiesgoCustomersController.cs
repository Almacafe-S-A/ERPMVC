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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class MatrizRiesgoCustomersController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public MatrizRiesgoCustomersController(ILogger<MatrizRiesgoCustomersController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Policy = "Monitoreo.Matriz de Riesgos Clientes")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<DataSourceResult> GetMatrizRiesgoCustomers([DataSourceRequest]DataSourceRequest request)
        {
            List<MatrizRiesgoCustomers> _MatrizRiesgoCustomers = new List<MatrizRiesgoCustomers>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/MatrizRiesgoCustomers/GetMatrizRiesgoCustomers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MatrizRiesgoCustomers = JsonConvert.DeserializeObject<List<MatrizRiesgoCustomers>>(valorrespuesta);
                }
                foreach (var item in _MatrizRiesgoCustomers)
                {
                    item.Customer.CustomerName = item.Customer.CustomerName + ", " + item.BranchName;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _MatrizRiesgoCustomers.ToDataSourceResult(request);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddMatrizRiesgoCustomers([FromBody]MatrizRiesgoCustomersDTO _sarpara)
        {
            MatrizRiesgoCustomersDTO _MatrizRiesgoCustomers = new MatrizRiesgoCustomersDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/MatrizRiesgoCustomers/GetMatrizRiesgoCustomersById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MatrizRiesgoCustomers = JsonConvert.DeserializeObject<MatrizRiesgoCustomersDTO>(valorrespuesta);
                }

                if (_MatrizRiesgoCustomers == null)
                {
                    _MatrizRiesgoCustomers = new MatrizRiesgoCustomersDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_MatrizRiesgoCustomers));
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<MatrizRiesgoCustomers>> SaveMatrizRiesgoCustomers([FromBody]MatrizRiesgoCustomersDTO _MatrizRiesgoCustomersP)
        {
            MatrizRiesgoCustomers _MatrizRiesgoCustomers = _MatrizRiesgoCustomersP;
            List<SeveridadRiesgo> _SeveridadRiesgo = new List<SeveridadRiesgo>();
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/MatrizRiesgoCustomers/GetMatrizRiesgoCustomersById/" + _MatrizRiesgoCustomers.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MatrizRiesgoCustomers = JsonConvert.DeserializeObject<MatrizRiesgoCustomersDTO>(valorrespuesta);
                }

                if (_MatrizRiesgoCustomers == null) { _MatrizRiesgoCustomers = new Models.MatrizRiesgoCustomers(); }

                var resultSeveridadRiesgo = await _client.GetAsync(baseadress + "api/SeveridadRiesgoes/GetSeveridadRiesgo");
                string valorrespuestaSeveridadRiesgo = "";
                if (resultSeveridadRiesgo.IsSuccessStatusCode)
                {
                    valorrespuestaSeveridadRiesgo = await (resultSeveridadRiesgo.Content.ReadAsStringAsync());
                    _SeveridadRiesgo = JsonConvert.DeserializeObject<List<SeveridadRiesgo>>(valorrespuestaSeveridadRiesgo);
                }

                foreach (var item in _SeveridadRiesgo)
                {
                    if(item.RangoInferiorSeveridad <= _MatrizRiesgoCustomersP.RiesgoInicialValorSeveridad 
                        && item.RangoSuperiorSeveridad >= _MatrizRiesgoCustomersP.RiesgoInicialValorSeveridad)
                    {
                        _MatrizRiesgoCustomersP.RiesgoInicialNivel = item.Nivel;
                        _MatrizRiesgoCustomersP.RiesgoInicialColorHexadecimal = item.ColorHexadecimal;
                    }

                    if (item.RangoInferiorSeveridad <= _MatrizRiesgoCustomersP.RiesgoResidualValorSeveridad
                        && item.RangoSuperiorSeveridad >= _MatrizRiesgoCustomersP.RiesgoResidualValorSeveridad)
                    {
                        _MatrizRiesgoCustomersP.RiesgoResidualNivel = item.Nivel;
                        _MatrizRiesgoCustomersP.RiesgoResidualColorHexadecimal = item.ColorHexadecimal;
                    }
                }

                if (_MatrizRiesgoCustomersP.Id == 0)
                {
                    _MatrizRiesgoCustomersP.FechaCreacion = DateTime.Now;
                    _MatrizRiesgoCustomersP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _MatrizRiesgoCustomersP.FechaModificacion = DateTime.Now;
                    _MatrizRiesgoCustomersP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_MatrizRiesgoCustomersP);
                }
                else
                {
                    _MatrizRiesgoCustomersP.FechaCreacion = _MatrizRiesgoCustomers.FechaCreacion;
                    _MatrizRiesgoCustomersP.UsuarioCreacion = _MatrizRiesgoCustomers.UsuarioCreacion;
                    _MatrizRiesgoCustomersP.FechaModificacion = DateTime.Now;
                    _MatrizRiesgoCustomersP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_MatrizRiesgoCustomers.Id, _MatrizRiesgoCustomersP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_MatrizRiesgoCustomers);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(MatrizRiesgoCustomers _MatrizRiesgoCustomersS)
        {
            MatrizRiesgoCustomers _MatrizRiesgoCustomers = _MatrizRiesgoCustomersS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/MatrizRiesgoCustomers/Insert", _MatrizRiesgoCustomersS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MatrizRiesgoCustomersS = JsonConvert.DeserializeObject<MatrizRiesgoCustomers>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _MatrizRiesgoCustomersS }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 IdMatrizRiesgoCustomers, MatrizRiesgoCustomers _MatrizRiesgoCustomersP)
        {
            MatrizRiesgoCustomers _MatrizRiesgoCustomers = _MatrizRiesgoCustomersP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/MatrizRiesgoCustomers/Update", _MatrizRiesgoCustomers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MatrizRiesgoCustomers = JsonConvert.DeserializeObject<MatrizRiesgoCustomers>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _MatrizRiesgoCustomers }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<MatrizRiesgoCustomers>> Delete(Int64 Id, [FromBody]MatrizRiesgoCustomers _MatrizRiesgoCustomersP)
        {
            MatrizRiesgoCustomers _MatrizRiesgoCustomers = _MatrizRiesgoCustomersP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/MatrizRiesgoCustomers/Delete", _MatrizRiesgoCustomers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MatrizRiesgoCustomers = JsonConvert.DeserializeObject<MatrizRiesgoCustomers>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _MatrizRiesgoCustomers }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        public async Task<IActionResult> SFResumenRiesgosClientes()
        {
            return await Task.Run(() => View());
        }
    }
}