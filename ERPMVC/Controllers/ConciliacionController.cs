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
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;


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
        private readonly ClaimsPrincipal _principal;
        public ConciliacionController(IHostingEnvironment hostingEnvironment
            , ILogger<ConciliacionController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _principal = httpContextAccessor.HttpContext.User;
        }

        class objeto
        {
            public ConciliacionDTO[] arreglo { get; set; }
            
        }



        // GET: Conciliacion
        public async Task<IActionResult> Conciliacion()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetConciliacion([DataSourceRequest]DataSourceRequest request)
        {
            List<ConciliacionDTO> _Conciliacion = new List<ConciliacionDTO>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacionConCuenta");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Conciliacion = JsonConvert.DeserializeObject<List<ConciliacionDTO>>(valorrespuesta);
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
                if (_ConciliacionLineas.Count == 0)
                {
                    Conciliacion _ConciliacionP = new Conciliacion();
                    
                        string baseadressP = config.Value.urlbase;
                        HttpClient _clientP = new HttpClient();
                        _clientP.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var resultP = await _client.GetAsync(baseadressP + "api/Conciliacion/GetConciliacionById/" + ConciliacionId);
                        string valorrespuestaP = "";
                        if (resultP.IsSuccessStatusCode)
                        {
                            valorrespuestaP = await (resultP.Content.ReadAsStringAsync());
                            _ConciliacionP = JsonConvert.DeserializeObject<Conciliacion>(valorrespuestaP);

                        }
                    string baseadressE = config.Value.urlbase;
                    HttpClient _clientE = new HttpClient();
                    _clientE.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultE = await _client.PostAsJsonAsync(baseadressE + "api/Conciliacion/GetJournalEntryByDateAccount",  _ConciliacionP);
                    string valorrespuestaE = "";
                    if (resultE.IsSuccessStatusCode)
                    {
                        valorrespuestaE = await (resultE.Content.ReadAsStringAsync());
                        _ConciliacionP.ConciliacionLinea = JsonConvert.DeserializeObject<List<ConciliacionLinea>>(valorrespuestaE);

                    }

                    _ConciliacionLineas = _ConciliacionP.ConciliacionLinea;
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
        public async Task<ActionResult> pvwAddConciliacion([FromBody]ConciliacionDTO _Conciliaciontp)
        {
            ConciliacionDTO _Conciliacion=null;
            List<MotivoConciliacion> motivos = new List<MotivoConciliacion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacionById/" + _Conciliaciontp.ConciliacionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    if (!string.IsNullOrEmpty(valorrespuesta))
                    {
                        _Conciliacion = JsonConvert.DeserializeObject<ConciliacionDTO>(valorrespuesta);
                        _Conciliacion.Editar = _Conciliaciontp.Editar;
                    }
                }

                if (_Conciliacion == null)
                {
                    _Conciliacion = new ConciliacionDTO();
                    _Conciliacion.DateBeginReconciled = DateTime.Now;
                    _Conciliacion.DateEndReconciled = DateTime.Now;
                    _Conciliacion.FechaConciliacion = DateTime.Now;
                    _Conciliacion.Editar = 1;
                }

                
                result = await _client.GetAsync(baseadress + "api/MotivoConciliacion/GetMotivosConciliacion");
                valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    if (!string.IsNullOrEmpty(valorrespuesta))
                    {
                        motivos = JsonConvert.DeserializeObject<List<MotivoConciliacion>>(valorrespuesta);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            ViewData["motivos"] = motivos;
            return PartialView(_Conciliacion);

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
        /*private async Task<ConciliacionDTO> ProcesoConciliacion(IEnumerable<IFormFile> files, ConciliacionDTO _ConciliacionP)
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
                

                
                 workbook.Close();

                
                excelEngine.Dispose();

                

            }
            return _NewConciliacionP;


        }*/


        
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
        public async Task<ActionResult<Conciliacion>> SaveConciliacion([FromBody]dynamic _ConciliacionDTO)
        {
            try
            {  
                Conciliacion conciliacion = JsonConvert.DeserializeObject<Conciliacion>(_ConciliacionDTO.ToString());
                conciliacion.FechaCreacion = DateTime.Now;
                conciliacion.FechaModificacion = DateTime.Now;
                conciliacion.UsuarioCreacion = HttpContext.Session.GetString("user");
                conciliacion.UsuarioModificacion = HttpContext.Session.GetString("user");
                foreach (var linea in conciliacion.ConciliacionLinea)
                {
                    linea.UsuarioCreacion = HttpContext.Session.GetString("user");
                    linea.UsuarioModificacion = HttpContext.Session.GetString("user");
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                HttpResponseMessage result = null;
                if (conciliacion.ConciliacionId == 0)
                {
                    result = await _client.PostAsJsonAsync(baseadress + "api/Conciliacion/Insert", conciliacion);
                }
                else
                {
                    result = await _client.PutAsJsonAsync(baseadress + "api/Conciliacion/Update", conciliacion);
                }
                 
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    conciliacion = JsonConvert.DeserializeObject<Conciliacion>(valorrespuesta);
                }
                return new ObjectResult(new DataSourceResult { Data = new[] { conciliacion }, Total = 1 });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
        }

    }
    

}