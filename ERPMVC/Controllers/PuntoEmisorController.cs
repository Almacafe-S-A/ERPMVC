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
    public class PuntoEmisorController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PuntoEmisorController(ILogger<CAIController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: PuntoEmisor
        public ActionResult Index()
        {
            return View();
        }

        // GET: PuntoEmisor
        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PuntoEmisor> _cais = new List<PuntoEmisor>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PuntoEmisor/GetCAI");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<PuntoEmisor>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _cais.ToDataSourceResult(request);

        }

        // POST: CAI/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(PuntoEmisor _PuntoEmisorp)
        {
            PuntoEmisor _PuntoEmisor = _PuntoEmisorp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PuntoEmisor.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PuntoEmisor.UsuarioModificacion = HttpContext.Session.GetString("user");
                _PuntoEmisor.FechaCreacion = DateTime.Now;
                _PuntoEmisor.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Insert", _PuntoEmisor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmisor = JsonConvert.DeserializeObject<PuntoEmisor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PuntoEmisor }, Total = 1 });
        }


        // POST: PuntoEmisor/Update
        [HttpPost]
        public async Task<IActionResult> Update(PuntoEmisor _PuntoEmisorp)
        {
            PuntoEmisor _PuntoEmisor = _PuntoEmisorp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PuntoEmisor.FechaModificacion = DateTime.Now;
                _PuntoEmisor.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Update", _PuntoEmisor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmisor = JsonConvert.DeserializeObject<PuntoEmisor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PuntoEmisor }, Total = 1 });
        }



        // GET: PuntoEmisor/Delete
        [HttpDelete("[action]")]
        public async Task<ActionResult<PuntoEmisor>> Delete(PuntoEmisor _PuntoEmisorp)
        {
            PuntoEmisor _PuntoEmisor = _PuntoEmisorp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Delete", _PuntoEmisor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmisor = JsonConvert.DeserializeObject<PuntoEmisor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PuntoEmisor }, Total = 1 });
        }

       
    }
}