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
    public class StateController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public StateController(ILogger<StateController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult State()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddState([FromBody]StateDTO _sarpara)
        {
            StateDTO _State = new StateDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/State/GetStateById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _State = JsonConvert.DeserializeObject<StateDTO>(valorrespuesta);

                }

                if (_State == null)
                {
                    _State = new StateDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_State);

        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<State> _State = new List<State>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/State/GetState");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _State = JsonConvert.DeserializeObject<List<State>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _State.ToDataSourceResult(request);

        }


        [HttpPost]
        public async Task<ActionResult<State>> SaveState([FromBody]StateDTO _StateS)
        {

            State _State = _StateS;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/State/GetStateById/" + _State.Id);
                string valorrespuesta = "";
                _State.FechaModificacion = DateTime.Now;
                _State.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _State = JsonConvert.DeserializeObject<StateDTO>(valorrespuesta);
                }

                if (_State == null) { _State = new Models.State(); }

                if (_StateS.Id == 0)
                {
                    //_CAI.FechaCreacion = DateTime.Now;
                    //_CAI.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_StateS);
                }
                else
                {
                    _StateS.Usuariocreacion = _State.Usuariocreacion;
                    _StateS.FechaCreacion = _State.FechaCreacion;
                    var updateresult = await Update(_State.Id, _StateS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_State);
        }

        // POST: State/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<State>> Insert(State _State)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _State.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_State.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/State/Insert", _State);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _State = JsonConvert.DeserializeObject<State>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_State);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _State }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<State>> Update(Int64 id, State _State)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/State/Update", _State);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _State = JsonConvert.DeserializeObject<State>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _State }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<State>> Delete(Int64 Id, State _Statep)
        {
            State _State = _Statep;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/State/Delete", _State);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _State = JsonConvert.DeserializeObject<State>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _State }, Total = 1 });
        }





    }
}