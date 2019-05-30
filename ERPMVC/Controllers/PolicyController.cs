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
    public class PolicyController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PolicyController(ILogger<PolicyController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Policy
        public ActionResult Policy()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Policy> _cais = new List<Policy>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Policies/GetPolicies");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<Policy>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            
            return Json(_cais.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult<Policy>> SavePolicy([FromBody]DTO_Policy _Policy)
        {

            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Policies/GetPoliciesSARById/" + _Policy.Id);
                string valorrespuesta = "";
                _Policy.FechaModificacion = DateTime.Now;
                _Policy.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Policy = JsonConvert.DeserializeObject<DTO_Policy>(valorrespuesta);
                }

                //_Policy.Id.ToString("N");
                // _Policy = _Policy.Where(q => q.Id == Id).ToList();


                if(_Policy.Id.ToString() == "00000000-0000-0000-0000-000000000000")
                
                {
                    _Policy.FechaCreacion = DateTime.Now;
                    _Policy.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Policy);
                }
                else
                {
                  
                   
                   // var updateresult = await Update(_Policy.Id, _Policy);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Policy);
            
        }



        [HttpPost]
        public async Task<ActionResult> Insert(Policy _Policyp)
        {
            Policy _Policy = _Policyp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Policy.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Policy.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Policy.FechaCreacion = DateTime.Now;
                _Policy.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Policies/Insert", _Policy);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Policy = JsonConvert.DeserializeObject<Policy>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Policy }, Total = 1 });
        }

        [HttpPut]
        public async Task<IActionResult> Update(Int64 id, Policy _Policy)
        {
  
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Policy.FechaModificacion = DateTime.Now;
                _Policy.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Policies/Update", _Policy);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Policy = JsonConvert.DeserializeObject<Policy>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Policy }, Total = 1 });
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<Policy>> Delete(Int64 Id, Policy _Policyp)
        {
            Policy _Policy = _Policyp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Policies/Delete", _Policy);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Policy = JsonConvert.DeserializeObject<Policy>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Policy }, Total = 1 });
        }
    }
}