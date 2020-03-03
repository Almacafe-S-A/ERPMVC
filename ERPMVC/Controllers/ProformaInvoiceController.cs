using System;
using System.Collections.Generic;
using System.IO;
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
using Syncfusion.XlsIO;

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

        [Authorize(Policy = "Ventas.Factura Proforma")]
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
                    _ProformaInvoice = _ProformaInvoice.OrderByDescending(q => q.ProformaId).ToList();
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
                    _ProformaInvoice = _ProformaInvoice.OrderByDescending(q => q.ProformaId).ToList();
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


        [HttpPost]
        public async Task<ActionResult> GetProformaInvoiceById([FromBody]ProformaInvoice _ProformaInvoicep)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            ProformaInvoice _ProformaInvoice = new ProformaInvoice();
            try
            {

                //GoodsDelivered _GoodsDeliveredp = JsonConvert.DeserializeObject<GoodsDelivered>(dto);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoice/GetProformaInvoiceLineById", _ProformaInvoicep);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);

                }

                if (_ProformaInvoice == null)
                {
                    _ProformaInvoice = new ProformaInvoice();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProformaInvoice);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetProformaInvoiceByCustomer([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<ProformaInvoice> _ProformaInvoice = new List<ProformaInvoice>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoiceByCustomer/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<List<ProformaInvoice>>(valorrespuesta);
                    _ProformaInvoice = _ProformaInvoice.OrderByDescending(x => x.ProformaId).ToList();
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
                var result3 = await _client.PostAsJsonAsync(baseadress + "api/CustomerConditions/GetCustomerConditionsByClass", _custo);
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
                    if (_CertificadoDeposito.FechaCertificado.Day <= 15)
                    {
                        dias = 30;
                    }
                    else
                    {
                        dias = 15;
                    }
                }
                else if (_proformap.OrderDate.Month > _CertificadoDeposito.FechaCertificado.Month
                    && _proformap.OrderDate.Year > _CertificadoDeposito.FechaCertificado.Year)
                {
                    dias = 30;
                }
                else
                {
                    dias = 30;
                }


                //Se utiliza el campo Discount como Quqantity de la linea y se usa el campo SubTotal como Price

                double totalfacturar = 0;
                foreach (var item in _CustomerConditions)
                {
                    foreach (var lineascertificadas in _CertificadoDeposito._CertificadoLine)
                    {

                        switch (item.LogicalCondition)
                        {
                            case ">=":
                                if (_proformap.SubTotal >= Convert.ToDouble(item.ValueToEvaluate))
                                    totalfacturar += ((item.ValueDecimal * (_proformap.Discount * _proformap.SubTotal)) / 30) * dias;
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
        // public async Task<ActionResult<ProformaInvoice>> SaveProformaInvoice([FromBody]ProformaInvoice _ProformaInvoice)
        public async Task<ActionResult<ProformaInvoiceDTO>> SaveProformaInvoice([FromBody]dynamic dto)
        {
            ProformaInvoiceDTO _ProformaInvoice = new ProformaInvoiceDTO();
            try
            {
                _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoiceDTO>(dto.ToString());
                if (_ProformaInvoice != null)
                {

                    if (_ProformaInvoice.Total <= 0 || _ProformaInvoice.SubTotal <= 0)
                    {
                        return await Task.Run(() => BadRequest($"No se esta calculando correctamente los totales!"));
                    }

                    foreach (var item in _ProformaInvoice.ProformaInvoiceLine)
                    {
                        if (item.UnitOfMeasureId == 0)
                        {
                            return await Task.Run(() => BadRequest("Ingrese una unidad de medida valido!"));
                        }

                        if(item.Total==0)
                        {
                            return await Task.Run(() => BadRequest("El documento no se ha calculado correctamente!"));
                        }

                        if(item.TaxCode=="")
                        {
                            return await Task.Run(() => BadRequest($"Debe llevar un código de impuesto!, el producto :{item.SubProductName}"));
                        }

                    } 

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
                        var value = (insertresult.Result as ObjectResult).Value;
                        ProformaInvoice resultado = ((ProformaInvoice)(value));
                        if (resultado.ProformaId <= 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero la factura proforma!"));
                        }
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



        [HttpGet]      
        public async Task<ActionResult> ExportCalculoFactura(Int64 id)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2007;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    List<InvoiceCalculation> InvoiceCalculationL = new List<InvoiceCalculation>();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetInvoiceCalculation/" + id);
                    string valorrespuesta = "";

                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        InvoiceCalculationL = JsonConvert.DeserializeObject<List<InvoiceCalculation>>(valorrespuesta);
                    }

                    //GetCustomerAsObjects method returns list of customers
                   // List<InvoiceCalculation> employees = GetEmployees();

                    //Import data to worksheet
                    worksheet.ImportData(InvoiceCalculationL, 2, 1, true);

                    //Saving the workbook as stream
                    FileStream file = new FileStream("Spreadsheet.xlsx", FileMode.Create, FileAccess.ReadWrite);
                    workbook.SaveAs(file);
                    //file.Dispose();

                    string fileName = $"CalculoFactura_Proforma_{id}.xlsx";
                    string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    file.Position = 0;
                    return File(file, fileType, fileName);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest(""));
            }



        }


        [HttpGet]
        public async Task<ActionResult> ExportCalculoFacturaByIdentificador(Guid id)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2007;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    List<InvoiceCalculation> InvoiceCalculationL = new List<InvoiceCalculation>();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetInvoiceCalculationByIdentificador/" + id);
                    string valorrespuesta = "";

                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        InvoiceCalculationL = JsonConvert.DeserializeObject<List<InvoiceCalculation>>(valorrespuesta);
                    }

                    //GetCustomerAsObjects method returns list of customers
                    // List<InvoiceCalculation> employees = GetEmployees();

                    //Import data to worksheet
                    worksheet.ImportData(InvoiceCalculationL, 2, 1, true);

                    //Saving the workbook as stream
                    FileStream file = new FileStream("Spreadsheet.xlsx", FileMode.Create, FileAccess.ReadWrite);
                    workbook.SaveAs(file);
                    //file.Dispose();

                    string fileName = $"CalculoFactura_Proforma_{id}.xlsx";
                    string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    file.Position = 0;
                    return File(file, fileType, fileName);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest(""));
            }



        }



        // POST: ProformaInvoice/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Insert(ProformaInvoiceDTO _ProformaInvoiceDTO)
        {
            ProformaInvoice _ProformaInvoice = new ProformaInvoice();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProformaInvoiceDTO.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ProformaInvoiceDTO.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoice/Insert", _ProformaInvoiceDTO);
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

            return Ok(_ProformaInvoice);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
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


            return Ok(_ProformaInvoice);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }

        [HttpGet]
        public async Task<ActionResult> SFProformaInvoice(Int32 id)
        {
            try
            {
                ProformaInvoiceDTO _invoicedto = new ProformaInvoiceDTO { ProformaId = id, };
                return await Task.Run(() => View(_invoicedto));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }


        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request)
        {
            var res = await GetProforma();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetProforma())
                {
                    if (values.Contains(order.ProformaId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<ProformaInvoice>> GetProforma()
        {
            List<ProformaInvoice> _CertificadoDeposito = new List<ProformaInvoice>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoice/");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<ProformaInvoice>>(valorrespuesta);
                    _CertificadoDeposito = (from c in _CertificadoDeposito                                            
                                            select new ProformaInvoice
                                            {
                                                ProformaId = c.ProformaId,
                                                CustomerName = "Id:" + c.ProformaId +  "  || Nombre:" + c.CustomerName + "|| Fecha:" + c.OrderDate + "|| Total:" + c.Total,
                                                CustomerId = c.CustomerId,
                                            }).ToList();              

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _CertificadoDeposito;
        }







    }
}