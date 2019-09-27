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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class TrialBalanceController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;

        public TrialBalanceController(IHostingEnvironment hostingEnvironment
            , ILogger<TrialBalanceController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }


        public async Task<IActionResult> Index()
        {

            List<AccountingDTO> _accounting = new List<AccountingDTO>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TrialBalance/TrialBalanceRes");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _accounting = JsonConvert.DeserializeObject<List<AccountingDTO>>(valorrespuesta);

                }

                if (_accounting == null)
                {
                    _accounting = new List<AccountingDTO>();
                }

                ViewBag.Tree = _accounting;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(()=> View());
        }


        public async Task<JsonResult> TrialBalanceRes([DataSourceRequest]DataSourceRequest request)
        {
            List<AccountingDTO> _accounting = new List<AccountingDTO>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.GetAsync(baseadress + "api/TrialBalance/TrialBalanceRes");
                var result = await _client.GetAsync(baseadress + "api/TrialBalance/GetAccounting");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _accounting = JsonConvert.DeserializeObject<List<AccountingDTO>>(valorrespuesta);

                }

                if (_accounting == null)
                {
                    _accounting = new List<AccountingDTO>();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


           // return Json(_accounting, JsonRequestBehavior.AllowGet);
            return Json(_accounting.ToTreeDataSourceResult(request) );

        }



    }
}