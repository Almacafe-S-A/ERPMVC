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
    public class BranchController : Controller
    {
        private readonly IOptions<MyConfig> config;

        public BranchController(IOptions<MyConfig> config)
        {
            this.config = config;

        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Branch> _customers = new List<Branch>();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
              _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + "api/Branch/GetBranch");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                _customers = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta);

            }


               return Json(_customers.ToDataSourceResult(request));

        }
    }
}