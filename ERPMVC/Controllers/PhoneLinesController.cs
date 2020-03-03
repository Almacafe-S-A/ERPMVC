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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class PhoneLinesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PhoneLinesController(ILogger<PhoneLinesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [Authorize(Policy = "RRHH.Lineas de Telefono")]
        public IActionResult Index()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PhoneLines> _PhoneLines = new List<PhoneLines>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PhoneLines/GetPhoneLines");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLines = JsonConvert.DeserializeObject<List<PhoneLines>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => _PhoneLines.ToDataSourceResult(request));
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddPhoneLines([FromBody]PhoneLinesDTO _sarpara)
        {
            PhoneLinesDTO _PhoneLines = new PhoneLinesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PhoneLines/GetPhoneLinesById/" + _sarpara.PhoneLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLines = JsonConvert.DeserializeObject<PhoneLinesDTO>(valorrespuesta);
                }
                if (_PhoneLines == null)
                {
                    _PhoneLines = new PhoneLinesDTO { PhoneLineId = 0, IdBranch = Convert.ToInt32(HttpContext.Session.GetString("BranchId")) };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_PhoneLines);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<PhoneLinesDTO>> SavePhoneLines([FromBody]PhoneLinesDTO _PhoneLinesP)
        {
            PhoneLines _PhoneLines = _PhoneLinesP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PhoneLines/GetPhoneLinesById/" + _PhoneLines.PhoneLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLines = JsonConvert.DeserializeObject<PhoneLinesDTO>(valorrespuesta);
                }

                if (_PhoneLines == null) { _PhoneLines = new Models.PhoneLines(); }

                if (_PhoneLines.PhoneLineId == 0)
                {
                    _PhoneLinesP.FechaCreacion = DateTime.Now;
                    _PhoneLinesP.FechaModificacion = DateTime.Now;
                    _PhoneLinesP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _PhoneLinesP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    //var ValidacionEmpleadoresult = await ValidacionEmpleado(_PhoneLinesP);
                    //if ((ValidacionEmpleadoresult as ObjectResult).Value.ToString() == "Ya exíste una Línea de Teléfono creada con el mismo Empleado.")
                    //{
                    //    return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Empleado."));
                    //}
                    //var ValidacionCompanyPhoneresult = await ValidacionCompanyPhone(_PhoneLinesP);
                    //if ((ValidacionCompanyPhoneresult as ObjectResult).Value.ToString() == "Ya exíste una Línea de Teléfono creada con el mismo Número de Teléfono.")
                    //{
                    //    return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Número de Teléfono."));
                    //}
                    var insertresult = await Insert(_PhoneLinesP);
                }
                else
                {
                    _PhoneLinesP.FechaCreacion = _PhoneLines.FechaCreacion;
                    _PhoneLinesP.UsuarioCreacion = _PhoneLines.UsuarioCreacion;
                    _PhoneLinesP.FechaModificacion = DateTime.Now;
                    _PhoneLinesP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    //var ValidacionEmpleadoresult = await ValidacionEmpleado(_PhoneLinesP);
                    //if ((ValidacionEmpleadoresult as ObjectResult).Value.ToString() == "Ya exíste una Línea de Teléfono creada con el mismo Empleado.")
                    //{
                    //    return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Empleado."));
                    //}
                    //var ValidacionCompanyPhoneresult = await ValidacionCompanyPhone(_PhoneLinesP);
                    //if ((ValidacionCompanyPhoneresult as ObjectResult).Value.ToString() == "Ya exíste una Línea de Teléfono creada con el mismo Número de Teléfono.")
                    //{
                    //    return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Número de Teléfono."));
                    //}
                    var updateresult = await Update(_PhoneLines.PhoneLineId, _PhoneLinesP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_PhoneLines);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(PhoneLines _PhoneLinesP)
        {
            PhoneLines _PhoneLines = _PhoneLinesP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PhoneLines/Insert", _PhoneLinesP);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLinesP = JsonConvert.DeserializeObject<PhoneLines>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _PhoneLinesP }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, PhoneLines _PhoneLinesP)
        {
            PhoneLines _PhoneLines = _PhoneLinesP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/PhoneLines/Update", _PhoneLines);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLines = JsonConvert.DeserializeObject<PhoneLines>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _PhoneLines }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<PhoneLines>> Delete(Int64 Id, [FromBody]PhoneLines _PhoneLinesP)
        {
            PhoneLines _PhoneLines = _PhoneLinesP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PhoneLines/Delete", _PhoneLines);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLines = JsonConvert.DeserializeObject<PhoneLines>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _PhoneLines }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> SFPhoneLines()
        {
            return await Task.Run(() => View());
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> TasaDeCambioDolar([FromBody]ExchangeRate _ExchangeRateP)
        {
            ExchangeRate _ExchangeRate = new ExchangeRate();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ExchangeRate.CurrencyId = _ExchangeRateP.CurrencyId;
                _ExchangeRate.DayofRate = DateTime.Now;
                _ExchangeRate.ExchangeRateValue = 0;
                _ExchangeRate.ExchangeRateId = 0;
                _ExchangeRate.CreatedDate = DateTime.Now;
                _ExchangeRate.ModifiedDate = DateTime.Now;
                _ExchangeRate.CreatedUser = HttpContext.Session.GetString("user");
                _ExchangeRate.ModifiedUser = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ExchangeRate/GetExchangeRateByFecha/ExchangeRate", _ExchangeRate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_ExchangeRate));
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionEmpleado([FromBody]PhoneLines _PhoneLines)
        {
            List<PhoneLines> _PhoneLinesList = new List<PhoneLines>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PhoneLines/GetPhoneLines");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLinesList = JsonConvert.DeserializeObject<List<PhoneLines>>(valorrespuesta);
                    if (_PhoneLines.PhoneLineId != 0)
                    {
                        _PhoneLines = _PhoneLinesList.Where(q => q.IdEmpleado == _PhoneLines.IdEmpleado && q.PhoneLineId != _PhoneLines.PhoneLineId).FirstOrDefault();
                        if (_PhoneLines != null)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Empleado."));
                        }
                    }
                    else
                    {
                        _PhoneLines = _PhoneLinesList.Where(q => q.IdEmpleado == _PhoneLines.IdEmpleado).FirstOrDefault();
                        if (_PhoneLines != null)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Empleado."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_PhoneLinesList));
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionCompanyPhone([FromBody]PhoneLines _PhoneLines)
        {
            List<PhoneLines> _PhoneLinesList = new List<PhoneLines>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PhoneLines/GetPhoneLines");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PhoneLinesList = JsonConvert.DeserializeObject<List<PhoneLines>>(valorrespuesta);
                    if (_PhoneLines.PhoneLineId != 0)
                    {
                        _PhoneLines = _PhoneLinesList.Where(q => q.CompanyPhone == _PhoneLines.CompanyPhone && q.PhoneLineId != _PhoneLines.PhoneLineId).FirstOrDefault();
                        if (_PhoneLines != null)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Número de Teléfono."));
                        }
                    }
                    else
                    {
                        _PhoneLines = _PhoneLinesList.Where(q => q.CompanyPhone == _PhoneLines.CompanyPhone).FirstOrDefault();
                        if (_PhoneLines != null)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Línea de Teléfono creada con el mismo Número de Teléfono."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_PhoneLinesList));
        }
    }
}