using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class EmployeeExtraHoursDetailController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EmployeeExtraHoursDetailController(ILogger<EmployeeExtraHoursDetailController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public async Task< IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwEmployeeExtraHoursDetail(Int64 Id = 0)
        {
            EmployeeExtraHoursDetail _EmployeeExtraHoursDetail = new EmployeeExtraHoursDetail();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHoursDetail/GetEmployeeExtraHoursDetailById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHoursDetail = JsonConvert.DeserializeObject<EmployeeExtraHoursDetail>(valorrespuesta);

                }

                if (_EmployeeExtraHoursDetail == null)
                {
                    _EmployeeExtraHoursDetail = new EmployeeExtraHoursDetail();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => PartialView(_EmployeeExtraHoursDetail));

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EmployeeExtraHoursDetail> _EmployeeExtraHoursDetail = new List<EmployeeExtraHoursDetail>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHoursDetail/GetEmployeeExtraHoursDetail");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHoursDetail = JsonConvert.DeserializeObject<List<EmployeeExtraHoursDetail>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => _EmployeeExtraHoursDetail.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EmployeeExtraHoursDetail>> SaveEmployeeExtraHoursDetail([FromBody]EmployeeExtraHoursDetail _EmployeeExtraHoursDetail)
        {

            try
            {
                EmployeeExtraHoursDetail _listEmployeeExtraHoursDetail = new EmployeeExtraHoursDetail();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeExtraHoursDetail/GetEmployeeExtraHoursDetailById/" + _EmployeeExtraHoursDetail.EmployeeExtraHoursDetailId);
                string valorrespuesta = "";
                _EmployeeExtraHoursDetail.FechaModificacion = DateTime.Now;
                _EmployeeExtraHoursDetail.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEmployeeExtraHoursDetail = JsonConvert.DeserializeObject<EmployeeExtraHoursDetail>(valorrespuesta);
                }

                if (_listEmployeeExtraHoursDetail == null) { _listEmployeeExtraHoursDetail = new EmployeeExtraHoursDetail(); }

                if (_listEmployeeExtraHoursDetail.EmployeeExtraHoursDetailId == 0)
                {
                    _EmployeeExtraHoursDetail.FechaCreacion = DateTime.Now;
                    _EmployeeExtraHoursDetail.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EmployeeExtraHoursDetail);
                }
                else
                {
                    var updateresult = await Update(_EmployeeExtraHoursDetail.EmployeeExtraHoursDetailId, _EmployeeExtraHoursDetail);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_EmployeeExtraHoursDetail));
        }

        // POST: EmployeeExtraHoursDetail/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EmployeeExtraHoursDetail>> Insert(EmployeeExtraHoursDetail _EmployeeExtraHoursDetail)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EmployeeExtraHoursDetail.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EmployeeExtraHoursDetail.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeExtraHoursDetail/Insert", _EmployeeExtraHoursDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHoursDetail = JsonConvert.DeserializeObject<EmployeeExtraHoursDetail>(valorrespuesta);
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
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return await Task.Run(() => Ok(_EmployeeExtraHoursDetail));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeExtraHoursDetail }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeExtraHoursDetail>> Update(Int64 id, EmployeeExtraHoursDetail _EmployeeExtraHoursDetail)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EmployeeExtraHoursDetail/Update", _EmployeeExtraHoursDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHoursDetail = JsonConvert.DeserializeObject<EmployeeExtraHoursDetail>(valorrespuesta);
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
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_EmployeeExtraHoursDetail));
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EmployeeExtraHoursDetail>> Delete([FromBody]EmployeeExtraHoursDetail _EmployeeExtraHoursDetail)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeExtraHoursDetail/Delete", _EmployeeExtraHoursDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeExtraHoursDetail = JsonConvert.DeserializeObject<EmployeeExtraHoursDetail>(valorrespuesta);
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
                return await Task.Run(() => BadRequest($"Ocurrio un error: {ex.Message}"));
            }



            return await Task.Run(() => Ok(_EmployeeExtraHoursDetail));
        }





    }
}