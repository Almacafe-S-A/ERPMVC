using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERPMVC.Models;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using ERPMVC.DTO;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class InsuranceCertificateController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public InsuranceCertificateController(ILogger<InsuranceCertificateController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        
        [Authorize(Policy = "Contabilidad.Seguros.Certificados de Seguro")]        
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        public IActionResult SFSegurosEndosados()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwAddInsuranceCertificate([FromBody] InsuranceCertificate _InsuranceCertificatep)
        {
            InsuranceCertificate _InsuranceCertificate = new InsuranceCertificate();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceCertificate/GetInsuranceCertificateById/" + _InsuranceCertificatep.Id);
                string valorrespuesta = "";
                _InsuranceCertificate.FechaCreacion = DateTime.Now;
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceCertificate = JsonConvert.DeserializeObject<InsuranceCertificate>(valorrespuesta);

                }

                if (_InsuranceCertificate == null)
                {
                    _InsuranceCertificate = new InsuranceCertificate();
                    _InsuranceCertificate.FechaCreacion = DateTime.Now;
                }
                else
                {
                    // _InsuranceCertificate.NumeroDEIString = $"{_InsuranceCertificate.Sucursal}-{_InsuranceCertificate.Caja}-01-{_InsuranceCertificate.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                
            }



            return PartialView(_InsuranceCertificate);

        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest] DataSourceRequest request)
        {
            List<InsuranceCertificate> _InsuranceCertificate = new List<InsuranceCertificate>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceCertificate/GetInsuranceCertificate");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceCertificate = JsonConvert.DeserializeObject<List<InsuranceCertificate>>(valorrespuesta);
                    _InsuranceCertificate = _InsuranceCertificate.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                
            }


            return _InsuranceCertificate.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetByInsuracePolicyId([DataSourceRequest] DataSourceRequest request, int insurancePolicyId)
        {
            List<InsuranceCertificate> _InsuranceCertificate = new List<InsuranceCertificate>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceCertificate/GetInsuranceCertificateByInsurancePolicyId/" + insurancePolicyId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceCertificate = JsonConvert.DeserializeObject<List<InsuranceCertificate>>(valorrespuesta);
                    _InsuranceCertificate = _InsuranceCertificate.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                
            }


            return _InsuranceCertificate.ToDataSourceResult(request);

        }


        [HttpPost]
        public async Task<ActionResult> GenerarCertificados()
        {
            List<InsuranceCertificate> insuranceCertificates = new List<InsuranceCertificate>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceCertificate/GenerateInsuranceCertificates" );
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                
            }


            return Ok(insuranceCertificates.Count) ;

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuranceCertificate>> SaveInsuranceCertificate([FromBody] InsuranceCertificate _InsuranceCertificate)
        {

            try
            {
                InsuranceCertificate _listInsuranceCertificate = new InsuranceCertificate();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceCertificate/GetInsuranceCertificateById/" + _InsuranceCertificate.Id);
                string valorrespuesta = "";
                _InsuranceCertificate.FechaModificacion = DateTime.Now;
                _InsuranceCertificate.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInsuranceCertificate = JsonConvert.DeserializeObject<InsuranceCertificate>(valorrespuesta);
                }

                if (_listInsuranceCertificate == null) { _listInsuranceCertificate = new InsuranceCertificate(); }

                if (_listInsuranceCertificate.Id == 0)
                {
                    _InsuranceCertificate.FechaCreacion = DateTime.Now;
                    _InsuranceCertificate.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsuranceCertificate);
                    var value = (insertresult.Result as ObjectResult).Value;

                    InsuranceCertificate resultado = ((InsuranceCertificate)(value));
                    if (resultado.Id <= 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero la factura!"));
                    }
                    else
                    {
                        // _InsuranceCertificate.NumeroDEIString = $"{resultado.Sucursal}-01-{resultado.NumeroDEI.ToString().PadLeft(8, '0')} ";
                    }

                }
                else
                {
                    var updateresult = await Update(_InsuranceCertificate.Id, _InsuranceCertificate);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"No se genero la factura : {ex.ToString()}"));

                
            }

            return Json(_InsuranceCertificate);
        }

        // POST: InsuranceCertificate/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsuranceCertificate>> Insert(InsuranceCertificate _InsuranceCertificate)
        {
            try
            {
                // TODO: Add insert logic here
                _InsuranceCertificate.EstadoId = 1;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsuranceCertificate.UsuarioCreacion = HttpContext.Session.GetString("user");
                _InsuranceCertificate.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuranceCertificate/Insert", _InsuranceCertificate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceCertificate = JsonConvert.DeserializeObject<InsuranceCertificate>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    
                    return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }
            return Ok(_InsuranceCertificate);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuranceCertificate }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsuranceCertificate>> Update(Int64 id, InsuranceCertificate _InsuranceCertificate)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InsuranceCertificate/Update", _InsuranceCertificate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceCertificate = JsonConvert.DeserializeObject<InsuranceCertificate>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_InsuranceCertificate));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuranceCertificate }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuranceCertificate>> Delete([FromBody] InsuranceCertificate _InsuranceCertificate)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuranceCertificate/Delete", _InsuranceCertificate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceCertificate = JsonConvert.DeserializeObject<InsuranceCertificate>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(() => Ok(_InsuranceCertificate));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuranceCertificate }, Total = 1 });
        }



        [HttpGet]
        public async Task<ActionResult> SFInsuranceCertificate(Int32 id)
        {
            try
            {
                InsuranceCertificate _InsuranceCertificate = new InsuranceCertificate { Id = id, };
                return await Task.Run(() => View(_InsuranceCertificate));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }
    }
}