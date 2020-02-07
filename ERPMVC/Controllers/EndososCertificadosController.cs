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
    public class EndososCertificadosController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EndososCertificadosController(ILogger<EndososCertificadosController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwEndosos([FromBody]EndososCertificados _EndososCertificadosp)
        {
            EndososDTO _EndososCertificados = new EndososDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetEndososCertificadosById/" + _EndososCertificadosp.EndososCertificadosId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificados = JsonConvert.DeserializeObject<EndososDTO>(valorrespuesta);

                }

                if (_EndososCertificados == null)
                {
                    _EndososCertificados = new EndososDTO { FechaOtorgado = DateTime.Now, DocumentDate = DateTime.Now, ExpirationDate = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EndososCertificados);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EndososCertificados> _EndososCertificados = new List<EndososCertificados>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetEndososCertificados");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificados = JsonConvert.DeserializeObject<List<EndososCertificados>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososCertificados.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetEndososCertificadosByIdCD([DataSourceRequest]DataSourceRequest request,[FromBody] EndososCertificados CD)
        {
            EndososCertificados _EndososCertificados = new EndososCertificados();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetEndososCertificadosByIdCD/"+CD.IdCD);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificados = JsonConvert.DeserializeObject<EndososCertificados>(valorrespuesta);

                }

                if (_EndososCertificados == null)
                {
                    _EndososCertificados = new EndososCertificados();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_EndososCertificados);

        }




        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetEndososSaldoByLineByIdCD([DataSourceRequest]DataSourceRequest request, [FromBody] EndososCertificadosLine ELiberacion)
        {
            EndososLiberacion _EndososLiberacion  = new EndososLiberacion();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososCertificados/GetEndososSaldoByLineByIdCD", ELiberacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<EndososLiberacion>(valorrespuesta);

                }

                if (_EndososLiberacion == null)
                {
                    _EndososLiberacion = new EndososLiberacion();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_EndososLiberacion);

        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EndososCertificados>> SaveEndososCertificados([FromBody]EndososDTO _EndososCertificados)
        {

            try
            {
                if (_EndososCertificados != null)
                {
                    EndososCertificados _listEndososCertificados = new EndososCertificados();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetEndososCertificadosById/" + _EndososCertificados.EndososCertificadosId);
                    string valorrespuesta = "";
                    _EndososCertificados.FechaModificacion = DateTime.Now;
                    _EndososCertificados.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listEndososCertificados = JsonConvert.DeserializeObject<EndososCertificados>(valorrespuesta);
                    }

                    if (_listEndososCertificados == null)
                    {
                        _listEndososCertificados = new EndososCertificados();
                    }

                    if (_listEndososCertificados.EndososCertificadosId == 0)
                    {
                        _EndososCertificados.FechaCreacion = DateTime.Now;
                        _EndososCertificados.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_EndososCertificados);
                        var value = (insertresult.Result as ObjectResult).Value;
                        EndososCertificados _EndososCertificado = ((EndososCertificados)(value));
                        if (_EndososCertificado.EndososCertificadosId <= 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero el documento!"));
                        }
                    }
                    else
                    {
                        var updateresult = await Update(_EndososCertificados.EndososCertificadosId, _EndososCertificados);
                    }
                }
                else
                {
                    return await Task.Run(()=> BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EndososCertificados);
        }

        // POST: EndososCertificados/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EndososCertificados>> Insert(EndososCertificados _EndososCertificados)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EndososCertificados.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EndososCertificados.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososCertificados/Insert", _EndososCertificados);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificados = JsonConvert.DeserializeObject<EndososCertificados>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EndososCertificados);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificados }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndososCertificados>> Update(Int64 id, EndososCertificados _EndososCertificados)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EndososCertificados/Update", _EndososCertificados);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificados = JsonConvert.DeserializeObject<EndososCertificados>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificados }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososCertificados>> Delete([FromBody]EndososCertificados _EndososCertificados)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososCertificados/Delete", _EndososCertificados);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificados = JsonConvert.DeserializeObject<EndososCertificados>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificados }, Total = 1 });
        }
        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> SFEndosos(Int32 id)
        {
            try
            {
                EndososCertificados _EndososCertificados = new EndososCertificados { EndososCertificadosId = id, };
                return await Task.Run(() => View(_EndososCertificados));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }
        }
    }
}