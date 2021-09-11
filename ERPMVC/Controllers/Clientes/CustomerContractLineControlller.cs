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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CustomerContractLinesController : Controller
    {
         private readonly IOptions<MyConfig> _config;
          private readonly ILogger _logger;
        public CustomerContractLinesController(ILogger<CustomerContractLinesController> logger, IOptions<MyConfig> config)
        {
              this._logger = logger;
             this._config = config;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwCustomerContractDetailMant([FromBody]CustomerContractLines _CustomerContractLines)
        {
            CustomerContractLines _CustomerContractf = new CustomerContractLines();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();


                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContract/GetCustomerContractLines/", _CustomerContractLines);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractf = JsonConvert.DeserializeObject<CustomerContractLines>(valorrespuesta);
                }

                if (_CustomerContractf == null) { _CustomerContractf = new CustomerContractLines { Description = ""  }; }
                //_CustomerContractf.editar = _CustomerContractLines.editar;



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return PartialView("~/Views/CustomerContract/pvwCustomerContractDetailMant.cshtml",_CustomerContractf);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCustomerContractLines([DataSourceRequest]DataSourceRequest request,CustomerContractLines _CustomerContractLines)
        {
            List<CustomerContractLines> _CustomerContractsLines = new List<CustomerContractLines>();

            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractLines/" + _CustomerContractLines.CustomerContractId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractsLines = JsonConvert.DeserializeObject<List<CustomerContractLines>>(valorrespuesta);

                }


            }
            catch (Exception ex) {
                //return BadRequest();
            
            }



                return _CustomerContractsLines.ToDataSourceResult(request);
        }




        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCustomerContractLinesTerms([DataSourceRequest] DataSourceRequest request, CustomerContractLines _CustomerContractLines)
        {
            List<CustomerContractLinesTerms> _CustomerContractsLines = new List<CustomerContractLinesTerms>();

            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractLinesTerms/" + _CustomerContractLines.CustomerContractId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractsLines = JsonConvert.DeserializeObject<List<CustomerContractLinesTerms>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                //return BadRequest();

            }



            return _CustomerContractsLines.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractLines>> SaveCustomerContractLines([FromBody]CustomerContractLines _CustomerContractLines)
        {
            if (_CustomerContractLines.Id == 0)
            {
                return await Insert(_CustomerContractLines);
            }
            else
            {
                return await Update(_CustomerContractLines.Id, _CustomerContractLines);
            }
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractLines>> Insert(CustomerContractLines _CustomerContractLines)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractLines/Insert", _CustomerContractLines);

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractLines = JsonConvert.DeserializeObject<CustomerContractLines>(valorrespuesta);

                }
                else
                {
                    string request = await result.Content.ReadAsStringAsync();
                    return BadRequest(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

              return Ok("Datos Guardados Correctamente! ");
           // return new ObjectResult(new DataSourceResult { Data = new[] { _role }, Total = 1 });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<CustomerContractLines>> Update(Int64 Id, CustomerContractLines _CustomerContractLines)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractLines/Update", _CustomerContractLines);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractLines = JsonConvert.DeserializeObject<CustomerContractLines>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractLines }, Total = 1 });

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractLines>> Delete([FromBody]CustomerContractLines _CustomerContract)
        {
            try
            {
                List<CustomerContractLines> _CustomerContractLIST =
                 JsonConvert.DeserializeObject<List<CustomerContractLines>>(HttpContext.Session.GetString("listadoproductos"));

                if (_CustomerContractLIST != null)
                {
                    _CustomerContractLIST = _CustomerContractLIST
                          .Where(q => q.Id != _CustomerContract.Id)
                           .Where(q => q.Quantity != _CustomerContract.Quantity)
                           .Where(q => q.Price != _CustomerContract.Price)
                           .Where(q => q.SubProductId != _CustomerContract.SubProductId)
                          .ToList();

                    HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_CustomerContractLIST));
                }


                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractLines/Delete", _CustomerContract);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContractLines>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return await Task.Run(() => Ok(_CustomerContract));
            //return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContract }, Total = 1 });
        }




    }
}