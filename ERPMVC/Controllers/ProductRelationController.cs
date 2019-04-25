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
    public class ProductRelationController : Controller
    {
        private readonly IOptions<MyConfig> _config;

        public ProductRelationController(IOptions<MyConfig> config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }


        // [HttpGet("[controller]/[action]/{ProductId}")]
        // [HttpPost("[controller]/[action]")]
        //public async Task<ActionResult>   GetSubProductByProductId[DataSourceRequest] DataSourceRequest request,[FromBody]ProductRelation ProductId)
        public async Task<ActionResult> GetSubProductByProductId(Int64 ProductId)
        {
            List<SubProduct> _clientes = new List<SubProduct>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetSubProductByProductId/" + ProductId);
                //  var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetSubProductByProductId/" + ProductId.ProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_clientes);//.ToDataSourceResult(request));

        }




    }
}