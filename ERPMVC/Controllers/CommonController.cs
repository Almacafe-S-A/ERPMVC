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
    public class CommonController : Controller
    {
           private readonly IOptions<MyConfig> _config;


        public CommonController(IOptions<MyConfig> config)
        {
            _config = config;
        }

        [HttpPost("[controller]/[action]")]
        public  ActionResult ClearSession([FromBody]List<string> sesiones)
        {
            foreach (var item in sesiones)
            {
                HttpContext.Session.SetString(item, "");
            }
           
            return Ok();
        }
           
        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetCustomer([DataSourceRequest]DataSourceRequest request)
        {
             List<Customer> _clientes = new List<Customer>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomer");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<Customer>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_clientes.ToDataSourceResult(request));

        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetProduct([DataSourceRequest]DataSourceRequest request)
        {
            List<Product> _clientes = new List<Product>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Product/GetProduct");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<Product>>(valorrespuesta);
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