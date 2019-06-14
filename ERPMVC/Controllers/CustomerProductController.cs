using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
    public class CustomerProductController : Controller
    {
        //Variables y opciones
        private readonly IOptions<MyConfig> _config;
        private readonly IMapper mapper;
        private readonly ILogger _logger;

        public CustomerProductController(ILogger<CustomerProductController> logger, IOptions<MyConfig> config, IMapper mapper)
        {
            this._config = config;
            this.mapper = mapper;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: api/CustomerProduct
        [HttpGet]
        public async Task<DataSourceResult> GetCustomerProduct([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerProduct> _CustomerProduct = new List<CustomerProduct>();
            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerProduct/GetCustomerProduct");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerProduct = JsonConvert.DeserializeObject<List<CustomerProduct>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerProduct.ToDataSourceResult(request);
        }

        // GET: api/CustomerProduct/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CustomerProduct
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/CustomerProduct/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
