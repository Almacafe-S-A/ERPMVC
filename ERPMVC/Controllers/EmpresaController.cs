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
    public class EmpresaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public EmpresaController(ILogger<EmpresaController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Customer
        public ActionResult Empresa()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Empresa> _cais = new List<Empresa>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Empresa/GetEmpresa");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<Empresa>>(valorrespuesta);

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
            List<Empresa> _Empresa = new List<Empresa>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Empresa/GetEmpresa");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Empresa = JsonConvert.DeserializeObject<List<Empresa>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Empresa);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddEmpresa([FromBody]EmpresaDTO _sarpara)
        {
            EmpresaDTO _Empresa = new EmpresaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Empresa/GetEmpresaById/" + _sarpara.IdEmpresa);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Empresa = JsonConvert.DeserializeObject<EmpresaDTO>(valorrespuesta);

                }

                if (_Empresa == null)
                {
                    _Empresa = new EmpresaDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Empresa);

        }


        [HttpPost]
        public async Task<ActionResult<Empresa>> SaveEmpresa([FromBody]EmpresaDTO _EmpresaP)
        {

            Empresa _Empresa = _EmpresaP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Empresa/GetEmpresaById/" + _Empresa.IdEmpresa);
                string valorrespuesta = "";
                _Empresa.FechaModificacion = DateTime.Now;
                _Empresa.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Empresa = JsonConvert.DeserializeObject<EmpresaDTO>(valorrespuesta);
                }

                if (_Empresa == null) { _Empresa = new Models.Empresa(); }

                if (_EmpresaP.IdEmpresa == 0)
                {
                    _Empresa.FechaCreacion = DateTime.Now;
                    _Empresa.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EmpresaP);
                }
                else
                {
                    _EmpresaP.Usuariocreacion = _Empresa.Usuariocreacion;
                    _EmpresaP.FechaCreacion = _Empresa.FechaCreacion;
                    var updateresult = await Update(_Empresa.IdEmpresa, _EmpresaP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Empresa);
        }


        //--------------------------------------------------------------------------------------
        // POST: Empresa/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Empresa _EmpresaP)
        {
            Empresa _Empresa = _EmpresaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Empresa.Usuariocreacion = HttpContext.Session.GetString("user");
                _Empresa.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Empresa.FechaCreacion = DateTime.Now;
                _Empresa.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Empresa/Insert", _Empresa);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Empresa = JsonConvert.DeserializeObject<Empresa>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Empresa }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, Empresa _EmpresaP)
        {
            Empresa _Empresa = _EmpresaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Empresa.FechaModificacion = DateTime.Now;
                _Empresa.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Empresa/Update", _Empresa);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Empresa = JsonConvert.DeserializeObject<Empresa>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Empresa }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult<Empresa>> Delete(Int64 Id, Empresa _EmpresaP)
        {
            Empresa _Empresa = _EmpresaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Empresa/Delete", _Empresa);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Empresa = JsonConvert.DeserializeObject<Empresa>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Empresa }, Total = 1 });
        }






    }
}