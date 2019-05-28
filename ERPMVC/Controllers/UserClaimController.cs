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
    public class UserClaimController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public UserClaimController(ILogger<UserClaimController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;

        }

        public ActionResult UserClaim()
        {
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationUserClaim> _ApplicationUserClaim = new List<ApplicationUserClaim>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/UserClaim/Get");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ApplicationUserClaim = JsonConvert.DeserializeObject<List<ApplicationUserClaim>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_ApplicationUserClaim.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUserClaim>> SaveUserClaim([FromBody]DTO_UserClaim _UserClaim)
        {

            try
            {
                DTO_UserClaim _li_UserClaim = new DTO_UserClaim();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/UserClaim/Get/" + _UserClaim.Id);
                string valorrespuesta = "";
              
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserClaim = JsonConvert.DeserializeObject<DTO_UserClaim>(valorrespuesta);
                }

                if (_UserClaim.Id == 0)
                {
                  
                    var insertresult = await Insert(_UserClaim);
                }
                else
                {
                    var updateresult = await Update(_UserClaim.Id, _UserClaim);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_UserClaim);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<ApplicationUserClaim>> Insert(ApplicationUserClaim _UserClaim)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/NumeracionSAR/Insert", _UserClaim);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserClaim = JsonConvert.DeserializeObject<ApplicationUserClaim>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _UserClaim }, Total = 1 });
        }

        [HttpPut("Id")]
        public async Task<IActionResult> Update(Int64 IdNumeracion, ApplicationUserClaim _UserClaim)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/NumeracionSAR/Update", _UserClaim);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserClaim = JsonConvert.DeserializeObject<ApplicationUserClaim>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _UserClaim }, Total = 1 });
        }

        [HttpDelete("IdNumeracion")]
        public async Task<ActionResult<ApplicationUserClaim>> Delete(Int64 Id, ApplicationUserClaim _UserClaim)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/NumeracionSAR/Delete", _UserClaim);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserClaim = JsonConvert.DeserializeObject<ApplicationUserClaim>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _UserClaim }, Total = 1 });
        }
    }
}