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
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractLines/GetCustomerContractLinesById/" ,_CustomerContractLines);
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
            List<CustomerContractLines> _CustomerContracts = new List<CustomerContractLines>();
          
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                if (HttpContext.Session.Get("listadoproductos") == null 
                    || HttpContext.Session.GetString("listadoproductos") =="")
                {
                    if (_CustomerContractLines.SubProductId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_CustomerContracts).ToString();
                        HttpContext.Session.SetString("listadoproductos", serialzado);
                    }
                }
                else
                {
                    _CustomerContracts = JsonConvert.DeserializeObject<List<CustomerContractLines>>(HttpContext.Session.GetString("listadoproductos"));
                }
                if (_CustomerContractLines.CustomerContractId > 0 && _CustomerContractLines.Id == 0 && _CustomerContractLines.Price != 0   )
                {

                    _CustomerContractLines.Id = 0;
                    var insertresult = await Insert(_CustomerContractLines);

                   
                    CustomerContract _cotizacion = new CustomerContract();

                    var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + _CustomerContractLines.CustomerContractId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _cotizacion = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);

                    }

                    _client.DefaultRequestHeaders.Add("CustomerContractId", _CustomerContractLines.CustomerContractId.ToString());
                    var result1 = await _client.GetAsync(baseadress + "api/CustomerContractLines/GetCustomerContractLines");
                    string valorrespuesta1 = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        _CustomerContracts = JsonConvert.DeserializeObject<List<CustomerContractLines>>(valorrespuesta1);
                        if (_CustomerContracts != null)
                        {
                            var result2 = await _client.PostAsJsonAsync(baseadress + "api/CustomerContract/Update", _cotizacion);
                            string valorrespuesta2 = "";
                            if (result2.IsSuccessStatusCode)
                            {
                                valorrespuesta2 = await (result2.Content.ReadAsStringAsync());
                                _cotizacion = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta2);


                            }
                        }
                    }

                    

                }


                if (_CustomerContractLines.CustomerContractId > 0 && _CustomerContractLines.Id > 0)
                {
                    var updateresult = await Update(_CustomerContractLines.Id, _CustomerContractLines);
                }

                if (_CustomerContractLines.CustomerContractId > 0)
                {


                    //_client.DefaultRequestHeaders.Add("CustomerContractId", _CustomerContract.CustomerContractId.ToString());
                    _client.DefaultRequestHeaders.Add("CustomerContractId", _CustomerContractLines.CustomerContractId.ToString());
                    var result = await _client.GetAsync(baseadress + "api/CustomerContractLines/GetCustomerContractLines");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _CustomerContracts = JsonConvert.DeserializeObject<List<CustomerContractLines>>(valorrespuesta);
                    }
                }
                else
                {

                    List<CustomerContractLines> _existelinea = new List<CustomerContractLines>();
                    if (HttpContext.Session.GetString("listadoproductos") != "" && HttpContext.Session.GetString("listadoproductos") != null)
                    {
                        _CustomerContracts = JsonConvert.DeserializeObject<List<CustomerContractLines>>(HttpContext.Session.GetString("listadoproductos"));
                        _existelinea = _CustomerContracts.Where(q => q.Id == _CustomerContractLines.Id).ToList();
                    }



                    if (_CustomerContractLines.Id > 0 && _existelinea.Count == 0)
                    {
                        _CustomerContracts.Add(_CustomerContractLines);
                        HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_CustomerContracts).ToString());
                    }
                       else
                    {
                        var obj = _CustomerContracts.FirstOrDefault(x => x.Id == _CustomerContractLines.Id);
                        if (obj != null)
                        {
                            obj.Description = _CustomerContractLines.Description;
                            obj.Price = _CustomerContractLines.Price;
                            obj.Quantity = _CustomerContractLines.Quantity;
                            obj.SubProductId = _CustomerContractLines.SubProductId;
                            obj.SubProductName = _CustomerContractLines.SubProductName;
                            obj.UnitOfMeasureId = _CustomerContractLines.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _CustomerContractLines.UnitOfMeasureName;
                            obj.Description = _CustomerContractLines.Description;
                            obj.PeriodoCobro = _CustomerContractLines.PeriodoCobro;
                            obj.TipoCobroId = _CustomerContractLines.TipoCobroId;
                            obj.TipoCobroName = _CustomerContractLines.TipoCobroName;


                        }

                        HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_CustomerContracts).ToString());



                    }





                }


            }
            catch (Exception ex)
            {
               _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContracts.ToDataSourceResult(request);
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