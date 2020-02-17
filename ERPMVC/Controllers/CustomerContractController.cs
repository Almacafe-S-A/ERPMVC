using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using Syncfusion.DocIO.Utilities;
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

namespace ERPMVC.Controllers
{
    public class CustomerContractController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerContractController(IHostingEnvironment hostingEnvironment
            , ILogger<CustomerContractController> logger, IOptions<MyConfig> config)

        {
            _hostingEnvironment = hostingEnvironment;
            this.config = config;
            this._logger = logger;

        }

        public async Task<ActionResult> Index()
        {
            return await Task.Run(() => PartialView());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerContract([FromBody]CustomerContract _CustomerContractp)
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerContract);

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
                _CustomerContract.FechaModificacion = DateTime.Now;
                _CustomerContract.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                }

                if (_listCustomerContract == null) { _listCustomerContract = new CustomerContract(); }

                if (_listCustomerContract.CustomerContractId == 0)
                {
                    _CustomerContract.FechaCreacion = DateTime.Now;
                    _CustomerContract.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerContract);
                }
                else
                {
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
                    case "Almacenaje Fiscal":
                    case "Almacenaje General":
                        Contrato = "ContratoAlmacenaje.docx";
                        break;
                    case "Bodega Habilitada":
                        Contrato = "ContratoHabilitacionBodegas.docx";
                        break;
                    case "Arrendamiento":
                        Contrato = "ContratoArrendamiento.docx";
                        break;
                    default:
                        break;
                }
                //FileStream fs = System.IO.File.OpenRead(basePath+ "/ContratosTemplate/Test.docx");
                FileStream fs = new FileStream(basePath + "/ContratosTemplate/"+Contrato, FileMode.Open, FileAccess.Read);

                WordDocument document = new WordDocument(fs, FormatType.Docx);
                string[] fieldNames = _customercontract.GetType()
                        .GetProperties()
                        .Select(q => q.Name)                       
                        .ToArray(); 

                string[] fieldValues = _customercontract.GetType()
                        .GetProperties()
                        .Select(p =>
                        {
                            object value = p.GetValue(_customercontract, null);
                            return value == null ? null : value.ToString();
                        })
                        .ToArray(); 

                //Performs the mail merge.
                document.MailMerge.Execute(fieldNames, fieldValues);
                //Saves and closes the WordDocument instance
                MemoryStream stream = new MemoryStream();

                document.Save(stream, FormatType.Docx);
                DocIORenderer render = new DocIORenderer();

                //  document.Save(stream, FormatType.Rtf);
                //  PdfDocument pdfDocument = render.ConvertToPDF(document);
                PdfDocument pdfDocument = new PdfDocument();
                await Task.Run( () =>  {  pdfDocument = render.ConvertToPDF(document); });
                
       

                //using (FileStream file = new FileStream(basePath + "/ContratosTemplate/file.docx", FileMode.Create, System.IO.FileAccess.Write))
                //    stream.WriteTo(file);
               
                document.Close();

                render.Dispose();
                MemoryStream outputStream = new MemoryStream();

                pdfDocument.Save(outputStream);

               
                document.Dispose();
                string completepath = basePath + $"/ContratosTemplate/Contrato_Cliente_{_customercontract.CustomerName}_ContratoNumero_{_customercontract.CustomerContractId}.pdf";


                using (FileStream file = new FileStream(completepath, FileMode.Create, System.IO.FileAccess.Write))
                    outputStream.WriteTo(file);

                //if (System.IO.File.Exists(_CustomerDocument.Path))
                //{
                    var stream1 = new FileStream(completepath, FileMode.Open);
                    return new FileStreamResult(stream1, "application/pdf");
                
                //else
                //{
                //    return await Task.Run(() => BadRequest($"No se encontro el archivo."));
                //}

                ViewBag.pathcontrato = completepath; //$"/ContratosTemplate/Contrato_Cliente_{_customercontract.CustomerName}_ContratoNumero_{_customercontract.CustomerContractId}.pdf";//_SFPrintContract.path = completepath;

                //Closes the instance of PDF document object

                pdfDocument.Close();
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