using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;

using Syncfusion.DocIO.DLS;
using Microsoft.AspNetCore.Hosting;
using ERPMVC.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ERPMVC.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Syncfusion.Pdf;
using Syncfusion.DocIORenderer;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    public class CustomerContractController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public CustomerContractController(IHostingEnvironment hostingEnvironment
            , ILogger<CustomerContractController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)

        {
            _hostingEnvironment = hostingEnvironment;
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public async Task<ActionResult> Index()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(() => PartialView());
        }

        public async Task<ActionResult> ListadoContratos()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(() => View());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerContract([FromBody] CustomerContract _CustomerContractp)
        {
            CustomerContract _CustomerContract = new CustomerContract();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + _CustomerContractp.CustomerContractId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);

                }

                if (_CustomerContract == null)
                {
                    _CustomerContract = new CustomerContract { CustomerId = _CustomerContractp.CustomerId };
                }
                _CustomerContract.ListaWarehouseIds = new List<long>();
                foreach (var item in _CustomerContract.customerContractWarehouse)
                {
                    int i = 1;
                    _CustomerContract.ListaWarehouseIds.Add(item.WareHouseId); // + (i < _CustomerContract.customerContractWarehouse.Count ? "," : "");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerContract);

        }



        public async Task<ActionResult> pvwCustomerContractLinesTerms([FromBody] CustomerContractTerms _sarpara)
        {
            CustomerContractLinesTerms _CustomerContractTerms = new CustomerContractLinesTerms();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/CustomerContractLinesTermsById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractTerms = JsonConvert.DeserializeObject<CustomerContractLinesTerms>(valorrespuesta);

                }

                if (_CustomerContractTerms == null)
                {
                    _CustomerContractTerms = new CustomerContractLinesTerms();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerContractTerms);

        }

        [HttpGet("[action]")]
        public async Task<ActionResult<CustomerContract>> GetCustomerContractById(int ContractId)
        {
            CustomerContract _CustomerContract = new CustomerContract();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + ContractId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                
            }



            return _CustomerContract;

        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<CustomerContract>>> GetCustomerActiveContractByCustomerId(int CustomerId)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerActiveContractByCustomerId/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");

            }



            return Json(_CustomerContract);

        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContract");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContract.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetList([DataSourceRequest] DataSourceRequest request)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractList");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContract.ToDataSourceResult(request);

        }



        [HttpGet]
        public async Task<DataSourceResult> GetContractsList([DataSourceRequest] DataSourceRequest request)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContract");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContract.ToDataSourceResult(request);

        }


        [HttpPost("[action]")]
        public async Task<ActionResult> GetCustomerContractbySalesorderId([FromBody] CustomerContract  idsalesorder)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            CustomerContract _CustomerContractp = new CustomerContract();

            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContract");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);
                    _CustomerContractp = _CustomerContract.Where(x => x.SalesOrderId == idsalesorder.SalesOrderId).FirstOrDefault();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => Ok(_CustomerContractp));

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCustomerContractByCustomerId([DataSourceRequest]DataSourceRequest request,Int64 CustomerId)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractByCustomerId/"+ CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContract.ToDataSourceResult(request);

        }
        


            [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> CustomerAdendumByContract([DataSourceRequest] DataSourceRequest request, Int64 ContractId)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/CustomerAdendumByContract/" + ContractId);

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            //return Json(_CustomerContract);
            return _CustomerContract.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCustomerContractActiveByCustomerId([DataSourceRequest] DataSourceRequest request, Int64 CustomerId)
        {
            List<CustomerContract> _CustomerContract = new List<CustomerContract>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractActiveByCustomerId/" + CustomerId);

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<List<CustomerContract>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            //return Json(_CustomerContract);
            return _CustomerContract.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContract>> SaveCustomerContract([FromBody]CustomerContract _CustomerContract)
        {
            try
            {
                CustomerContract _listCustomerContract = new CustomerContract();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + _CustomerContract.CustomerContractId);
                string valorrespuesta = "";
               
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

                if (_listCustomerContract == null) { _listCustomerContract = new CustomerContract(); }

                if (_CustomerContract.ListaWarehouseIds.Count > 0) {
                    _CustomerContract.customerContractWarehouse = new List<CustomerContractWareHouse>();
                    foreach (var item in _CustomerContract.ListaWarehouseIds)
                    {
                        var Warehouse = new CustomerContractWareHouse()
                        {
                            CustomerContractId = _CustomerContract.CustomerContractId,
                            WareHouseId = Convert.ToInt32(item),
                            EdificioName="",
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = HttpContext.Session.GetString("user")
                        };
                        _CustomerContract.customerContractWarehouse.Add(Warehouse);
                    }
                }

                if (_listCustomerContract.CustomerContractId == 0)
                {
                    _CustomerContract.FechaCreacion = DateTime.Now;
                    _CustomerContract.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerContract);
                }
                else
                {
                    _CustomerContract.FechaModificacion = DateTime.Now;
                    _CustomerContract.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_CustomerContract.CustomerContractId, _CustomerContract);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerContract);
        }

        // POST: CustomerContract/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerContract>> Insert(CustomerContract _CustomerContract)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerContract.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerContract.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContract/Insert", _CustomerContract);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CustomerContract);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContract }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerContract>> Update(Int64 id, CustomerContract _CustomerContract)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerContract/Update", _CustomerContract);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContract }, Total = 1 });
        }



        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContract>> ActivarContrato([FromBody] CustomerContract _CustomerContract)
        {
            CustomerContract CustomerContract = new CustomerContract();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + _CustomerContract.CustomerContractId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                    bool dias = (decimal)CustomerContract.Plazo -  Decimal.ToInt32((decimal)CustomerContract.Plazo) > 0 ? true :false;
                    CustomerContract.FechaInicioContrato = _CustomerContract.FechaInicioContrato;
                    CustomerContract.FechaVencimiento = ((DateTime)CustomerContract.FechaInicioContrato).AddMonths(Decimal.ToInt32((decimal)CustomerContract.Plazo));
                    CustomerContract.FechaVencimiento = ((DateTime)CustomerContract.FechaVencimiento)
                        .AddDays(dias?15:0);
                    CustomerContract.Estado = "Vigente";
                    CustomerContract.IdEstado = 7;
                    CustomerContract.FechaModificacion = DateTime.Now;
                    CustomerContract.UsuarioModificacion = HttpContext.Session.GetString("user");
                }

                var result2 = await _client.PutAsJsonAsync(baseadress + "api/CustomerContract/Update", CustomerContract);                
                if (result2.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result2.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContract }, Total = 1 });
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CustomerContract>> Anular([FromBody]CustomerContract _CustomerContract)
        {
            CustomerContract _cc = new CustomerContract();
            if (_CustomerContract != null)
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + _CustomerContract.CustomerContractId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _cc = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                        _cc.IdEstado = 7;
                        _cc.Estado = "Cancelado";
                        _cc.Observcion = _CustomerContract.Observcion;
                        var resultcustomerc = await Update(_cc.CustomerContractId, _cc);

                        var value = (resultcustomerc.Result as ObjectResult).Value;
                        //CustomerContract resultado = ((CustomerContract)(value));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            else
            {
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }

            return await Task.Run(() => Ok(_cc));
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContract>> Delete([FromBody]CustomerContract _CustomerContract)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContract/Delete", _CustomerContract);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContract }, Total = 1 });
        }




        public async Task<ActionResult> SFPrintContract(Int64 id)
        {
            CustomerContract _customercontract = new CustomerContract();
        //    SFPrintContract _SFPrintContract = new SFPrintContract();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContract/GetCustomerContractById/" + id);
                string valorrespuesta = "";
                _customercontract.FechaModificacion = DateTime.Now;
                _customercontract.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _customercontract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

                string basePath = _hostingEnvironment.WebRootPath;
                string Contrato = "";
                switch (_customercontract.ProductName)
                {
                    case "Almacen Fiscal":
                    case "Almacen General":
                        Contrato = "CONTRATOALMACENGENERAL.docx";
                        break;
                    case "Bodega Habilitada":
                        Contrato = "ContratoHabilitacionBodegas.docx";
                        break;
                    case "Arrendamiento":
                        Contrato = "ContratoArrendamiento.docx";
                        break;
                    case "Arrendamiento de Equipo":
                        Contrato = "ArrendamientoEquipo.docx";
                        break;
                    default:
                        break;
                }

                if (String.IsNullOrEmpty(Contrato))
                {
                    return await Task.Run(() => BadRequest($"No se encontro producto : " + _customercontract.ProductName + " para el archivo a imprimir."));
                }
                //FileStream fs = System.IO.File.OpenRead(basePath+ "/ContratosTemplate/Test.docx");
                FileStream fs = new FileStream(basePath + "/ContratosTemplate/"+Contrato, FileMode.Open, FileAccess.Read);

                WordDocument document = new WordDocument(fs, FormatType.Docx);
                List<string> nombres = _customercontract.GetType()
                        .GetProperties()
                        .Select(q => q.Name).ToList();



                List<string> valores = _customercontract.GetType()
                        .GetProperties()
                        .Select(p =>
                        {
                            object value = p.GetValue(_customercontract, null);
                            return value == null ? null : value.ToString();
                        }).ToList();

                foreach (var item in _customercontract.customerContractLinesTerms.OrderBy(o => o.Position))
                {
                    //fieldNames.Append<string>("");
                    nombres.Add($"ClausulaTitulo{item.Position}");
                    valores.Add(item.TermTitle);
                    nombres.Add($"Clausula{item.Position}");
                    valores.Add(item.Term);
                }

                string[] fieldNames = nombres.ToArray();
                string[] fieldValues = valores.ToArray();


                document.MailMerge.Execute(fieldNames, fieldValues);
                //Saves and closes the WordDocument instance
                MemoryStream stream = new MemoryStream();

                document.Save(stream, FormatType.Docx);


                
                stream.Position = 0;
                //return File(stream, "application/msword", $"{_customercontract.CustomerContractId}{_customercontract.ProductName}-{_customercontract.CustomerName}.docx");
                MemoryStream outputStream = new MemoryStream();
                
                await Task.Run( () =>  {
                    DocIORenderer render = new DocIORenderer();
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument = render.ConvertToPDF(document);
                    pdfDocument.Save(outputStream);
                    render.Dispose();

                });
                document.Close();



                //pdfDocument.Close();
                //document.Dispose();
                outputStream.Position = 0;
                    return  File(outputStream, "application/pdf" , $"{_customercontract.CustomerContractId}{_customercontract.ProductName}-{_customercontract.CustomerName}.pdf");

                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
           

            return View();
        }


        public ActionResult ReplaceTest()
        {


            string basePath = _hostingEnvironment.WebRootPath;
            //FileStream fs = System.IO.File.OpenRead(basePath+ "/ContratosTemplate/Test.docx");
            FileStream fs = new FileStream(basePath + "/ContratosTemplate/Test.docx", FileMode.Open, FileAccess.Read);

            WordDocument document = new WordDocument(fs, FormatType.Docx);
            string[] fieldNames = new string[] { "EmployeeId", "Name", "Phone", "City" };
            string[] fieldValues = new string[] { "1001", "Freddy", "+122-97242717", "Tegucigalpa" };

            //Performs the mail merge.

            document.MailMerge.Execute(fieldNames, fieldValues);

            //Saves and closes the WordDocument instance
            // document.Save(document.)
            MemoryStream stream = new MemoryStream();

            
            document.Save(stream, FormatType.Docx);

            using (FileStream file = new FileStream(basePath+ "/ContratosTemplate/file.docx", FileMode.Create, System.IO.FileAccess.Write))
                stream.WriteTo(file);

            document.Close();

            return View();
        }


    }
}