using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class DashBoardController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public DashBoardController(ILogger<ConditionsController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpPost]
        public async  Task<ActionResult> FacturacionMes(Fechas _Fecha)
        {
            List<FacturacionMensual> _Facturacionmensual = new List<FacturacionMensual>();
            try
            {
                string baseadress = config.Value.urlbase;
             
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/DashBoard/FacturacionMes", _Fecha);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Facturacionmensual = JsonConvert.DeserializeObject<List<FacturacionMensual>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
         

          

            return Json(_Facturacionmensual);
            
        }



    }
}