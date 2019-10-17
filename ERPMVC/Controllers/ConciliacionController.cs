﻿using System;
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


        //public ConciliacionController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        //{
        //    this.config = config;
        //    this._logger = logger;
        //}


        // GET: Conciliacion
        public ActionResult Conciliacion()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetConciliacion([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<CustomerDocument> _CustomerDocument = new List<CustomerDocument>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerDocument/GeDocumentByCustomerId/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerDocument = JsonConvert.DeserializeObject<List<CustomerDocument>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerDocument.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddConciliacion([FromBody]Conciliacion _Conciliaciontp)
        {
            Conciliacion _Conciliacion = new Conciliacion();
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
                    _Conciliacion = JsonConvert.DeserializeObject<Conciliacion>(valorrespuesta);

                }

                if (_Conciliacion == null)
                {
                    _Conciliacion = new Conciliacion();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Conciliacion);

        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Conciliacion>> SaveConciliacion(List<IFormFile> files, ConciliacionDTO _Conciliaciontp)
        {

            try
            {

                Conciliacion _listConciliacion = new Conciliacion();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacionById/" + _Conciliaciontp.ConciliacionId);
                string valorrespuesta = "";

                foreach (var file in files)
                {


                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpg")
                        || info.Extension.Equals(".png")
                       || info.Extension.Equals(".xls") || info.Extension.Equals(".xlsx"))
                    {

                        _Conciliaciontp.FechaModificacion = DateTime.Now;
                        _Conciliaciontp.UsuarioModificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listConciliacion = JsonConvert.DeserializeObject<Conciliacion>(valorrespuesta);
                        }

                        if (_listConciliacion == null) { _listConciliacion = new Models.Conciliacion(); }
                        if (_listConciliacion.ConciliacionId == 0)
                        {
                            _Conciliaciontp.FechaCreacion = DateTime.Now;
                            //_Conciliacion.DocumentName = file.FileName;
                            _Conciliaciontp.UsuarioCreacion = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_Conciliaciontp);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _Conciliaciontp = ((ConciliacionDTO)(value));
                        }
                        else
                        {
                            var updateresult = await Update(_Conciliaciontp.ConciliacionId, _Conciliaciontp);
                        }



                        //var filePath = _hostingEnvironment.WebRootPath + "/Conciliacions/" + _Conciliacion.ConciliacionId + "_"
                        //    + file.FileName.Replace(info.Extension, "") + "_" + _Conciliacion.DocumentTypeId + "_" + _Conciliacion.DocumentTypeName
                        //    + info.Extension;

                        //using (var stream = new FileStream(filePath, FileMode.Create))
                        //{
                        //    await file.CopyToAsync(stream);
                        //    // MemoryStream mstream = new MemoryStream();
                        //    //mstream.WriteTo(stream);
                        //}

                        //_Conciliacion.Path = filePath;
                        var updateresult2 = await Update(_Conciliaciontp.ConciliacionId, _Conciliaciontp);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Conciliaciontp);
        }

        public async Task<ActionResult> Submit(List<IFormFile> files, ConciliacionDTO _Conciliaciontp)
        {
            if (files != null)
            {
                await ProcesoConciliacion(files);
            }

            

            return View("Result");
        }

        public ActionResult Result()
        {
            return View();
        }

        private async Task<IActionResult> ProcesoConciliacion(List<IFormFile> files)
        {
            List<string> fileInfo = new List<string>();

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
                FileStream sampleFile = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(sampleFile);

                //Access first worksheet from the workbook.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Set Text in cell A3.
                //worksheet.Range["A3"].Text;

                string variable = worksheet.Range["A2"].Text;

                //Defining the ContentType for excel file.
                string ContentType = "Application/msexcel";

                //Define the file name.
                string fileoutput = "Output.xlsx";

                //Creating stream object.
                MemoryStream newstream = new MemoryStream();

                //Saving the workbook to stream in XLSX format
                workbook.SaveAs(newstream);

                newstream.Position = 0;

                //Closing the workbook.
                workbook.Close();

                //Dispose the Excel engine
                excelEngine.Dispose();

                //Creates a FileContentResult object by using the file contents, content type, and file name.
                return File(newstream, ContentType, fileoutput);

            }



            return Ok(new {  });

        }

        //public async Task<IActionResult> ProcesoConciliacion(List<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);
        //    var filePath = _hostingEnvironment.WebRootPath + "/Conciliacion/" + files;

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
                    

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    // Process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok(new { count = files.Count, size, filePath });
        //}



        //[HttpGet]
        //public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        //{
        //    List<Conciliacion> _Conciliacion = new List<Conciliacion>();
        //    try
        //    {

        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/Conciliacion/GetConciliacion");
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _Conciliacion = JsonConvert.DeserializeObject<List<Conciliacion>>(valorrespuesta);

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }


        //    return _Conciliacion.ToDataSourceResult(request);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ConciliacionDTO>> Insert(ConciliacionDTO _Conciliacion)
        {
            ConciliacionDTO _custo = new ConciliacionDTO();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Conciliacion.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Conciliacion.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Conciliacion/Insert", _Conciliacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _custo = JsonConvert.DeserializeObject<ConciliacionDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_custo);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Conciliacion }, Total = 1 });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ConciliacionDTO>> Update(Int64 id, ConciliacionDTO _Conciliacion)
        {
            ConciliacionDTO _ConciliacionDTO = new ConciliacionDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Conciliacion/Update", _Conciliacion);
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

    }
}