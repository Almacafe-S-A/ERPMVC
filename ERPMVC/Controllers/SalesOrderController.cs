using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
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
     [CustomAuthorization]
    public class SalesOrderController : Controller
    {
        //private readonly ApplicationDbContext _context;
       //  private readonly ILogger _logger;
         private readonly IOptions<MyConfig> _config;
          private readonly IMapper mapper;

        //public SalesOrderController(ILogger<SalesOrderController> logger,IOptions<MyConfig> config)
         public SalesOrderController(IOptions<MyConfig> config, IMapper mapper)
        {
            this.mapper = mapper;
           // _logger = logger;
            _config = config;
        }
        public IActionResult Index()
        {
           // SalesOrderDTO _dto = new SalesOrderDTO();
            try
            {
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwSalesOrder([FromBody]SalesOrderDTO _salesorder)
        {
            SalesOrderDTO _salesorderf = new SalesOrderDTO();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();


                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetById/"+_salesorder.SalesOrderId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _salesorderf = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }

                if (_salesorderf == null) { _salesorderf = new SalesOrderDTO { DeliveryDate=DateTime.Now,OrderDate=DateTime.Now, editar = _salesorder.editar, SalesOrderId = _salesorder.SalesOrderId }; }
                _salesorderf.editar = _salesorder.editar;             



            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View(_salesorderf);
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
         
        public async Task<ActionResult> EnviarCotizacionA([DataSourceRequest]DataSourceRequest request , SalesOrderDTO _SalesOrderDTO)
        {

            try
            {
                SalesOrderDTO _salesorderf = new SalesOrderDTO();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetById/" + _SalesOrderDTO.SalesOrderId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _salesorderf = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }

                _salesorderf.IdEstado = _SalesOrderDTO.IdEstado;

                var resultado = await Update(_SalesOrderDTO.SalesOrderId, _salesorderf);
                var value = (resultado.Result as ObjectResult).Value;
                SalesOrder _result = (SalesOrder)value;

                switch (_salesorderf.IdEstado)
                {
                    //Factura proforma
                    case 3:

                        break;
                    //Factura fiscal
                    case 4:
                    case 6: 
                        break;

                  
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return RedirectToAction("", "");
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrder>> SaveSalesOrder([FromBody]SalesOrderDTO _SalesOrder)
        {
            if (_SalesOrder != null)
            {
                SalesOrder _SalesOrdermodel = new SalesOrder();
                try
                {
                  _SalesOrdermodel  = mapper.Map<SalesOrderDTO, SalesOrder>(_SalesOrder);
                    if (_SalesOrder.SalesOrderId == 0)
                    {
                        _SalesOrder.UsuarioCreacion = HttpContext.Session.GetString("user");
                        _SalesOrder.UsuarioModificacion = HttpContext.Session.GetString("user");
                        var resultsalesorder = await Insert(_SalesOrder);
                        foreach (var item in _SalesOrder._SalesOrderLine)
                        {
                            var value = (resultsalesorder.Result as ObjectResult).Value;

                            SalesOrderLine _SalesOrderlineresponse = new SalesOrderLine();
                            item.SalesOrderId = ((SalesOrderDTO)(value)).SalesOrderId;
                            string baseadress = _config.Value.urlbase;
                            HttpClient _client = new HttpClient();

                            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                            var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrderLine/Insert", item);

                            string valorrespuesta = "";
                            if (result.IsSuccessStatusCode)
                            {
                                valorrespuesta = await (result.Content.ReadAsStringAsync());
                                _SalesOrderlineresponse = JsonConvert.DeserializeObject<SalesOrderLine>(valorrespuesta);

                            }
                            else
                            {
                                string request = await result.Content.ReadAsStringAsync();
                                return BadRequest(request);
                            }

                        }
                        return resultsalesorder;
                    }
                    else
                    {
                      //  return await Update(_SalesOrder.SalesOrderId.ToString(), _SalesOrder);
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return BadRequest("No llego correctamente el modelo!");
        }



        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrder>> Insert(SalesOrder _SalesOrder)
        {
            try
            {
                if (_SalesOrder.SalesOrderId == 0)
                {
                    // TODO: Add insert logic here
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Insert", _SalesOrder);

                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _SalesOrder = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);

                    }
                    else
                    {
                        string request = await result.Content.ReadAsStringAsync();
                        return BadRequest(request);
                    }


                }
                else
                {
                    return BadRequest("Ya existe!");
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_SalesOrder);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _role }, Total = 1 });
        }


        [HttpPut("[action]")]
        public async Task<ActionResult<SalesOrder>> Update(Int64 Id, SalesOrderDTO _SalesOrderLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/SalesOrder/Update", _SalesOrderLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrderLine = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                // _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SalesOrderLine }, Total = 1 });

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrderDTO>> Delete([FromBody]SalesOrderDTO _rol)
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
                    _rol = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                //  _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });
        }



        [HttpGet("{SalesOrderId}")]
       public  ActionResult AR(Int32 SalesOrderId)
        {

            SalesOrderDTO _salesorderdto = new SalesOrderDTO { SalesOrderId = SalesOrderId, token = HttpContext.Session.GetString("token") };

            return View(_salesorderdto);
        }



    }
}