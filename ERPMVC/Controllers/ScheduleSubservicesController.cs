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
    public class ScheduleSubservicesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ScheduleSubservicesController(ILogger<ScheduleSubservicesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddScheduleSubServices([FromBody]ScheduleSubservices _ScheduleSubservicesp)
        {
            ScheduleSubservicesDTO _ScheduleSubservices = new ScheduleSubservicesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ScheduleSubservices/GetScheduleSubservicesById/" + _ScheduleSubservicesp.ScheduleSubservicesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ScheduleSubservices = JsonConvert.DeserializeObject<ScheduleSubservicesDTO>(valorrespuesta);

                }

                if (_ScheduleSubservices == null)
                {
                    _ScheduleSubservices = new ScheduleSubservicesDTO {  StartTime = DateTime.Now, EndTime = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => PartialView(_ScheduleSubservices));

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ScheduleSubservices> _ScheduleSubservices = new List<ScheduleSubservices>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ScheduleSubservices/GetScheduleSubservices");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ScheduleSubservices = JsonConvert.DeserializeObject<List<ScheduleSubservices>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => _ScheduleSubservices.ToDataSourceResult(request));

        }

        [HttpPost("[controller]/[action]")]
      //  public async Task<ActionResult<ScheduleSubservices>> SaveScheduleSubservices([FromBody]dynamic dto)
       public async Task<ActionResult<ScheduleSubservices>> SaveScheduleSubservices([FromBody]ScheduleSubservices _ScheduleSubservices)
        {
            //ScheduleSubservices _ScheduleSubservices = new ScheduleSubservices();
            try
            {
               // _ScheduleSubservices = JsonConvert.DeserializeObject<ScheduleSubservices>(dto.ToString());
                if (_ScheduleSubservices != null)
                {
                    ScheduleSubservices _listScheduleSubservices = new ScheduleSubservices();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ScheduleSubservices/GetScheduleSubservicesById/" + _ScheduleSubservices.ScheduleSubservicesId);
                    string valorrespuesta = "";
                    _ScheduleSubservices.FechaModificacion = DateTime.Now;
                    _ScheduleSubservices.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listScheduleSubservices = JsonConvert.DeserializeObject<ScheduleSubservices>(valorrespuesta);
                    }

                    if (_listScheduleSubservices == null) { _listScheduleSubservices = new ScheduleSubservices(); }

                    if (_listScheduleSubservices.ScheduleSubservicesId == 0)
                    {
                        _ScheduleSubservices.FechaCreacion = DateTime.Now;
                        _ScheduleSubservices.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_ScheduleSubservices);
                    }
                    else
                    {
                        var updateresult = await Update(_ScheduleSubservices.ScheduleSubservicesId, _ScheduleSubservices);
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_ScheduleSubservices));
        }

        // POST: ScheduleSubservices/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ScheduleSubservices>> Insert(ScheduleSubservices _ScheduleSubservices)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ScheduleSubservices.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ScheduleSubservices.UsuarioModificacion = HttpContext.Session.GetString("user");
                TimeSpan t = _ScheduleSubservices.EndTime.Subtract(_ScheduleSubservices.StartTime);
                _ScheduleSubservices.QuantityHours = t.TotalHours;
                var result = await _client.PostAsJsonAsync(baseadress + "api/ScheduleSubservices/Insert", _ScheduleSubservices);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ScheduleSubservices = JsonConvert.DeserializeObject<ScheduleSubservices>(valorrespuesta);
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

            return await Task.Run(() => Ok(_ScheduleSubservices));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ScheduleSubservices }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ScheduleSubservices>> Update(Int64 id, ScheduleSubservices _ScheduleSubservices)
        {
            try
            {
                
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                TimeSpan t = _ScheduleSubservices.EndTime.Subtract(_ScheduleSubservices.StartTime);
                _ScheduleSubservices.QuantityHours = t.TotalHours;
                var result = await _client.PutAsJsonAsync(baseadress + "api/ScheduleSubservices/Update", _ScheduleSubservices);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ScheduleSubservices = JsonConvert.DeserializeObject<ScheduleSubservices>(valorrespuesta);
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

            return await Task.Run(() => Ok(_ScheduleSubservices));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ScheduleSubservices>> Delete([FromBody]ScheduleSubservices _ScheduleSubservices)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ScheduleSubservices/Delete", _ScheduleSubservices);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ScheduleSubservices = JsonConvert.DeserializeObject<ScheduleSubservices>(valorrespuesta);
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



            return await Task.Run(()=> Ok(_ScheduleSubservices));
        }





    }
}