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
    public class EmployeeExtraHoursController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EmployeeExtraHoursController(ILogger<EmployeeExtraHoursController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [Authorize(Policy = "Clientes.Horas Extra")]
        public async  Task<IActionResult> Index()
        {
            return await Task.Run(()=> View());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddEmployeeExtraHours([FromBody]EmployeeExtraHoursDTO _EmployeeExtraHoursDTO)
        {
            EmployeeExtraHoursDTO _EmployeeExtraHours = new EmployeeExtraHoursDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHours/GetEmployeeExtraHoursById/" + _EmployeeExtraHoursDTO.EmployeeExtraHoursId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHours = JsonConvert.DeserializeObject<EmployeeExtraHoursDTO>(valorrespuesta);

                }

                if (_EmployeeExtraHours == null)
                {
                    _EmployeeExtraHours = new EmployeeExtraHoursDTO { WorkDate = DateTime.Now , StartTime = DateTime.Now , EndTime = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EmployeeExtraHours);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EmployeeExtraHours> _EmployeeExtraHours = new List<EmployeeExtraHours>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHours/GetEmployeeExtraHours");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHours = JsonConvert.DeserializeObject<List<EmployeeExtraHours>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EmployeeExtraHours.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EmployeeExtraHours>> SaveEmployeeExtraHours([FromBody]EmployeeExtraHours _EmployeeExtraHours)
        {
            EmployeeExtraHours _EmployeeExtraHoursp = _EmployeeExtraHours;
            try
            {
                EmployeeExtraHours _listEmployeeExtraHours = new EmployeeExtraHours();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/EmployeeExtraHours/GetEmployeeExtraHoursByEmployeeName/" + _EmployeeExtraHours.EmployeeName);
                string valorrespuesta1 = "";

                if (result1.IsSuccessStatusCode)
                {
                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                    _EmployeeExtraHoursp = JsonConvert.DeserializeObject<EmployeeExtraHoursDTO>(valorrespuesta1);
                }

                if (_EmployeeExtraHoursp == null) { _EmployeeExtraHoursp = new Models.EmployeeExtraHours(); }

                if (_EmployeeExtraHoursp.EmployeeExtraHoursId > 0)
                {
                    if (_EmployeeExtraHoursp.EmployeeExtraHoursId != _EmployeeExtraHours.EmployeeExtraHoursId)
                        return await Task.Run(() => BadRequest($"Ya existe un Empleado registrado con el mismo Nombre."));
                }


                if (_EmployeeExtraHours.EmployeeExtraHoursId == 0)
                {
                    _EmployeeExtraHoursp.FechaCreacion = DateTime.Now;
                    _EmployeeExtraHoursp.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EmployeeExtraHours);
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHours/GetEmployeeExtraHoursById/" + _EmployeeExtraHours.EmployeeExtraHoursId);
                    string valorrespuesta = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _EmployeeExtraHoursp = JsonConvert.DeserializeObject<EmployeeExtraHoursDTO>(valorrespuesta);
                    }
                    //_EmployeeExtraHours.FechaCreacion = _EmployeeExtraHoursp.FechaCreacion;
                    //_EmployeeExtraHours.UsuarioCreacion = _EmployeeExtraHoursp.UsuarioCreacion;
                    _EmployeeExtraHours.UsuarioCreacion = _EmployeeExtraHoursp.UsuarioCreacion;
                    _EmployeeExtraHours.FechaCreacion = _EmployeeExtraHoursp.FechaCreacion;
                    var updateresult = await Update(_EmployeeExtraHoursp.EmployeeExtraHoursId, _EmployeeExtraHours);
                }

            }
            //try
            //{
            //    EmployeeExtraHours _listEmployeeExtraHours = new EmployeeExtraHours();
            //    string baseadress = config.Value.urlbase;
            //    HttpClient _client = new HttpClient();
            //    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            //    var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHours/GetEmployeeExtraHoursById/" + _EmployeeExtraHours.EmployeeExtraHoursId);
            //    string valorrespuesta = "";
            //    _EmployeeExtraHours.FechaModificacion = DateTime.Now;
            //    _EmployeeExtraHours.UsuarioModificacion = HttpContext.Session.GetString("user");
            //    if (result.IsSuccessStatusCode)
            //    {

            //        valorrespuesta = await (result.Content.ReadAsStringAsync());
            //        _listEmployeeExtraHours = JsonConvert.DeserializeObject<EmployeeExtraHours>(valorrespuesta);
            //    }

            //    if (_listEmployeeExtraHours == null) { _listEmployeeExtraHours = new EmployeeExtraHours(); }

            //    if (_listEmployeeExtraHours.EmployeeExtraHoursId == 0)
            //    {
            //        _EmployeeExtraHours.FechaCreacion = DateTime.Now;
            //        _EmployeeExtraHours.UsuarioCreacion = HttpContext.Session.GetString("user");
            //        var insertresult = await Insert(_EmployeeExtraHours);
            //    }
            //    else
            //    {
            //        var updateresult = await Update(_EmployeeExtraHours);
            //    }

            //}
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_EmployeeExtraHours));
        }

        // POST: EmployeeExtraHours/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EmployeeExtraHours>> Insert(EmployeeExtraHours _EmployeeExtraHours)
        {
            EmployeeExtraHours _EmployeeExtraHoursp = _EmployeeExtraHours;
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //_EmployeeExtraHours.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_EmployeeExtraHours.UsuarioModificacion = HttpContext.Session.GetString("user");
                _EmployeeExtraHoursp.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EmployeeExtraHoursp.UsuarioModificacion = HttpContext.Session.GetString("user");
                _EmployeeExtraHoursp.FechaCreacion = DateTime.Now;
                _EmployeeExtraHoursp.FechaModificacion = DateTime.Now;
                
                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeExtraHours/Insert", _EmployeeExtraHours);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHours = JsonConvert.DeserializeObject<EmployeeExtraHours>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return await Task.Run(() =>  Ok(_EmployeeExtraHours));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeExtraHours }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EmployeeExtraHours>> Update(Int64 Id, EmployeeExtraHours _EmployeeExtraHours)
        {
            EmployeeExtraHours _EmployeeExtraHoursp = _EmployeeExtraHours;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EmployeeExtraHoursp.FechaModificacion = DateTime.Now;
                _EmployeeExtraHoursp.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/EmployeeExtraHours/Update", _EmployeeExtraHours);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHours = JsonConvert.DeserializeObject<EmployeeExtraHours>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return await Task.Run(() => Ok(_EmployeeExtraHours));
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EmployeeExtraHours>> Delete([FromBody]EmployeeExtraHours _EmployeeExtraHours)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeExtraHours/Delete", _EmployeeExtraHours);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHours = JsonConvert.DeserializeObject<EmployeeExtraHours>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return await Task.Run(() => Ok(_EmployeeExtraHours));
        }





    }
}