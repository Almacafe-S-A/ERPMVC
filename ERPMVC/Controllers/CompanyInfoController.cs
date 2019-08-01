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
    public class CompanyInfoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CompanyInfoController(ILogger<CompanyInfoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult CompanyInfo()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCompanyInfo([FromBody]CompanyInfoDTO _sarpara)

        {
            CompanyInfoDTO _CompanyInfo = new CompanyInfoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfoById/" + _sarpara.CompanyInfoId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfoDTO>(valorrespuesta);

                }

                if (_CompanyInfo == null)
                {
                    _CompanyInfo = new CompanyInfoDTO();
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

        //[HttpPost("[action]")]
        //public async Task<ActionResult<CompanyInfo>> SaveCompanyInfo([FromBody]CompanyInfoDTO _CompanyInfo)
        //{

        //    try
        //    {
        //        CompanyInfo _listCompanyInfo = new CompanyInfo();
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfoById/" + _CompanyInfo.CompanyInfoId);
        //        string valorrespuesta = "";
        //        _CompanyInfo.FechaCreacion = DateTime.Now;
        //        _CompanyInfo.UsuarioCreacion = HttpContext.Session.GetString("user");
        //        if (result.IsSuccessStatusCode)
        //        {

        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _listCompanyInfo = JsonConvert.DeserializeObject<CompanyInfoDTO>(valorrespuesta);

        //        }

        //        if (_listCompanyInfo.CompanyInfoId == 0)
        //        {
        //            _CompanyInfo.FechaCreacion = DateTime.Now;
        //            _CompanyInfo.UsuarioCreacion = HttpContext.Session.GetString("user");
        //            var insertresult = await Insert(_CompanyInfo);
        //        }
        //        else
        //        {
        //            var updateresult = await Update(_CompanyInfo.CompanyInfoId, _CompanyInfo);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    return Json(_CompanyInfo);
        //}



        [HttpPost("[action]")]
        public async Task<ActionResult<CompanyInfo>> SaveCompanyInfo([FromBody]CompanyInfoDTO _CompanyInfoS)
        {
            CompanyInfo _CompanyInfo = _CompanyInfoS;
            try
            {
                CompanyInfo _listCompanyInfo= new CompanyInfo();
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
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

                if (_CompanyInfo == null) { _CompanyInfo = new Models.CompanyInfo(); }

                if (_CompanyInfoS.CompanyInfoId == 0)
                {
                    _CompanyInfoS.FechaCreacion = DateTime.Now;
                    _CompanyInfoS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CompanyInfoS);
                }
                else
                {
                    _CompanyInfoS.UsuarioCreacion = _CompanyInfo.UsuarioCreacion;
                    _CompanyInfoS.FechaCreacion = _CompanyInfo.FechaCreacion;
                    var updateresult = await Update(_CompanyInfo.CompanyInfoId, _CompanyInfoS);
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

        [HttpPut("{CompanyInfoId}")]
        public async Task<ActionResult<CompanyInfo>> Update(Int64 CompanyInfoId, CompanyInfo _CompanyInfop)
        {
            CompanyInfo _CompanyInfo = _CompanyInfop;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CompanyInfo.FechaModificacion = DateTime.Now;
                _CompanyInfo.UsuarioModificacion = HttpContext.Session.GetString("user");
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
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<CompanyInfo>> Delete(Int64 CompanyInfo, CompanyInfo _CompanyInfo)
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