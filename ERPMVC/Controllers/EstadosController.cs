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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{

     [Authorize]
      [CustomAuthorization]
    public class EstadosController : Controller
    {
        private readonly IOptions<MyConfig> config;

        public EstadosController(IOptions<MyConfig> config)
        {
            this.config = config;
          
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Estados> _customers = new List<Estados>();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            var result = await _client.GetAsync(baseadress + "api/estados");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                _customers = JsonConvert.DeserializeObject<List<Estados>>(valorrespuesta);

            }


           return Json(_customers); 

        }


    }
}