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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DependientesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public DependientesController(ILogger<DependientesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Dependientes
        public ActionResult Index()
        {
            return PartialView();
        }


        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Dependientes> _Dependientes = new List<Dependientes>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dependientes/GetDependientes");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<List<Dependientes>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Dependientes.ToDataSourceResult(request));

        }

        //--------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public async Task<JsonResult> GetDependientesByEmployeeId([DataSourceRequest]DataSourceRequest request,Dependientes _EmployeeP)
        {
            List<Dependientes> _Dependientes = new List<Dependientes>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dependientes/GetDependientesByEmployeeId/" + _EmployeeP.IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<List<Dependientes>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Dependientes.ToDataSourceResult(request));

        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<Dependientes> _Dependientes = new List<Dependientes>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dependientes/GetDependientes");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<List<Dependientes>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Dependientes);

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddDependientes([FromBody]Dependientes _Dependientesp)
        {
            Dependientes _Dependientes = new Dependientes();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dependientes/GetDependientesById/" + _Dependientesp.IdDependientes);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<Dependientes>(valorrespuesta);

                }

                if (_Dependientes == null)
                {
                    _Dependientes = new Dependientes { IdDependientes = _Dependientesp.IdDependientes };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Dependientes);

        }
        
        [HttpPost]
        public async Task<ActionResult<Dependientes>> SaveDependientes([FromBody]DependientesDTO _DependientesS)
        {

            Dependientes _Dependientes = _DependientesS;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dependientes/GetDependientes/" + _Dependientes.IdDependientes);
                string valorrespuesta = "";
                _Dependientes.FechaModificacion = DateTime.Now;
                _Dependientes.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<DependientesDTO>(valorrespuesta);
                }

                if (_Dependientes == null) { _Dependientes = new Models.Dependientes(); }

                if (_DependientesS.IdDependientes == 0)
                {
                    _Dependientes.FechaCreacion = DateTime.Now;
                    _Dependientes.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DependientesS);
                }
                else
                {
                    _DependientesS.Usuariocreacion = _Dependientes.Usuariocreacion;
                    _DependientesS.FechaCreacion = _Dependientes.FechaCreacion;
                    var updateresult = await Update(_Dependientes.IdDependientes, _DependientesS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Dependientes);
        }


        //--------------------------------------------------------------------------------------
        // POST: CAI/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Dependientes _DependientesS)
        {
            Dependientes _Dependientes = _DependientesS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Dependientes.Usuariocreacion = HttpContext.Session.GetString("user");
                _Dependientes.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Dependientes.FechaCreacion = DateTime.Now;
                _Dependientes.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Dependientes/Insert", _Dependientes);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<Dependientes>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Dependientes }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 IdDependientes, Dependientes _Dependientesp)
        {
            Dependientes _Dependientes = _Dependientesp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Dependientes.FechaModificacion = DateTime.Now;
                _Dependientes.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Dependientes/Update", _Dependientes);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<Dependientes>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Dependientes }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<Dependientes>> Delete(Int64 Id, Dependientes _Departamentop)
        {
            Dependientes _Dependientes = _Departamentop;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Dependientes/Delete", _Dependientes);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dependientes = JsonConvert.DeserializeObject<Dependientes>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Dependientes }, Total = 1 });
        }



    }
}