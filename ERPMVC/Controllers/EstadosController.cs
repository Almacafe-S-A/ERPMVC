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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{

     [Authorize]
      [CustomAuthorization]
    public class EstadosController : Controller
    {
        private readonly IOptions<MyConfig> _config;

        public EstadosController(IOptions<MyConfig> config)
        {
            this._config = config;
          
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Estados> _customers = new List<Estados>();
            string baseadress = _config.Value.urlbase;
            HttpClient _client = new HttpClient();
              _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + "api/estados");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                _customers = JsonConvert.DeserializeObject<List<Estados>>(valorrespuesta);

            }


           return Json(_customers); 

        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetEnviosCotizacion([DataSourceRequest]DataSourceRequest request)
        {
            List<Estados> _clientes = new List<Estados>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/estados/GetEstadosByGrupo/"+2);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<Estados>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_clientes.ToDataSourceResult(request));
        }


    }
}