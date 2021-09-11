using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    public class CustomerContractTermsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public CustomerContractTermsController(ILogger<CustomerContractTermsController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Contabilidad.Centros de Costos")]
        public ActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwCustomerContractTerms([FromBody]CustomerContractTerms _sarpara)
        {
            CustomerContractTerms _CustomerContractTerms = new CustomerContractTerms();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractTerms/GetCustomerContractTermsById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<CustomerContractTerms>(valorrespuesta);

                }

                if (_CustomerContractTerms == null)
                {
                    _CustomerContractTerms = new CustomerContractTerms();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerContractTerms);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerContractTerms> _CustomerContractTerms = new List<CustomerContractTerms>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractTerms/GetCustomerContractTerms");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<List<CustomerContractTerms>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContractTerms.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetCustomerContractTerms([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerContractTerms> _CustomerContractTerms = new List<CustomerContractTerms>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractTerms/GetCustomerContractTerms");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<List<CustomerContractTerms>>(valorrespuesta);
                    if (_CustomerContractTerms.Count > 0)
                    {
                        //_CustomerContractTerms = _CustomerContractTerms.Where(q => q.Estado == "Activo").ToList();
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_CustomerContractTerms.ToDataSourceResult(request));

        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetCustomerContractTermsByBranchId([DataSourceRequest]DataSourceRequest request, Int64 BranchId)
        {
            List<CustomerContractTerms> _CustomerContractTerms = new List<CustomerContractTerms>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractTerms/GetCustomerContractTerms");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<List<CustomerContractTerms>>(valorrespuesta);

                   // _CustomerContractTerms = _CustomerContractTerms.Where(q => q.BranchId == BranchId).ToList();


    }


}
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_CustomerContractTerms.ToDataSourceResult(request));

        }


        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractTerms>> SaveCustomerContractTerms([FromBody]CustomerContractTerms _CustomerContractTerms)
        {

            try
            {
                CustomerContractTerms _listCustomerContractTerms = new CustomerContractTerms();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractTerms/GetCustomerContractTermsById/" + _CustomerContractTerms.Id);
                string valorrespuesta = "";
                _CustomerContractTerms.FechaModificacion = DateTime.Now;
                _CustomerContractTerms.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {                   
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerContractTerms = JsonConvert.DeserializeObject<CustomerContractTerms>(valorrespuesta);
                }
                if (_CustomerContractTerms.Id == 0)
                {
                    _CustomerContractTerms.FechaCreacion = DateTime.Now;
                    _CustomerContractTerms.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerContractTerms);
                }
                else
                {
                    var updateresult = await Update(_CustomerContractTerms.Id, _CustomerContractTerms);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error{ex.Message}");

                throw ex;
            }

            return Json(_CustomerContractTerms);
        }

        // POST: CustomerContractTerms/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerContractTerms>> Insert(CustomerContractTerms _CustomerContractTerms)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerContractTerms.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerContractTerms.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractTerms/Insert", _CustomerContractTerms);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<CustomerContractTerms>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error{ex.Message}");
            }
            return Ok(_CustomerContractTerms);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractTerms }, Total = 1 });
        }

        [HttpPut("{CenterCostId}")]
        public async Task<IActionResult> Update(Int64 CustomerContractTermsId, CustomerContractTerms _CustomerContractTerms)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerContractTerms/Update", _CustomerContractTerms);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<CustomerContractTerms>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractTerms }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractTerms>> Delete([FromBody]CustomerContractTerms _CustomerContractTerms)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractTerms/Delete", _CustomerContractTerms);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<CustomerContractTerms>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractTerms }, Total = 1 });
        }





    }
}