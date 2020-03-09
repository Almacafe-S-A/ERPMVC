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
    public class PrecioCafesController : Controller
    {


        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PrecioCafesController    (ILogger<PrecioCafesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        public IActionResult PrecioCafe()
        {
            return View();
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PrecioCafe> _PrecioCafe = new List<PrecioCafe>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PrecioCafes/GetPrecioCafe");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PrecioCafe = JsonConvert.DeserializeObject<List<PrecioCafe>>(valorrespuesta);
                    _PrecioCafe = _PrecioCafe.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PrecioCafe.ToDataSourceResult(request);

        }

        [HttpPost("savepreciocafes")]
        public async Task<ActionResult<PrecioCafe>> savepreciocafes(PrecioCafeDTO _Preciocafes)
        {
            try
            {
                

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Preciocafes.FechaCreacion = DateTime.Now;
                _Preciocafes.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Preciocafes.FechaModificacion = DateTime.Now;
                _Preciocafes.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/PrecioCafes/Insert", _Preciocafes);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Preciocafes = JsonConvert.DeserializeObject<PrecioCafeDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Preciocafes);
        }


        [HttpPost("Update")]
        public async Task<ActionResult<PrecioCafe>> Update(/*Int64 Id,*/ PrecioCafeDTO _Preciocafes)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _Preciocafes.FechaCreacion = DateTime.Now;
                _Preciocafes.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Preciocafes.FechaModificacion = DateTime.Now;
                _Preciocafes.UsuarioModificacion = HttpContext.Session.GetString("user");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/PrecioCafes/Update", _Preciocafes);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Preciocafes = JsonConvert.DeserializeObject<PrecioCafeDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Preciocafes }, Total = 1 });

        }

        //[HttpPost("Delete")]
        [AcceptVerbs("Post")]
        public async Task<ActionResult<PrecioCafe>> Delete(PrecioCafe _PrecioCafe)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PrecioCafes/Delete", _PrecioCafe);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PrecioCafe = JsonConvert.DeserializeObject<PrecioCafe>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _PrecioCafe }, Total = 1 });
        }
    }

   
}