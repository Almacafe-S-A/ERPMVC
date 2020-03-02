using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.Contexts;
using ERPMVC.Models;

using System.Net.Http;
using ERPMVC.Helpers;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ERPMVC.DTO;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HoursWorkedController : Controller
    {
        private readonly IOptions<MyConfig> _config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public HoursWorkedController(ILogger<HoursWorkedController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this._config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public ActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetHoursWorked([DataSourceRequest]DataSourceRequest request)
        {
            List<HoursWorked> _HoursWorked = new List<HoursWorked>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/HoursWorked/GetHoursWorked");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorked = JsonConvert.DeserializeObject<List<HoursWorked>>(valorrespuesta);
                    _HoursWorked = _HoursWorked.OrderByDescending(q => q.IdHorastrabajadas).ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _HoursWorked.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddHoursWorked([FromBody]HoursWorkedDTO _HoursWorked)
        {
            HoursWorkedDTO _HoursWorkedF = new HoursWorkedDTO();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();


                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/HoursWorked/GetHoursWorkedById/" + _HoursWorked.IdHorastrabajadas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorkedF = JsonConvert.DeserializeObject<HoursWorkedDTO>(valorrespuesta);
                }

                if (_HoursWorkedF == null)
                {
                    _HoursWorkedF = new HoursWorkedDTO
                    {
                        editar = _HoursWorked.editar,
                        IdHorastrabajadas = _HoursWorked.IdHorastrabajadas
                    };
                }
                _HoursWorkedF.editar = _HoursWorked.editar;
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return View(_HoursWorkedF);
        }
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<HoursWorked>> SaveHoursWorked([FromBody]HoursWorkedDTO _HoursWorked)
        {
            try
            {
                HoursWorked _listHoursWorked = new HoursWorked();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/HoursWorked/GetHoursWorkedById/" + _HoursWorked.IdHorastrabajadas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listHoursWorked = JsonConvert.DeserializeObject<HoursWorked>(valorrespuesta);
                }

                if (_listHoursWorked == null)
                {
                    _listHoursWorked = new HoursWorked();
                }

                if (_listHoursWorked.IdHorastrabajadas == 0)
                {
                    _HoursWorked.FechaCreacion = DateTime.Now;
                    _HoursWorked.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_HoursWorked);
                }
                else
                {
                    _HoursWorked.FechaCreacion = _listHoursWorked.FechaCreacion;
                    _HoursWorked.UsuarioCreacion = _listHoursWorked.UsuarioCreacion;
                    _HoursWorked.FechaModificacion = DateTime.Now;
                    _HoursWorked.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_HoursWorked.IdHorastrabajadas, _HoursWorked);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_HoursWorked);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<HoursWorked>> Insert(HoursWorked _HoursWorked)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _HoursWorked.UsuarioCreacion = HttpContext.Session.GetString("user");
                _HoursWorked.FechaCreacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/HoursWorked/Insert", _HoursWorked);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorked = JsonConvert.DeserializeObject<HoursWorked>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_HoursWorked);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HoursWorked>> Update(Int64 id, HoursWorked _HoursWorked)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _HoursWorked.FechaModificacion = DateTime.Now;
                _HoursWorked.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/HoursWorked/Update", _HoursWorked);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorked = JsonConvert.DeserializeObject<HoursWorked>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _HoursWorked }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<HoursWorked>> Delete([FromBody]HoursWorked _HoursWorked)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/HoursWorked/Delete", _HoursWorked);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorked = JsonConvert.DeserializeObject<HoursWorked>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _HoursWorked }, Total = 1 });
        }
    }
}
