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
    public class TaxController : Controller
    {

         private readonly IOptions<MyConfig> _config;

        public TaxController(IOptions<MyConfig> config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetTaxes([DataSourceRequest]DataSourceRequest request)
        {
            List<Tax> _Taxes = new List<Tax>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTaxes");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Taxes = JsonConvert.DeserializeObject<List<Tax>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_Taxes.ToDataSourceResult(request));

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<Tax>> GetTaxById([DataSourceRequest]DataSourceRequest request,Int64 TaxId)
        {
            Tax _Taxes = new Tax();
            try
            {
                
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Tax/GetTaxById/"+TaxId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Taxes = JsonConvert.DeserializeObject<Tax>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            
            return _Taxes;

        }





    }
}