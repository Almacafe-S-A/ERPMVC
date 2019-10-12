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
    public class DebitNoteLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public DebitNoteLineController(ILogger<DebitNoteLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwDebitNoteLine(Int64 Id = 0)
        {
            DebitNoteLine _DebitNoteLine = new DebitNoteLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);

                }

                if (_DebitNoteLine == null)
                {
                    _DebitNoteLine = new DebitNoteLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_DebitNoteLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<DebitNoteLine> _DebitNoteLine = new List<DebitNoteLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<List<DebitNoteLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _DebitNoteLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DebitNoteLine>> SaveDebitNoteLine([FromBody]DebitNoteLine _DebitNoteLine)
        {

            try
            {
                DebitNoteLine _listDebitNoteLine = new DebitNoteLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLineById/" + _DebitNoteLine.DebitNoteLineId);
                string valorrespuesta = "";
               // _DebitNoteLine.FechaModificacion = DateTime.Now;
               // _DebitNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listDebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

                if (_listDebitNoteLine == null) { _listDebitNoteLine = new DebitNoteLine(); }

                if (_listDebitNoteLine.DebitNoteLineId == 0)
                {
                   // _DebitNoteLine.FechaCreacion = DateTime.Now;
                   // _DebitNoteLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DebitNoteLine);
                }
                else
                {
                    var updateresult = await Update(_DebitNoteLine.DebitNoteLineId, _DebitNoteLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_DebitNoteLine);
        }

        // POST: DebitNoteLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<DebitNoteLine>> Insert(DebitNoteLine _DebitNoteLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _DebitNoteLine.UsuarioCreacion = HttpContext.Session.GetString("user");
               // _DebitNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/DebitNoteLine/Insert", _DebitNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_DebitNoteLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNoteLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DebitNoteLine>> Update(Int64 id, DebitNoteLine _DebitNoteLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/DebitNoteLine/Update", _DebitNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=>BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_DebitNoteLine);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNoteLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DebitNoteLine>> Delete([FromBody]DebitNoteLine _DebitNoteLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/DebitNoteLine/Delete", _DebitNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNoteLine }, Total = 1 });
        }





    }
}