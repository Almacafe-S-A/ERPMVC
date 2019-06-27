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
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProformaInvoiceController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ProformaInvoiceController(ILogger<ProformaInvoiceController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwProformaInvoice([FromBody]ProformaInvoiceDTO _ProformaId)
        {
            ProformaInvoiceDTO _ProformaInvoice = new ProformaInvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoiceById/" + _ProformaId.ProformaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoiceDTO>(valorrespuesta);

                }

                if (_ProformaInvoice == null)
                {
                    _ProformaInvoice = new ProformaInvoiceDTO { ExpirationDate = DateTime.Now.AddDays(30), OrderDate = DateTime.Now, editar = 1 };
                }
                else
                {
                    _ProformaInvoice.editar = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ProformaInvoice);

        }


        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ProformaInvoice> _ProformaInvoice = new List<ProformaInvoice>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoice");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<List<ProformaInvoice>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProformaInvoice.ToDataSourceResult(request);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetProformaInvoice([DataSourceRequest]DataSourceRequest request)
        {
            List<ProformaInvoice> _ProformaInvoice = new List<ProformaInvoice>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoice");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<List<ProformaInvoice>>(valorrespuesta);
                }
                //else if(result.StatusCode== 401)
                //{
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }


            return _ProformaInvoice.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CalcularConCertificado([FromBody]ProformaInvoice _proformap)
        {
            ProformaInvoice _pinvoice = new ProformaInvoice();
            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito();
            SalesOrder _SalesOrder = new SalesOrder();
            List<CustomerConditions> _CustomerConditions = new List<CustomerConditions>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _proformap.CertificadoDepositoId);
                string valorrespuesta = "";
                _proformap.FechaModificacion = DateTime.Now;
                _proformap.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }


                _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result2 = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + _proformap.SalesOrderId);
                 valorrespuesta = "";
                _proformap.FechaModificacion = DateTime.Now;
                _proformap.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result2.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result2.Content.ReadAsStringAsync());
                    _SalesOrder = JsonConvert.DeserializeObject<SalesOrder>(valorrespuesta);
                }


                _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                CustomerConditions _custo = new CustomerConditions { DocumentId = _proformap.SalesOrderId, IdTipoDocumento = 12 };
                var result3 = await _client.PostAsJsonAsync(baseadress + "api/CustomerConditions/GetCustomerConditionsByClass" , _custo);
                valorrespuesta = "";
                _proformap.FechaModificacion = DateTime.Now;
                _proformap.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result3.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result3.Content.ReadAsStringAsync());
                    _CustomerConditions = JsonConvert.DeserializeObject<List<CustomerConditions>>(valorrespuesta);
                }


                int dias = 0;
                if (_proformap.OrderDate.Month == _CertificadoDeposito.FechaCertificado.Month
                    && _proformap.OrderDate.Year == _CertificadoDeposito.FechaCertificado.Year)
                {
                    if(_CertificadoDeposito.FechaCertificado.Day<=15)
                    {
                        dias = 30;
                    }
                    else
                    {
                        dias = 15;
                    }
                }
                else if(_proformap.OrderDate.Month > _CertificadoDeposito.FechaCertificado.Month 
                    && _proformap.OrderDate.Year > _CertificadoDeposito.FechaCertificado.Year)
                {
                    dias = 30;
                }
                else
                {
                    dias = 30;
                }


                double totalfacturar = 0;
                foreach (var item in _CustomerConditions)
                {
                    foreach (var lineascertificadas in _CertificadoDeposito._CertificadoLine)
                    {

                        switch (item.LogicalCondition)
                        {
                            case ">=":
                                if(_proformap.SubTotal>=Convert.ToDouble(item.ValueToEvaluate))
                                totalfacturar += ((item.ValueDecimal * (_proformap.Discount * _proformap.SubTotal) )/30)*dias;
                                break;
                            case "<=":
                                if (_proformap.SubTotal <= Convert.ToDouble(item.ValueToEvaluate))
                                    totalfacturar += ((item.ValueDecimal * (_proformap.Discount * _proformap.SubTotal)) / 30) * dias;
                                break;
                            case ">":
                                if (_proformap.SubTotal > Convert.ToDouble(item.ValueToEvaluate))
                                    totalfacturar += ((item.ValueDecimal * (_proformap.Discount * _proformap.SubTotal)) / 30) * dias;
                                break;
                            case "<":
                                if (_proformap.SubTotal < Convert.ToDouble(item.ValueToEvaluate))
                                    totalfacturar += ((item.ValueDecimal * (_proformap.Discount * _proformap.SubTotal)) / 30) * dias;
                                break;
                            case "=":
                                if (_proformap.SubTotal == Convert.ToDouble(item.ValueToEvaluate))
                                    totalfacturar += ((item.ValueDecimal * (_proformap.Discount * _proformap.SubTotal)) / 30) * dias;
                                break;
                            default:
                                break;
                        }
                    }
                }

                _pinvoice.Amount = totalfacturar;
                _pinvoice.Total = totalfacturar;

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_pinvoice);
        }

        [HttpPost("[action]")]
       // [ValidateAntiForgeryToken]
         public async Task<ActionResult<ProformaInvoice>> SaveProformaInvoice([FromBody]ProformaInvoice _ProformaInvoice)
      //  public async Task<ActionResult<SalesOrder>> SaveProformaInvoice([FromBody]dynamic dto)
        {
           // ProformaInvoice _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(dto.ToString());
            try
            {
                if (_ProformaInvoice != null)
                {
                    ProformaInvoice _listProformaInvoice = new ProformaInvoice();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoiceById/" + _ProformaInvoice.ProformaId);
                    string valorrespuesta = "";
                    _ProformaInvoice.FechaModificacion = DateTime.Now;
                    _ProformaInvoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                    }

                    if (_listProformaInvoice == null) { _listProformaInvoice = new ProformaInvoice(); }
                    if (_listProformaInvoice.ProformaId == 0)
                    {
                        _ProformaInvoice.FechaCreacion = DateTime.Now;
                        _ProformaInvoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_ProformaInvoice);
                    }
                    else
                    {
                        var updateresult = await Update(_ProformaInvoice.ProformaId, _ProformaInvoice);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProformaInvoice);
        }

        // POST: ProformaInvoice/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Insert(ProformaInvoice _ProformaInvoice)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProformaInvoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ProformaInvoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoice/Insert", _ProformaInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_ProformaInvoice);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Update(Int64 id, ProformaInvoice _ProformaInvoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ProformaInvoice/Update", _ProformaInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Delete([FromBody]ProformaInvoice _ProformaInvoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoice/Delete", _ProformaInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }




    }
}