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
    public class PolicyRolesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PolicyRolesController(ILogger<PolicyRolesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;

        }

        public ActionResult PolicyRoles()
        {
            return View();
        }


        [HttpGet ]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PolicyRoles> _cais = new List<PolicyRoles>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PolicyRoles/GetPolicyRoles");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<PolicyRoles>>(valorrespuesta);

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
        public async Task<ActionResult> GetPolicyRole([DataSourceRequest]DataSourceRequest request)
        {
            List<PolicyRoles> _PolicyRoles = new List<PolicyRoles>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PolicyRoles/GetPolicyRoles");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PolicyRoles = JsonConvert.DeserializeObject<List<PolicyRoles>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_PolicyRoles.ToDataSourceResult(request));

        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPolicyRoles([FromBody]PolicyRolesDTO _sarpara)
        {
            PolicyRolesDTO _Policy = new PolicyRolesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PolicyRoles/GetPolicyRolesById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Policy = JsonConvert.DeserializeObject<PolicyRolesDTO>(valorrespuesta);

                }

                if (_Policy == null)
                {
                    _Policy = new PolicyRolesDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Policy);

        }




        [HttpPost("[action]")]
        public async Task<ActionResult<PolicyRoles>> SavePolicyRoles([FromBody]PolicyRolesDTO _PolicyRolesp)
        {
            PolicyRoles _PolicyRoles = _PolicyRolesp;
            try
            {
                
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PolicyRoles/GetPolicyRolesById/" + _PolicyRoles.Id);
                string valorrespuesta = "";              
                _PolicyRoles.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PolicyRoles = JsonConvert.DeserializeObject<PolicyRoles>(valorrespuesta);
                }

                if (_PolicyRoles == null) { _PolicyRoles = new Models.PolicyRoles(); }
              


                if (_PolicyRolesp.Id.ToString() == "00000000-0000-0000-0000-000000000000")

                {

                    _PolicyRoles.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PolicyRolesp);
                }
                else
                {
                    _PolicyRolesp.UsuarioCreacion = _PolicyRoles.UsuarioCreacion;                    
                    var updateresult = await Update(_PolicyRolesp.Id, _PolicyRolesp);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PolicyRoles);

        }



        [HttpPost]
        public async Task<ActionResult> Insert(PolicyRoles _PolicyRoles)
        {
           
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PolicyRoles.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PolicyRoles.UsuarioModificacion = HttpContext.Session.GetString("user");             
                var result = await _client.PostAsJsonAsync(baseadress + "api/PolicyRoles/Insert", _PolicyRoles);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PolicyRoles = JsonConvert.DeserializeObject<PolicyRoles>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_PolicyRoles);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _PolicyRoles }, Total = 1 });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(Guid id, PolicyRoles _PolicyRoles)
        {

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PolicyRoles.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/PolicyRoles/Update", _PolicyRoles);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PolicyRoles = JsonConvert.DeserializeObject<PolicyRoles>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_PolicyRoles);//new ObjectResult(new DataSourceResult { Data = new[] { _PolicyRoles }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<PolicyRoles>> Delete(Int64 Id, PolicyRoles _PolicyRoles)
        {

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PolicyRoles/Delete", _PolicyRoles);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PolicyRoles = JsonConvert.DeserializeObject<PolicyRoles>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PolicyRoles }, Total = 1 });
        }
    }
}