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
    public class CustomerTypeController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public CustomerTypeController(ILogger<CustomerTypeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;

        }

        public ActionResult CustomerType()
        {
            return View();
        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerType> _CustomerType = new List<CustomerType>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerType/Get");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerType = JsonConvert.DeserializeObject<List<CustomerType>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_CustomerType.ToDataSourceResult(request));

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(CustomerType _CustomerTypep)
        {
            CustomerType _CustomerType = _CustomerTypep;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerType.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerType.UsuarioModificacion = HttpContext.Session.GetString("user");
                _CustomerType.FechaCreacion = DateTime.Now;
                _CustomerType.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerType/Insert", _CustomerType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerType = JsonConvert.DeserializeObject<CustomerType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerType }, Total = 1 });
        }



        [HttpPut]
        public async Task<IActionResult> Update(CustomerType _customertype)
        {
           
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _customertype.FechaModificacion = DateTime.Now;
                _customertype.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerType/Update", _customertype);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customertype = JsonConvert.DeserializeObject<CustomerType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _customertype }, Total = 1 });
        }


        [HttpDelete("CustomerTypeId")]
        public async Task<ActionResult<CustomerType>> Delete(Int64 CustomerTypeId, CustomerType _CustomerTypep)
        {
            CustomerType _CustomerType = _CustomerTypep;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerType/Delete", _CustomerType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerType = JsonConvert.DeserializeObject<CustomerType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerType }, Total = 1 });
        }



    }
}