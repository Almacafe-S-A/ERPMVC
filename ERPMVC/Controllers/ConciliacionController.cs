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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using Syncfusion.XlsIO;

using Syncfusion.Drawing;


using Microsoft.Net.Http.Headers;


namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ConciliacionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        public ConciliacionController(IHostingEnvironment hostingEnvironment
            , ILogger<ConciliacionController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }

        class objeto
        {
            public ConciliacionDTO[] arreglo { get; set; }
            
        }



        // GET: Conciliacion
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Conciliacion()
        {
            ViewData["CheckAccount"] = await ObtenerCheckAccount();

            return View();
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetConciliacion([DataSourceRequest]DataSourceRequest request)
        {
            List<Conciliacion> _Conciliacion = new List<Conciliacion>();


            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Conciliacion = JsonConvert.DeserializeObject<List<Conciliacion>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Conciliacion.ToDataSourceResult(request);

        }
        [HttpGet("[controller]/[action]")]

        public async Task<DataSourceResult> GetConciliacionLineaByConciliacionId([DataSourceRequest]DataSourceRequest request, Int64 ConciliacionId)
        {
            List<ConciliacionLinea> _ConciliacionLineas = new List<ConciliacionLinea>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConciliacionLinea/GetConciliacionLineaByConciliacionId/" + ConciliacionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionLineas = JsonConvert.DeserializeObject<List<ConciliacionLinea>>(valorrespuesta);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return (_ConciliacionLineas.ToDataSourceResult(request));
        }



       

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddConciliacion([FromBody]Conciliacion _Conciliaciontp)
        {
            ConciliacionDTO _Conciliacion = new ConciliacionDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GetCustomerDocumentById/" + _Conciliaciontp.ConciliacionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Conciliacion = JsonConvert.DeserializeObject<ConciliacionDTO>(valorrespuesta);

                }

                if (_Conciliacion == null)
                {
                    _Conciliacion = new ConciliacionDTO();
                    _Conciliacion.DateBeginReconciled = DateTime.Now;
                    _Conciliacion.DateEndReconciled = DateTime.Now;
                    _Conciliacion.FechaConciliacion = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Conciliacion);

        }



       

        public async Task<ActionResult> Submit(IEnumerable<IFormFile> files, ConciliacionDTO _Conciliaciontp)
        {
            Int64 AccountId = _Conciliaciontp.AccountId;
            


            // Task<IActionResult> resultadoProcesoConciliacion  = new Task<IActionResult>;
            //
            //var resultadoProcesoConciliacion="";
            

            if (files != null)
            {

                Conciliacion resultadoProcesoConciliacion = await ProcesoConciliacion(files, _Conciliaciontp);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntry");


                ViewBag.resultado = resultadoProcesoConciliacion;

                ViewData["resultado"] = resultadoProcesoConciliacion;
                //TempData['resultado'] = resultadoProcesoConciliacion;
                return View("Result",resultadoProcesoConciliacion);
            }

            return View("Result");



        }
        public async Task<ActionResult> DetailsConciliation(Int64 ConciliacionId)
        {
            Conciliacion _ConciliacionP = new Conciliacion();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacionById/" + ConciliacionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _ConciliacionP = JsonConvert.DeserializeObject<Conciliacion>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_ConciliacionP));

            
        }
        public ActionResult Result()
        {
            return View();
        }
        public async Task<JsonResult> GetCheckAccountByAccountNumber(string AccountNumber)
        {
            CheckAccount _CheckAccountP = new CheckAccount();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountByAccountNumber/" + AccountNumber);
                string valorrespuesta = "";
                _CheckAccountP.FechaModificacion = DateTime.Now;
                _CheckAccountP.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountP = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CheckAccountP);
        }
        //IEnumerable<IFormFile>
        private async Task<ConciliacionDTO> ProcesoConciliacion(IEnumerable<IFormFile> files, ConciliacionDTO _ConciliacionP)
        {
            List<string> fileInfo = new List<string>();
            //Aigno la conciliacion para crearla
            ConciliacionDTO _NewConciliacionP = _ConciliacionP;
            _NewConciliacionP.ConciliacionLinea = new List<ConciliacionLinea>();

            ConciliacionDTO _JournalEntry = new ConciliacionDTO();
            //List<Accounting> _JournalEntryAccountName = new List<Accounting>();
            //Accounting _JournalEntryAccountName = new Accounting();




            foreach (var file in files)
            {
                var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));


                //A existing workbook is opened.              
                var filePath = _hostingEnvironment.WebRootPath + "/Conciliacion/" + fileName;

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                //Aqui va la funcionalidad con syncfusion
                //New instance of ExcelEngine is created 
                //Equivalent to launching Microsoft Excel with no workbooks open
                //Instantiate the spreadsheet creation engine
                ExcelEngine excelEngine = new ExcelEngine();

                //Instantiate the Excel application object
                IApplication application = excelEngine.Excel;

                //Assigns default application version
                application.DefaultVersion = ExcelVersion.Excel2016;

                //A existing workbook is opened.              
                FileStream sampleFile = new FileStream(filePath, FileMode.Open,FileAccess.ReadWrite);

                IWorkbook workbook = application.Workbooks.Open(sampleFile);

                //Access first worksheet from the workbook.
                IWorksheet worksheet = workbook.Worksheets[0];


                //OBTENER LA DATA NECESARIA

                //EL SIGUIENTE PROCEDIMIENTO RECORRE EL EXCEL POR MEDIO DE UN RANGO
                IMigrantRange migrantRange = worksheet.MigrantRange;


                int rowCount = worksheet.UsedRange.LastRow;
                int colCount = worksheet.UsedRange.LastColumn;

                //OBTENER LA DATA DE LA BASE DE DATOS A LA CUAL TENGO QUE COMPARAR
                string CuentaBancaria="";

                for (int ro = 2; ro < rowCount; ro++) {

                    ConciliacionLinea _ConciliacionLineaP = new ConciliacionLinea();
                    _ConciliacionLineaP.AccountId = _ConciliacionP.AccountId;
                    _ConciliacionLineaP.AccountName = _ConciliacionP.AccountName;
                    _ConciliacionLineaP.Credit = Convert.ToDouble(worksheet[ro, 7].Text);
                    _ConciliacionLineaP.Debit = Convert.ToDouble(worksheet[ro, 6].Text);
                    _ConciliacionLineaP.Monto = Convert.ToDouble(worksheet[ro,8 ].Text);
                    _ConciliacionLineaP.ReferenceTrans = worksheet[ro, 2].Text;
                    _ConciliacionLineaP.CurrencyId= Convert.ToInt32(worksheet[ro, 5].Text);
                    _ConciliacionLineaP.MonedaName = worksheet[ ro,9].Text;
                    
                    _ConciliacionLineaP.TransDate =Convert.ToDateTime(worksheet[ro, 1].Text);
                    _ConciliacionLineaP.ReferenciaBancaria= worksheet[ ro,3].Text;
                    _ConciliacionLineaP.UsuarioCreacion= HttpContext.Session.GetString("user");
                    _ConciliacionLineaP.UsuarioModificacion= HttpContext.Session.GetString("user");
                    _ConciliacionLineaP.FechaCreacion = DateTime.Now;
                    _ConciliacionLineaP.FechaModificacion = DateTime.Now;
                    CuentaBancaria = worksheet[ro,3].Text;
                    // for (int col = 1; col < colCount; col++) {
                    //   cadena=worksheet[col, ro].Text;
                    //}
                    _NewConciliacionP.ConciliacionLinea.Add(_ConciliacionLineaP);
                }

                //
                var CheckAccountP = await GetCheckAccountByAccountNumber(_NewConciliacionP.ConciliacionLinea[0].ReferenciaBancaria);
                CheckAccount CuentaCheque = new CheckAccount();
                CuentaCheque = ((CheckAccount)CheckAccountP.Value);

                _NewConciliacionP.CheckAccountId = CuentaCheque.CheckAccountId;
                _NewConciliacionP.BankId = CuentaCheque.BankId;
                _NewConciliacionP.BankName = CuentaCheque.BankName;
                _NewConciliacionP.FechaConciliacion = DateTime.Now;
                _NewConciliacionP.SaldoConciliado = 0;
                

                /*
                */

                //foreach (var item in _JournalEntry)
                //{
                //    //double verificaicon = item.TotalCredit;

                //    //string verificacion = item;
                //    Debit = item.Debit;
                //    Credit = item.Credit;
                //    //Console.WriteLine("Amount is {0} and type is {1}");
                //    Saldo = Convert.ToDouble(worksheet.Range["D8"].Number);

                //}

                // Saldo = Convert.ToDouble(worksheet.Range["D"+(rowCount - 1).ToString()].Number);




                //for (int i = 1; i <= rowCount; i++)
                //{
                //    string rango = worksheet.Range["D" + i.ToString()].Number;



                //}


                //=================================================================

                //Set Text in cell A3.
                //worksheet.Range["A3"].Text;

                // string variable = worksheet.Range["A"+"3"].Text;

                //Defining the ContentType for excel file.
                // string ContentType = "Application/msexcel";

                //Define the file name.
                //string fileoutput = "Output.xlsx";

                //Creating stream object.
                //MemoryStream newstream = new MemoryStream();

                //Saving the workbook to stream in XLSX format
                // workbook.SaveAs(newstream);

                //newstream.Position = 0;

                //Closing the workbook.
                 workbook.Close();

                //Dispose the Excel engine
                excelEngine.Dispose();

                //Creates a FileContentResult object by using the file contents, content type, and file name.
                //return File(newstream, ContentType, fileoutput);

            }



            //return Ok(new { 1, Credit });

            //return new ObjectResult(new DataSourceResult { Data = new[] { Credit }, Total = 1 });

            //return Json(new { foo = "bar", baz = "Blech" });

            //double saldito=23.3;
            //var resultado = new OkObjectResult(new { message = saldito, currentDate = DateTime.Now });
            //return resultado;
            return _NewConciliacionP;


        }


        
        /*public async Task<ActionResult<ConciliacionDTO>> Confirmacion([FromBody] dynamic dto)

        {

           ConciliacionDTO _Conciliacion = new ConciliacionDTO();
           List<ConciliacionLinea> _ConciliacionP = new List<ConciliacionLinea>();



            ////String[] tempArray;
            //var customer = _customer;

            //Conciliacion _conciliacion = _SubProductS;

            //_SubProductS = JsonConvert.DeserializeObject<SubProductDTO>(dto.ToString());

            try
            {
                //_Conciliacion = JsonConvert.DeserializeObject<ConciliacionDTO>(dto.ToString());

                _Conciliacion = JsonConvert.DeserializeObject<List<ConciliacionLinea>>(dto.ToString());

                //objeto _Conciliacion = JsonConvert.DeserializeObject<objeto>(dto.ToString());

               // var insertresult = await Insert(_Conciliacion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return await Task.Run(()=> Ok(_Conciliacion));

        }
        */
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Conciliacion>> Insert(ConciliacionDTO _ConciliacionP)
        {
            Conciliacion _ConciliacionBanco = _ConciliacionP;

            //List<ConciliacionLinea> _Conciliacionq = new List<ConciliacionLinea>();
            //_Conciliacionq = _ConciliacionP;
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ConciliacionBanco.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ConciliacionBanco.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Conciliacion/Insert", _ConciliacionBanco);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionP = JsonConvert.DeserializeObject<ConciliacionDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_ConciliacionP);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Conciliacion }, Total = 1 });
        }
        async Task<IEnumerable<CheckAccount>> ObtenerCheckAccount()
        {
            IEnumerable<CheckAccount> CheckAccount = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    CheckAccount = JsonConvert.DeserializeObject<IEnumerable<CheckAccount>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["defaultcheckaccount"] = CheckAccount.FirstOrDefault();
            return CheckAccount;

        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ConciliacionDTO>> Update(Int64 id, ConciliacionDTO _ConciliacionP)
        {
            ConciliacionDTO _ConciliacionDTO = new ConciliacionDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Conciliacion/Update", _ConciliacionP);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionDTO = JsonConvert.DeserializeObject<ConciliacionDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ConciliacionDTO }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Conciliacion>> Delete([FromBody]Conciliacion _Conciliacion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Conciliacion/Delete", _Conciliacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Conciliacion = JsonConvert.DeserializeObject<Conciliacion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Conciliacion }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Conciliacion>> SaveConciliacion([FromBody]ConciliacionDTO _ConciliacionDTO)
        {

            try
            {
               // var Conciliacionvar = await Submit(files, _ConciliacionDTO);

                Conciliacion _listConciliacion = _ConciliacionDTO;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacionById/" + _ConciliacionDTO.ConciliacionId);
                string valorrespuesta = "";

                //foreach (var file in files)
                //{


                   // FileInfo info = new FileInfo(file.FileName);
                    //if (
                     //   info.Extension.Equals(".xls") || info.Extension.Equals(".xlsx"))
                    //{

                        _ConciliacionDTO.FechaModificacion = DateTime.Now;
                        _ConciliacionDTO.UsuarioModificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listConciliacion = JsonConvert.DeserializeObject<Conciliacion>(valorrespuesta);
                        }

                        if (_listConciliacion == null) { _listConciliacion = new Models.Conciliacion(); }
                        if (_listConciliacion.ConciliacionId == 0)
                        {
                    //  ConciliacionDTO NuevaConciliacion = await ProcesoConciliacion(files, _ConciliacionDTO);
                    //                         NuevaConciliacion = ((Conciliacion)Conciliacionvar.va);



                    //NuevaConciliacion.FechaCreacion = DateTime.Now;
                    // NuevaConciliacion.UsuarioCreacion = HttpContext.Session.GetString("user");

                    //var insertresult = await Insert(NuevaConciliacion);
                    _ConciliacionDTO.FechaCreacion = DateTime.Now;
                    _ConciliacionDTO.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_ConciliacionDTO);
                        var value = ((ConciliacionDTO)insertresult.Value);
                           

                            _ConciliacionDTO = value;
                        }
                        else
                        {
                            var updateresult = await Update(_ConciliacionDTO.ConciliacionId, _ConciliacionDTO);
                        }



                     /*   var filePath = _hostingEnvironment.WebRootPath + "/Conciliacion/" + _ConciliacionDTO.ConciliacionId + "_"
                            + file.FileName.Replace(info.Extension, "") + "_"  + file.FileName
                            + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // MemoryStream mstream = new MemoryStream();
                            //mstream.WriteTo(stream);
                        }
                        */
                        //_ConciliacionDTO.Path = filePath;
                        // var updateresult2 = await Update(_ConciliacionDTO.ConciliacionId, _InsurancesDTO);
                    //}
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ConciliacionDTO }, Total = 1 });
            //return Json(_ConciliacionDTO);

        }

    }
    

}