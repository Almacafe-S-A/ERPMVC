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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace ERPMVC.Controllers
{
    public class FeriadoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public FeriadoController(IOptions<MyConfig> config, ILogger<FeriadoController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Feriado()
        {
            return View();
        }
        
        public async Task<ActionResult> GetFeriados()
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/Feriado/GetFeriados");
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Feriado>>(contenido);
                return Ok(resultado);
            }
            return BadRequest();
        }


        [HttpGet]
        public async Task<DataSourceResult> GetFeriadosByPeriodo([DataSourceRequest] DataSourceRequest request, int PeriodoId)
        {
            List<Feriado> _Feriado = new List<Feriado>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Feriado/GetFeriadosByPeriodo/{PeriodoId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Feriado = JsonConvert.DeserializeObject<List<Feriado>>(valorrespuesta);
                    _Feriado = _Feriado.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _Feriado.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Feriado>> SaveFeriado([FromBody]FeriadoDTO feriado)
        {
            Feriado _Feriado = feriado;
            try
            {
                Feriado _listferiado = new Feriado();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                if (_Feriado.Id == 0)
                {
                    var result = await _client.GetAsync(baseadress + "api/Feriado/GetFeriadoByName/" + _Feriado.Nombre);
                    string valorrespuesta = "";
                    _Feriado.FechaModificacion = DateTime.Now;
                    _Feriado.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Feriado = JsonConvert.DeserializeObject<Feriado>(valorrespuesta);
                    }
                    if (_Feriado == null) { _Feriado = new Models.Feriado(); }

                    if (_Feriado.Nombre == feriado.Nombre && _Feriado.PeriodoId == feriado.PeriodoId)
                    {
                        return await Task.Run(() => BadRequest($"Ya existe un feriado registrado con ese nombre."));
                    }
                    if (feriado.FechaInicio > feriado.FechaFin)
                    {
                        return await Task.Run(() => BadRequest($"La fecha de inicio no puede ser mayor a la fecha de fin."));
                    }
                }

                if (feriado.Id == 0)
                {
                    feriado.FechaCreacion = DateTime.Now;
                    feriado.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(feriado);
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/Feriado/GetFeriadoById/" + _Feriado.Id);
                    feriado.UsuarioCreacion = _Feriado.UsuarioCreacion;
                    feriado.FechaCreacion = _Feriado.FechaCreacion;
                    var updateresult = await Update(_Feriado.Id, feriado);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return Json(feriado);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddFeriado([FromBody] FeriadoDTO _sarpara)
        {
            Feriado _Feriado = new FeriadoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Feriado/GetFeriadoById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Feriado = JsonConvert.DeserializeObject<FeriadoDTO>(valorrespuesta);

                }

                if (_Feriado == null)
                {
                    _Feriado = new FeriadoDTO { FechaCreacion = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
            return PartialView(_Feriado);
        }


        [HttpPost]
        public async Task<ActionResult<Feriado>> Insert(Feriado _Feriado)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Feriado.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Feriado.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Feriado.FechaCreacion = DateTime.Now;
                _Feriado.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Feriado/Insert", _Feriado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Feriado = JsonConvert.DeserializeObject<Feriado>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Feriado);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Feriado>> Update(Int64 id, Feriado _Feriado)
        {
            try
            {
                // TODO: Add insert logic here
                string baseAddress = config.Value.urlbase;
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Feriado.FechaModificacion = DateTime.Now;
                _Feriado.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await httpClient.PutAsJsonAsync(baseAddress + "api/Feriado/Update", _Feriado);
                string valorRespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorRespuesta = await result.Content.ReadAsStringAsync();
                    _Feriado = JsonConvert.DeserializeObject<Feriado>(valorRespuesta);
                }
                else
                {
                    valorRespuesta = await result.Content.ReadAsStringAsync();
                    return BadRequest(valorRespuesta);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Feriado }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<Feriado>> Delete(Int64 Id, [FromBody] Feriado feriado)
        {
            Feriado _Feriado = feriado;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Feriado/Delete", _Feriado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Feriado = JsonConvert.DeserializeObject<Feriado>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    //throw  new Exception(d);
                    return await Task.Run(() => BadRequest($"{d}"));
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Feriado }, Total = 1 });
        }
    }
}