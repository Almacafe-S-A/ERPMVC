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
    public class CompanyInfoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CompanyInfoController(ILogger<CompanyInfoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwCompanyInfo(Int64 Id = 0)
        {
            CompanyInfo _CompanyInfo = new CompanyInfo();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfoById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);

                }

                if (_CompanyInfo == null)
                {
                    _CompanyInfo = new CompanyInfo();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CompanyInfo);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CompanyInfo> _CompanyInfo = new List<CompanyInfo>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<List<CompanyInfo>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CompanyInfo.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CompanyInfo>> SaveCompanyInfo([FromBody]CompanyInfo _CompanyInfo)
        {

            try
            {
                CompanyInfo _listCompanyInfo = new CompanyInfo();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfoById/" + _CompanyInfo.CompanyInfoId);
                string valorrespuesta = "";
                _CompanyInfo.FechaModificacion = DateTime.Now;
                _CompanyInfo.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

                if (_listCompanyInfo.CompanyInfoId == 0)
                {
                    _CompanyInfo.FechaCreacion = DateTime.Now;
                    _CompanyInfo.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CompanyInfo);
                }
                else
                {
                    var updateresult = await Update(_CompanyInfo.CompanyInfoId, _CompanyInfo);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CompanyInfo);
        }

        // POST: CompanyInfo/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CompanyInfo>> Insert(CompanyInfo _CompanyInfo)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CompanyInfo.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CompanyInfo.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CompanyInfo/Insert", _CompanyInfo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CompanyInfo);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyInfo>> Update(Int64 id, CompanyInfo _CompanyInfo)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CompanyInfo/Update", _CompanyInfo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CompanyInfo>> Delete([FromBody]CompanyInfo _CompanyInfo)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CompanyInfo/Delete", _CompanyInfo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }





    }
}