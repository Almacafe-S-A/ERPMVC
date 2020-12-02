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
    [Route("api/[controller]")]
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class MotivoConciliacionController : ControllerBase
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public MotivoConciliacionController(ILogger<MotivoConciliacion> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetMotivosConciliacion([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<MotivoConciliacion> motivos = new List<MotivoConciliacion>();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/MotivoConciliacion/GetMotivosConciliacion");
                if (result.IsSuccessStatusCode)
                {
                    string valorrespuesta = await (result.Content.ReadAsStringAsync());
                    motivos = JsonConvert.DeserializeObject<List<MotivoConciliacion>>(valorrespuesta);
                }
                return motivos.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
        }
    }
}