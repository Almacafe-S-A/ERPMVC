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
    public class ControlPalletsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ControlPalletsController(ILogger<ControlPalletsController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwControlPallets(Int64 Id=0)
        {
            ControlPalletsDTO _ControlPallets = new ControlPalletsDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPalletsDTO>(valorrespuesta);

                }

                if (_ControlPallets == null)
                {
                    _ControlPallets = new ControlPalletsDTO { DocumentDate=DateTime.Now     };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
           


            return PartialView(_ControlPallets);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ControlPallets> _ControlPallets = new List<ControlPallets>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPallets");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<List<ControlPallets>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ControlPallets.ToDataSourceResult(request);

        }


        public async Task<ActionResult<ControlPallets>> SaveControlPallets([FromBody]ControlPalletsDTO _ControlPalletsDTO)
        {

            try
            {
                if (_ControlPalletsDTO != null)
                {
                    ControlPallets _listControlPallets = new ControlPallets();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + _ControlPalletsDTO.ControlPalletsId);
                    string valorrespuesta = "";
                    _ControlPalletsDTO.FechaModificacion = DateTime.Now;
                    _ControlPalletsDTO.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);
                    }

                    if (_listControlPallets == null) { _listControlPallets = new ControlPallets(); }
                    if (_listControlPallets.ControlPalletsId == 0)
                    {
                        _ControlPalletsDTO.FechaCreacion = DateTime.Now;
                        _ControlPalletsDTO.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_ControlPalletsDTO);

                        foreach (var item in _ControlPalletsDTO._ControlPalletsLine)
                        {
                            var value = (insertresult.Result as ObjectResult).Value;

                            ControlPalletsLine _ControlPalletsLineResponse = new ControlPalletsLine();
                            item.ControlPalletsId = ((ControlPalletsDTO)(value)).ControlPalletsId;
                            // string baseadress = config.Value.urlbase;
                            HttpClient _client2 = new HttpClient();

                            _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                            var result2 = await _client2.PostAsJsonAsync(baseadress + "api/ControlPalletsLine/Insert", item);

                            valorrespuesta = "";
                            if (result2.IsSuccessStatusCode)
                            {
                                valorrespuesta = await (result2.Content.ReadAsStringAsync());
                                _ControlPalletsLineResponse = JsonConvert.DeserializeObject<ControlPalletsLine>(valorrespuesta);

                            }
                            else
                            {
                                string request = await result2.Content.ReadAsStringAsync();
                                return BadRequest(request);
                            }
                        }

                    }
                    else
                    {
                        //var updateresult = await Update(_ControlPalletsDTO.ControlPalletsId, _ControlPalletsDTO);
                    }
                }
                else
                {
                    return BadRequest("No llego correctamente el modelo!");
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ControlPalletsDTO);
        }

        // POST: ControlPallets/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ControlPalletsDTO>> Insert(ControlPalletsDTO _ControlPallets)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ControlPallets.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ControlPallets.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlPallets/Insert", _ControlPallets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPalletsDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_ControlPallets);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPallets }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ControlPallets>> Update(Int64 id, ControlPallets _ControlPallets)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ControlPallets/Update", _ControlPallets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPallets }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ControlPallets>> Delete([FromBody]ControlPallets _ControlPallets)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlPallets/Delete", _ControlPallets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


         
            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPallets }, Total = 1 });
        }





    }
}