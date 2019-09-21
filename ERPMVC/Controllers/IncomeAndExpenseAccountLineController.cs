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
    public class IncomeAndExpenseAccountLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public IncomeAndExpenseAccountLineController(ILogger<IncomeAndExpenseAccountLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwIncomeAndExpenseAccountLine(Int64 Id = 0)
        {
            IncomeAndExpenseAccountLine _IncomeAndExpenseAccountLine = new IncomeAndExpenseAccountLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/IncomeAndExpenseAccountLine/GetIncomeAndExpenseAccountLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpenseAccountLine = JsonConvert.DeserializeObject<IncomeAndExpenseAccountLine>(valorrespuesta);

                }

                if (_IncomeAndExpenseAccountLine == null)
                {
                    _IncomeAndExpenseAccountLine = new IncomeAndExpenseAccountLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_IncomeAndExpenseAccountLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<IncomeAndExpenseAccountLine> _IncomeAndExpenseAccountLine = new List<IncomeAndExpenseAccountLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/IncomeAndExpenseAccountLine/GetIncomeAndExpenseAccountLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpenseAccountLine = JsonConvert.DeserializeObject<List<IncomeAndExpenseAccountLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _IncomeAndExpenseAccountLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IncomeAndExpenseAccountLine>> SaveIncomeAndExpenseAccountLine([FromBody]IncomeAndExpenseAccountLine _IncomeAndExpenseAccountLine)
        {

            try
            {
                IncomeAndExpenseAccountLine _listIncomeAndExpenseAccountLine = new IncomeAndExpenseAccountLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/IncomeAndExpenseAccountLine/GetIncomeAndExpenseAccountLineById/" + _IncomeAndExpenseAccountLine.IncomeAndExpenseAccountLineId);
                string valorrespuesta = "";
                _IncomeAndExpenseAccountLine.FechaModificacion = DateTime.Now;
                _IncomeAndExpenseAccountLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listIncomeAndExpenseAccountLine = JsonConvert.DeserializeObject<IncomeAndExpenseAccountLine>(valorrespuesta);
                }

                if (_listIncomeAndExpenseAccountLine.IncomeAndExpenseAccountLineId == 0)
                {
                    _IncomeAndExpenseAccountLine.FechaCreacion = DateTime.Now;
                    _IncomeAndExpenseAccountLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_IncomeAndExpenseAccountLine);
                }
                else
                {
                    var updateresult = await Update(_IncomeAndExpenseAccountLine.IncomeAndExpenseAccountLineId, _IncomeAndExpenseAccountLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_IncomeAndExpenseAccountLine);
        }

        // POST: IncomeAndExpenseAccountLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IncomeAndExpenseAccountLine>> Insert(IncomeAndExpenseAccountLine _IncomeAndExpenseAccountLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _IncomeAndExpenseAccountLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                _IncomeAndExpenseAccountLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/IncomeAndExpenseAccountLine/Insert", _IncomeAndExpenseAccountLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpenseAccountLine = JsonConvert.DeserializeObject<IncomeAndExpenseAccountLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_IncomeAndExpenseAccountLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _IncomeAndExpenseAccountLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IncomeAndExpenseAccountLine>> Update(Int64 id, IncomeAndExpenseAccountLine _IncomeAndExpenseAccountLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/IncomeAndExpenseAccountLine/Update", _IncomeAndExpenseAccountLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpenseAccountLine = JsonConvert.DeserializeObject<IncomeAndExpenseAccountLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _IncomeAndExpenseAccountLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IncomeAndExpenseAccountLine>> Delete([FromBody]IncomeAndExpenseAccountLine _IncomeAndExpenseAccountLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/IncomeAndExpenseAccountLine/Delete", _IncomeAndExpenseAccountLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpenseAccountLine = JsonConvert.DeserializeObject<IncomeAndExpenseAccountLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _IncomeAndExpenseAccountLine }, Total = 1 });
        }





    }
}