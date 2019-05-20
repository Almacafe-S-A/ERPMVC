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
    public class CAIController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public CAIController(ILogger<CAIController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CAI> _cais = new List<CAI>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAI");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                  _cais  = JsonConvert.DeserializeObject<List<CAI>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


             return Json(_cais.ToDataSourceResult(request));

        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<CAI> _cais = new List<CAI>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CAI/GetCAI");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<CAI>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_cais);

        }


        //--------------------------------------------------------------------------------------
        // POST: CAI/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(CAI _CAIp)
        {
            CAI _CAI = _CAIp;
            try
            {                          
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CAI.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CAI.UsuarioModificacion = HttpContext.Session.GetString("user");
                _CAI.FechaCreacion = DateTime.Now;
                _CAI.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Insert", _CAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CAI }, Total = 1 });
        }
                     
        [HttpPost]
        public async Task<IActionResult> Update( CAI _CAIp)
        {
            CAI _CAI = _CAIp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CAI.FechaModificacion = DateTime.Now;
                _CAI.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Update", _CAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {                    
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CAI }, Total = 1 });
        }           

        [HttpDelete("{IdCAI}")]
        public async Task<ActionResult<CAI>> Delete(Int64 IdCAI, CAI _CAIp)
        {
            CAI _CAI = _CAIp;
            try   
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CAI/Delete", _CAI);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CAI = JsonConvert.DeserializeObject<CAI>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CAI }, Total = 1 });
        }






    }
}