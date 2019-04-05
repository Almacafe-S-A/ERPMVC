using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class SalesOrderController : Controller
    {
        //private readonly ApplicationDbContext _context;
       //  private readonly ILogger _logger;
         private readonly IOptions<MyConfig> _config;

        //public SalesOrderController(ILogger<SalesOrderController> logger,IOptions<MyConfig> config)
         public SalesOrderController(IOptions<MyConfig> config)
        {
           // _logger = logger;
            _config = config;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> pvwSalesOrder()
        {
            // SalesOrder _salesorder = 

            return View();
        }

       
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetSalesOrder([DataSourceRequest]DataSourceRequest request)
        {
            List<SalesOrder> _SalesOrders = new List<SalesOrder>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrder");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrder>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
              // _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }


            return _SalesOrders.ToDataSourceResult(request);
        }


       [HttpPost("[action]")]
        public async Task<ActionResult<ApplicationUserRole>> Insert(ApplicationUserRole _role)
        {
            try
            {

                if (_role.RoleId != "" && _role.UserId != "" && _role.RoleId != null && _role.UserId != null)
                {
                    // TODO: Add insert logic here
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                        var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Insert", _role);

                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _role = JsonConvert.DeserializeObject<ApplicationUserRole>(valorrespuesta);

                        }
                        else
                        {
                            string request = await result.Content.ReadAsStringAsync();
                            return BadRequest(request);
                        }

                  
                }
                else
                {
                    return BadRequest("Ingrese todos los datos!");
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

              return Ok("Datos Guardados Correctamente! ");
           // return new ObjectResult(new DataSourceResult { Data = new[] { _role }, Total = 1 });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ApplicationUserRole>> Update(string Id, ApplicationUserRole _rol)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/SalesOrder/Update", _rol);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _rol = JsonConvert.DeserializeObject<ApplicationUserRole>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
               // _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ApplicationRole>> Delete([FromBody]ApplicationUserRole _rol)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Delete", _rol);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _rol = JsonConvert.DeserializeObject<ApplicationUserRole>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
              //  _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


          //  return new ObjectResult(new DataSourceResult { Data = new[] { RoleId }, Total = 1 });
            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });
        }




    }
}