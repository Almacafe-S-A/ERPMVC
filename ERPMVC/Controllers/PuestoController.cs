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
    public class PuestoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PuestoController(ILogger<PuestoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Customer
        [Authorize(Policy = "RRHH.Puestos")]
        public ActionResult Puesto()
        {
            return View();
        }
        public async Task<JsonResult> GetPuestoById(Int64 PuestoId)
        {
            Puesto _PuestoP = new Puesto();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuestoById/" + PuestoId);
                string valorrespuesta = "";
                _PuestoP.FechaCreacion = DateTime.Now;
                _PuestoP.Usuariocreacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuestoP = JsonConvert.DeserializeObject<Puesto>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PuestoP);
        }

        public async Task<JsonResult> GetPuestoByIdDepartamento([DataSourceRequest]DataSourceRequest request, Int64 IdDepartamento)
        {
            List<Puesto> _PuestoP = new List<Puesto>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuestoByIdDepartamento/" + IdDepartamento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuestoP = JsonConvert.DeserializeObject<List<Puesto>>(valorrespuesta);
                    _PuestoP = _PuestoP.Where(q => q.Estado == "Activo").ToList();
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PuestoP.ToDataSourceResult(request));
        }
        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Puesto> _cais = new List<Puesto>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuesto");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<Puesto>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_cais.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<JsonResult> GetPuestos([DataSourceRequest]DataSourceRequest request)
        {
            List<Puesto> _cais = new List<Puesto>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuesto");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<Puesto>>(valorrespuesta);
                    _cais = _cais.Where(q => q.Estado == "Activo").ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_cais.ToDataSourceResult(request));

        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<Puesto> _Puesto = new List<Puesto>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuesto");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Puesto = JsonConvert.DeserializeObject<List<Puesto>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Puesto);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPuesto([FromBody]PuestoDTO _sarpara)
        {
            PuestoDTO _Puesto = new PuestoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuestoById/" + _sarpara.IdPuesto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Puesto = JsonConvert.DeserializeObject<PuestoDTO>(valorrespuesta);

                }

                if (_Puesto == null)
                {
                    _Puesto = new PuestoDTO { FechaCreacion = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Puesto);

        }


        [HttpPost]
        public async Task<ActionResult<Puesto>> SavePuesto([FromBody]PuestoDTO _PuestoP)
        {

            Puesto _Puesto = _PuestoP;
            try
            {
                Puesto _listPuesto = new Puesto();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/Puesto/GetPuestoByNombrePuesto/" + _Puesto.NombrePuesto + "/" + _Puesto.NombreDepartamento);
                string valorrespuesta1 = "";

                if (result1.IsSuccessStatusCode)
                {
                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                    _Puesto = JsonConvert.DeserializeObject<PuestoDTO>(valorrespuesta1);
                }

                if (_Puesto == null) { _Puesto = new Models.Puesto(); }

                if (_Puesto.IdPuesto > 0)
                {
                    if (_Puesto.IdPuesto != _PuestoP.IdPuesto)
                        return await Task.Run(() => BadRequest($"Ya existe un Puesto registrado para este Departamento con el mismo Nombre."));
                }


                if (_PuestoP.IdPuesto == 0)
                {
                    _Puesto.FechaCreacion = DateTime.Now;
                    _Puesto.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PuestoP);
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/Puesto/GetPuestoById/" + _PuestoP.IdPuesto);
                    string valorrespuesta = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Puesto = JsonConvert.DeserializeObject<PuestoDTO>(valorrespuesta);
                    }
                    _PuestoP.FechaCreacion = _Puesto.FechaCreacion;
                    _PuestoP.Usuariocreacion = _Puesto.Usuariocreacion;
                    _Puesto.Usuariomodificacion = _Puesto.Usuariomodificacion;
                    _Puesto.FechaModificacion = _Puesto.FechaModificacion;
                    var updateresult = await Update(_Puesto.IdPuesto, _PuestoP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Puesto);
        }


        //--------------------------------------------------------------------------------------
        // POST: Puesto/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Puesto _PuestoP)
        {
            Puesto _Puesto = _PuestoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Puesto.Usuariocreacion = HttpContext.Session.GetString("user");
                _Puesto.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Puesto.FechaCreacion = DateTime.Now;
                _Puesto.FechaModificacion = DateTime.Now;

                var result = await _client.PostAsJsonAsync(baseadress + "api/Puesto/Insert", _Puesto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Puesto = JsonConvert.DeserializeObject<Puesto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Puesto }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, Puesto _PuestoP)
        {
            Puesto _Puesto = _PuestoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Puesto.FechaModificacion = DateTime.Now;
                _Puesto.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Puesto/Update", _Puesto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Puesto = JsonConvert.DeserializeObject<Puesto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Puesto }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult<Puesto>> Delete([FromBody] Puesto _PuestoP)
        {
            Puesto _Puesto = _PuestoP;
            List<Employees> _Employees = new List<Employees>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/Puesto/ValidationDelete/" + _Puesto.IdPuesto);
                string valorrespuesta1 = "";

                if (result1.IsSuccessStatusCode)
                {

                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                }
                if (valorrespuesta1 == "0")
                {
                    var result = await _client.PostAsJsonAsync(baseadress + "api/Puesto/Delete", _Puesto);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Puesto = JsonConvert.DeserializeObject<Puesto>(valorrespuesta);
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("Este registro tiene referencia a otros datos,No se puede Eliminar"));
                }


            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Puesto }, Total = 1 });
        }






    }
}