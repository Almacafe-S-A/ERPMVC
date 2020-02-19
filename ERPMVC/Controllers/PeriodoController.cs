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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
   
    public class PeriodoController : Controller
    {
       private readonly IOptions<MyConfig> _config;
       private readonly ILogger _logger;

        public PeriodoController(ILogger<PeriodoController> logger, IOptions<MyConfig> config)
        {
            this._config = config;
            this._logger = logger;

        }

        public IActionResult Periodo()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddPeriodo([FromBody]PeriodoDTO _Periodos)
        {

            PeriodoDTO _Periodo = new PeriodoDTO();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Periodo/GetPeriodoById/" + _Periodos.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Periodo = JsonConvert.DeserializeObject<PeriodoDTO>(valorrespuesta);

                }

                if (_Periodo == null)
                {
                    _Periodo = new PeriodoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Periodo);

        }

        /// <summary>
        /// Obitiene el listado de los Periodos!
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Periodo> _Periodo = new List<Periodo>();
            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Periodo/GetPeriodo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Periodo = JsonConvert.DeserializeObject<List<Periodo>>(valorrespuesta);
                    _Periodo = _Periodo.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Periodo.ToDataSourceResult(request);

        }

        [HttpPost]
        public async Task<ActionResult<Periodo>> SavePeriodos([FromBody]PeriodoDTO _PeriodosP)
        {
            Periodo _Periodos = _PeriodosP;
            try
            {
                Periodo _listPeriodo = new Periodo();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Periodo/GetPeriodoById/" + _Periodos.Id);
                string valorrespuesta = "";
                _Periodos.FechaModificacion = DateTime.Now;
                _Periodos.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Periodos = JsonConvert.DeserializeObject<Periodo>(valorrespuesta);
                }

                if (_Periodos == null) { _Periodos = new Models.Periodo(); }

                if (_Periodos.Id > 0)
                {
                    if (_Periodos.Id != _PeriodosP.Id)
                    {
                        return await Task.Run(() => BadRequest($"Ya existe el Año ingresado."));
                    }
                }


                if (_Periodos.Id == 0)
                {
                    _PeriodosP.FechaCreacion = DateTime.Now;
                    _PeriodosP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PeriodosP);
                }
                else
                {
                    _PeriodosP.UsuarioCreacion = _Periodos.UsuarioCreacion;
                    _PeriodosP.FechaCreacion = _Periodos.FechaCreacion;
                    var updateresult = await Update(_Periodos.Id, _PeriodosP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Periodos);
        }

        // POST: Periodo/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Periodo>> Insert(Periodo _Periodosp)
        {
            Periodo _Periodos = _Periodosp;
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Periodos.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Periodos.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Periodos.FechaCreacion = DateTime.Now;
                _Periodos.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Periodo/Insert", _Periodos);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Periodos = JsonConvert.DeserializeObject<Periodo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_Periodos);
           
        }

        [HttpPut("Id")]
        public async Task<ActionResult<Periodo>> Update(Int64 Id, Periodo _Periodo)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Periodo/Update", _Periodo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Periodo = JsonConvert.DeserializeObject<Periodo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_Periodo);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Periodo>> Delete([FromBody]Periodo _Periodo)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Periodo/Delete", _Periodo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Periodo = JsonConvert.DeserializeObject<Periodo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


            return Ok(_Periodo);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _Periodo }, Total = 1 });
        }
    }
}