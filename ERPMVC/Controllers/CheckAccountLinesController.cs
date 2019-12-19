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
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CheckAccountLinesController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CheckAccountLinesController(ILogger<CheckAccountController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCheckAccountLinesByCheckAccountId([DataSourceRequest]DataSourceRequest request, int CheckAccountId)
        {
            List<CheckAccountLines> _CheckAccountLines = new List<CheckAccountLines>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccountLines/GetCheckAccountLinesByCheckAccountId/" + CheckAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountLines = JsonConvert.DeserializeObject<List<CheckAccountLines>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CheckAccountLines.ToDataSourceResult(request);

        }
    }
}