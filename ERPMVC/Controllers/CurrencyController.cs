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
    public class CurrencyController : Controller
    {

        private readonly IOptions<MyConfig> _config;

        public CurrencyController(IOptions<MyConfig> config)
        {
            _config = config;
        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetCurrency([DataSourceRequest]DataSourceRequest request)
        {
            List<Currency> _Currency = new List<Currency>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Currency/GetCurrency");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Currency = JsonConvert.DeserializeObject<List<Currency>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_Currency.ToDataSourceResult(request));

        }



    }
}