using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.DTO;
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
    public class ElementoConfiguracionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ElementoConfiguracionController(ILogger<ElementoConfiguracionController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult ElementoConfiguracion()
        {
            return View();
        }

        [HttpPost("[action]")]
        //public async Task<ActionResult> pvwElementoConfiguracion(Int64 Id = 0)
        public async Task<ActionResult> pvwElementoConfiguracion([FromBody]ElementoConfiguracionDTO _sarpara)
        {
            //ElementoConfiguracion _ElementoConfiguracion = new ElementoConfiguracion();
            ElementoConfiguracionDTO _ElementoConfiguracion = new ElementoConfiguracionDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracionDTO>(valorrespuesta);
                   // _ElementoConfiguracion.Estado = _ElementoConfiguracion.Estado=="A"?"Activo":"Inactivo";
                }

                if (_ElementoConfiguracion == null)
                {
                    _ElementoConfiguracion = new ElementoConfiguracionDTO { FechaCreacion = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ElementoConfiguracion);

        }
        
        public async Task<ActionResult> GetElementoConfiguracionById(long Id)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/ElementoConfiguracion/GetElementoConfiguracionById/"+Id);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ElementoConfiguracion>(contenido);
                    return Ok(resultado);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al cargar elemento configuracion por Id");
                return BadRequest(ex);
            }
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetElementoByIdConfiguracion([DataSourceRequest]DataSourceRequest request,Int64 Id,string Estado="Activo")
        {
            List<ElementoConfiguracion> _ElementoConfiguracion = new List<ElementoConfiguracion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionByIdConfiguracion/"+Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);
                    _ElementoConfiguracion = _ElementoConfiguracion.Where(q => q.IdEstado == 1).ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ElementoConfiguracion.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ElementoConfiguracion> _ElementoConfiguracion = new List<ElementoConfiguracion>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);
                    _ElementoConfiguracion = _ElementoConfiguracion.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ElementoConfiguracion.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetElementoByGrupoConfiguracion([DataSourceRequest]DataSourceRequest request, int GrupoConfiguracionId)
        {
            List<ElementoConfiguracion> _ElementosConfiguracion = new List<ElementoConfiguracion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementosConfiguracionByGrupoConfiguracion/" + GrupoConfiguracionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementosConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ElementosConfiguracion.ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetElementoByGrupoConfiguracionV2(int GrupoConfiguracionId)
        {
            List<ElementoConfiguracion> _ElementosConfiguracion = new List<ElementoConfiguracion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementosConfiguracionByGrupoConfiguracion/" + GrupoConfiguracionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementosConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Ok(_ElementosConfiguracion);
        }

        //public async Task<ActionResult<ElementoConfiguracion>> SaveElementoConfiguracion([FromBody]ElementoConfiguracion _ElementoConfiguracion)
        //[HttpPost("[action]")]
        //public async Task<ActionResult<ElementoConfiguracion>> SaveProduct([FromBody]ElementoConfiguracionDTO _ElementoConfiguracionS)
        //{

        //    try
        //    {
        //        ElementoConfiguracion _listElementoConfiguracion = new ElementoConfiguracion();
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + _ElementoConfiguracionS.Id);
        //        string valorrespuesta = "";
        //        _ElementoConfiguracion.FechaModificacion = DateTime.Now;
        //        _ElementoConfiguracion.UsuarioModificacion = HttpContext.Session.GetString("user");
        //        if (result.IsSuccessStatusCode)
        //        {

        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _listElementoConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta);


        //        }

        //        if (_listElementoConfiguracion.Id == 0)
        //        {
        //            _ElementoConfiguracion.FechaCreacion = DateTime.Now;
        //            _ElementoConfiguracion.UsuarioCreacion = HttpContext.Session.GetString("user");
        //            var insertresult = await Insert(_ElementoConfiguracion);
        //        }
        //        else
        //        {
        //            var updateresult = await Update(_ElementoConfiguracion.Id, _ElementoConfiguracion);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    return Json(_ElementoConfiguracion);
        //}

        [HttpPost("[action]")]
        public async Task<ActionResult<ElementoConfiguracion>> SaveElementoConfiguracion([FromBody]ElementoConfiguracionDTO _ElementoConfiguracionS)
        {
            ElementoConfiguracion _ElementoConfiguracion = _ElementoConfiguracionS;

            try
            {
                ElementoConfiguracion _listElementoConfiguracion = new ElementoConfiguracion();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElemntoConfiguracionByNombre/" + _ElementoConfiguracion.Nombre + "/" + _ElementoConfiguracion.Idconfiguracion);
                string valorrespuesta1 = "";

                if (result1.IsSuccessStatusCode)
                {

                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracionDTO>(valorrespuesta1);
                }

                if (_ElementoConfiguracion == null) { _ElementoConfiguracion = new Models.ElementoConfiguracion(); }

                if (_ElementoConfiguracion.Id > 0)
                {
                    if (_ElementoConfiguracion.Id != _ElementoConfiguracionS.Id)
                        return await Task.Run(() => BadRequest($"Ya existe un Elemento registrado con ese nombre."));
                }

                if (_ElementoConfiguracionS.Id == 0)
                {
                    _ElementoConfiguracionS.FechaCreacion = DateTime.Now;
                    _ElementoConfiguracionS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ElementoConfiguracionS);
                }
                else
                {
                    //_ElementoConfiguracionS.UsuarioCreacion = _ElementoConfiguracion.UsuarioCreacion;
                    //_ElementoConfiguracionS.FechaCreacion = _ElementoConfiguracion.FechaCreacion;
                    //var updateresult = await Update(_ElementoConfiguracion.Id, _ElementoConfiguracionS);
                    var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + _ElementoConfiguracionS.Id);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracionDTO>(valorrespuesta);
                    }
                    _ElementoConfiguracion.FechaCreacion = _ElementoConfiguracionS.FechaCreacion;
                    _ElementoConfiguracion.UsuarioCreacion = _ElementoConfiguracionS.UsuarioCreacion;
                    _ElementoConfiguracion.UsuarioModificacion = _ElementoConfiguracionS.UsuarioModificacion;
                    _ElementoConfiguracion.FechaModificacion = _ElementoConfiguracionS.FechaModificacion;
                    var updateresult = await Update(_ElementoConfiguracion.Id, _ElementoConfiguracionS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ElementoConfiguracion);
        }

        // POST: ElementoConfiguracion/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ElementoConfiguracion>> Insert(ElementoConfiguracion _ElementoConfiguracion)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ElementoConfiguracion.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ElementoConfiguracion.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ElementoConfiguracion/Insert", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ElementoConfiguracion }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ElementoConfiguracion>> Update(Int64 id, ElementoConfiguracion _ElementoConfiguracion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ElementoConfiguracion/Update", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ElementoConfiguracion }, Total = 1 });
        }

        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, ElementoConfiguracion _ElementoConfiguracion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ElementoConfiguracion/Update", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Json(new[] { _ElementoConfiguracion }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult<ElementoConfiguracion>> Delete(Int64 Id, ElementoConfiguracion _ElementoConfiguracionP)
        {
            ElementoConfiguracion _ElementoConfiguracion = _ElementoConfiguracionP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ElementoConfiguracion/Delete", _ElementoConfiguracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ElementoConfiguracion = JsonConvert.DeserializeObject<ElementoConfiguracion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ElementoConfiguracion }, Total = 1 });
        }

        public async Task<ActionResult> ActualizarElementoConfiguracionValorDecimal(long id, double valor)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/ElementoConfiguracion/UpdateElementoConfiguracionValorDecimal/{id}/{valor}/{HttpContext.Session.GetString("user")}",null);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ElementoConfiguracion>(contenido);
                    return Ok(resultado);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al actualizar el elemento configuración " + id);
                return BadRequest(ex);
            }
        }

        public IActionResult ConfiguracionRap()
        {
            return View("ConfiguracionRAP");
        }

        public IActionResult ConfiguracionIHSS()
        {
            return View("ConfiguracionIHSS");
        }

    }
}