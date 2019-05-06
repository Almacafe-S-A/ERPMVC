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
    public class SalesOrderLineController : Controller
    {
         private readonly IOptions<MyConfig> _config;
          private readonly ILogger _logger;
        public SalesOrderLineController(ILogger<SalesOrderLineController> logger, IOptions<MyConfig> config)
        {
              this._logger = logger;
             this._config = config;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwSalesOrderDetailMant([FromBody]SalesOrderLine _salesorderline)
        {
            SalesOrderLine _salesorderf = new SalesOrderLine();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();


                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrderLine/GetSalesOrderLineById/" ,_salesorderline);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _salesorderf = JsonConvert.DeserializeObject<SalesOrderLine>(valorrespuesta);
                }

                if (_salesorderf == null) { _salesorderf = new SalesOrderLine { Description = ""  }; }
                //_salesorderf.editar = _salesorderline.editar;



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return PartialView("~/Views/SalesOrder/pvwSalesOrderDetailMant.cshtml",_salesorderf);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetSalesOrderLine([DataSourceRequest]DataSourceRequest request,SalesOrderLine _SalesOrderLine)
        {
            List<SalesOrderLine> _SalesOrders = new List<SalesOrderLine>();
          
            try
            {

                if (HttpContext.Session.Get("listadoproductos") == null 
                    || HttpContext.Session.GetString("listadoproductos") =="")
                {
                    if (_SalesOrderLine.SubProductId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_SalesOrders).ToString();
                        HttpContext.Session.SetString("listadoproductos", serialzado);
                    }
                }
                else
                {
                    _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrderLine>>(HttpContext.Session.GetString("listadoproductos"));
                }
                if (_SalesOrderLine.SalesOrderId > 0)
                {

                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    _client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/SalesOrderLine/");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrderLine>>(valorrespuesta);
                    }
                }
                else
                {
                    if (_SalesOrderLine.SubProductId > 0)
                    {
                        _SalesOrders.Add(_SalesOrderLine);
                        HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_SalesOrders).ToString());
                    }
                }


            }
            catch (Exception ex)
            {
               _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _SalesOrders.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrderLine>> SaveSalesOrderLine([FromBody]SalesOrderLine _SalesOrderLine)
        {
            if (_SalesOrderLine.SalesOrderLineId == 0)
            {
                return await Insert(_SalesOrderLine);
            }
            else
            {
                return await Update(_SalesOrderLine.SalesOrderLineId, _SalesOrderLine);
            }
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrderLine>> Insert(SalesOrderLine _SalesOrderLine)
        {
            try
            {


                if (_SalesOrderLine.Quantity >0)
                {
                    // TODO: Add insert logic here
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                        var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrderLine/Insert", _SalesOrderLine);

                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _SalesOrderLine = JsonConvert.DeserializeObject<SalesOrderLine>(valorrespuesta);

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
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

              return Ok("Datos Guardados Correctamente! ");
           // return new ObjectResult(new DataSourceResult { Data = new[] { _role }, Total = 1 });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<SalesOrderLine>> Update(Int64 Id, SalesOrderLine _SalesOrderLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/SalesOrderLine/Update", _SalesOrderLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrderLine = JsonConvert.DeserializeObject<SalesOrderLine>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SalesOrderLine }, Total = 1 });

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrderLine>> Delete([FromBody]SalesOrderLine _rol)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrderLine/Delete", _rol);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _rol = JsonConvert.DeserializeObject<SalesOrderLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
        
            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });
        }




    }
}