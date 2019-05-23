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
    public class SubProductController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public SubProductController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipo(Int64 ProductTypeId)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeId/"+ ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SubProducto);
        }

        
        [HttpGet("[action]")]
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipoByCustomer([DataSourceRequest]DataSourceRequest request, CustomerTypeSubProduct _CustomerTypeSubProduct)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
               // CustomerTypeSubProduct _CustomerTypeSubProduct = new CustomerTypeSubProduct();
                _CustomerTypeSubProduct.ProductTypeId= _CustomerTypeSubProduct.ProductTypeId ==0 ? 2 : _CustomerTypeSubProduct.ProductTypeId;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/GetSubProductoByTipoByCustomer", _CustomerTypeSubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_SubProducto.ToDataSourceResult(request));
           // return Json(_SubProducto);
        }





    }

    
}