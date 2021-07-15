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
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

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
                if (_SalesOrderLine.SalesOrderId > 0 && _SalesOrderLine.SalesOrderLineId == 0 && _SalesOrderLine.Total > 0)
                {

                    _SalesOrderLine.SalesOrderLineId = 0;
                    var insertresult = await Insert(_SalesOrderLine);

                   
                    SalesOrder _cotizacion = new SalesOrder();

                    var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + _SalesOrderLine.SalesOrderId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _cotizacion = JsonConvert.DeserializeObject<SalesOrder>(valorrespuesta);

                    }

                    _client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());
                    var result1 = await _client.GetAsync(baseadress + "api/SalesOrderLine/GetSalesOrderLine");
                    string valorrespuesta1 = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrderLine>>(valorrespuesta1);
                        if (_SalesOrders != null)
                        {
                            var result2 = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Update", _cotizacion);
                            string valorrespuesta2 = "";
                            if (result2.IsSuccessStatusCode)
                            {
                                valorrespuesta2 = await (result2.Content.ReadAsStringAsync());
                                _cotizacion = JsonConvert.DeserializeObject<SalesOrder>(valorrespuesta2);


                            }
                        }
                    }

                    

                }


                if (_SalesOrderLine.SalesOrderId > 0 && _SalesOrderLine.SalesOrderLineId > 0)
                {
                    var updateresult = await Update(_SalesOrderLine.SalesOrderLineId, _SalesOrderLine);
                }

                if (_SalesOrderLine.SalesOrderId > 0)
                {


                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    _client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());
                    var result = await _client.GetAsync(baseadress + "api/SalesOrderLine/GetSalesOrderLine");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrderLine>>(valorrespuesta);
                    }
                }
                else
                {

                    List<SalesOrderLine> _existelinea = new List<SalesOrderLine>();
                    if (HttpContext.Session.GetString("listadoproductos") != "" && HttpContext.Session.GetString("listadoproductos") != null)
                    {
                        _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrderLine>>(HttpContext.Session.GetString("listadoproductos"));
                        _existelinea = _SalesOrders.Where(q => q.SalesOrderLineId == _SalesOrderLine.SalesOrderLineId).ToList();
                    }



                    if (_SalesOrderLine.SalesOrderLineId > 0 && _existelinea.Count == 0)
                    {
                        _SalesOrders.Add(_SalesOrderLine);
                        HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_SalesOrders).ToString());
                    }
                       else
                    {
                        var obj = _SalesOrders.FirstOrDefault(x => x.SalesOrderLineId == _SalesOrderLine.SalesOrderLineId);
                        if (obj != null)
                        {
                            obj.Description = _SalesOrderLine.Description;
                            obj.Price = _SalesOrderLine.Price;
                            obj.Quantity = _SalesOrderLine.Quantity;
                            obj.Amount = _SalesOrderLine.Amount;
                            obj.SubProductId = _SalesOrderLine.SubProductId;
                            obj.SubProductName = _SalesOrderLine.SubProductName;
                            obj.Total = _SalesOrderLine.Total;
                            obj.UnitOfMeasureId = _SalesOrderLine.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _SalesOrderLine.UnitOfMeasureName;
                            obj.TaxId = _SalesOrderLine.TaxId;
                            obj.TaxCode = _SalesOrderLine.TaxCode;
                            obj.TaxPercentage = _SalesOrderLine.TaxPercentage;
                            obj.TaxAmount = _SalesOrderLine.TaxAmount;
                            obj.SubTotal = _SalesOrderLine.SubTotal;
                            obj.ProductId = _SalesOrderLine.ProductId;
                            obj.DiscountAmount = _SalesOrderLine.DiscountAmount;
                            obj.DiscountPercentage = _SalesOrderLine.DiscountPercentage;
                            obj.Description = _SalesOrderLine.Description;

                        }

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
                var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrderLine/Update", _SalesOrderLine);
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
        public async Task<ActionResult<SalesOrderLine>> Delete([FromBody]SalesOrderLine _salesorder)
        {
            try
            {
                List<SalesOrderLine> _salesorderLIST =
                 JsonConvert.DeserializeObject<List<SalesOrderLine>>(HttpContext.Session.GetString("listadoproductos"));

                if (_salesorderLIST != null)
                {
                    _salesorderLIST = _salesorderLIST
                          .Where(q => q.SalesOrderLineId != _salesorder.SalesOrderLineId)
                           .Where(q => q.Quantity != _salesorder.Quantity)
                           .Where(q => q.Amount != _salesorder.Amount)
                           .Where(q => q.Total != _salesorder.Total)
                           .Where(q => q.Price != _salesorder.Price)
                           .Where(q => q.SubProductId != _salesorder.SubProductId)
                          .ToList();

                    HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_salesorderLIST));
                }


                //string baseadress = _config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrderLine/Delete", _rol);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _rol = JsonConvert.DeserializeObject<SalesOrderLine>(valorrespuesta);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return await Task.Run(() => Ok(_salesorder));
            //return new ObjectResult(new DataSourceResult { Data = new[] { _salesorder }, Total = 1 });
        }




    }
}