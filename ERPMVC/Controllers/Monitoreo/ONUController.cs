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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ONUController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ONUController(ILogger<ControlPalletsController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<DataSourceResult>> GetONUPersonByName([DataSourceRequest]DataSourceRequest request
           , INDIVIDUALM _sdnListSdnEntryM)
        {
            List<INDIVIDUALM> _sdnListSdnEntryMlist = new List<INDIVIDUALM>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ONU/GetPersonByName", _sdnListSdnEntryM);
                string valorrespuesta = "";
                // _ControlPalletsDTO.FechaModificacion = DateTime.Now;
                // ControlPalletsDTO.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _sdnListSdnEntryMlist = JsonConvert.DeserializeObject<List<INDIVIDUALM>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return _sdnListSdnEntryMlist.ToDataSourceResult(request);
            //return Json(_sdnListSdnEntryMlist);
        }


        public IActionResult ONULISTFind()
        {
            return PartialView();
        }


    }
}