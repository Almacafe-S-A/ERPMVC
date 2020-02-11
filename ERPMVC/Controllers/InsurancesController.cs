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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class InsurancesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        public InsurancesController(IHostingEnvironment hostingEnvironment
            , ILogger<InsurancesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Insurances()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddInsurances([FromBody]Insurances _InsurancesDocumentp)
        {
            Insurances _InsurancesDocument = new Insurances();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _InsurancesDocumentp.InsurancesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesDocument = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);

                }

                if (_InsurancesDocument == null)
                {
                    _InsurancesDocument = new Insurances();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            //
            return PartialView(_InsurancesDocument);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Insurances> _Insurances = new List<Insurances>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurances");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<List<Insurances>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Insurances.ToDataSourceResult(request);

        }


        /*[HttpGet("[action]")]
        public async Task<DataSourceResult> GeDocumentByCustomerId([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
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

    */


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InsurancesDTO>> SaveInsurancesDocument(IEnumerable<IFormFile> files, InsurancesDTO _InsurancesP)
        {
            Insurances _Insurances = _InsurancesP;
            Insurances _Insurances1 = new Insurances();

            try
            {
                Insurances _listInsurances = new Insurances();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _InsurancesP.InsurancesId);
                //var result2 = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _InsurancesP.InsurancesId);
                //string valorrespuesta2 = "";
                //if (result2.IsSuccessStatusCode)
                //{
                //    valorrespuesta2 = await (result2.Content.ReadAsStringAsync());
                //    _Insurances1 = JsonConvert.DeserializeObject<Insurances>(valorrespuesta2);
                //}
                //if (_Insurances1 == null) { _Insurances1 = new Models.Insurances(); }
                var result1 = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesByInsurancesName/" + _InsurancesP.InsurancesName);
                string valorrespuesta1 = "";
                IFormFile file = files.FirstOrDefault();


                _InsurancesP.ModifiedDate = DateTime.Now;
                _InsurancesP.ModifiedUser = HttpContext.Session.GetString("user");
                if (result1.IsSuccessStatusCode)
                {
                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta1);
                }

                if (_Insurances == null) { _Insurances = new Models.Insurances(); }

                if (_Insurances.InsurancesId > 0)
                {
                    if (_Insurances.InsurancesId != _InsurancesP.InsurancesId)
                        return await Task.Run(() => BadRequest($"Ya existe una Aseguradora registrada con ese nombre."));
                }

                if (_InsurancesP.InsurancesId == 0)
                {
                    _InsurancesP.CreatedDate = DateTime.Now;
                    _InsurancesP.DocumentName = file.FileName;
                    _InsurancesP.CreatedUser = HttpContext.Session.GetString("user");
                    _InsurancesP.DocumentTypeId = 53;
                    _InsurancesP.DocumentTypeName = "Foto";
                    var insertresult = await Insert(_InsurancesP);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _InsurancesP = ((InsurancesDTO)(value));
                }

                if (file != null)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".jpeg") || info.Extension.Equals(".jpg")
                        || info.Extension.Equals(".png") || info.Extension.Equals(".gif"))
                    {

                        if (System.IO.File.Exists(_Insurances.Path))
                        {
                            System.IO.File.Delete(_Insurances.Path);
                        }
                        //_InsurancesP.DocumentName = file.FileName;
                        //_InsurancesP.CreatedDate = _Insurances.CreatedDate;
                        ////_InsurancesP.CreatedUser = _Insurances.CreatedUser;

                    }
                    else
                    {
                        return await Task.Run(() => BadRequest($"Extensión de Imagen no permitida."));
                    }

                    var filePath = _hostingEnvironment.WebRootPath + "/Insurances/" + _InsurancesP.InsurancesId + "_"
                        + file.FileName.Replace(info.Extension, "") + "_" + _InsurancesP.DocumentTypeId + "_" + _InsurancesP.DocumentTypeName
                        + info.Extension;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _InsurancesP.Path = filePath;
                    //_InsurancesP.CreatedUser = _Insurances1.CreatedUser;
                    var updateresult2 = await Update(_Insurances.InsurancesId, _InsurancesP);
                }

                else
                {
                    var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _InsurancesP.InsurancesId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                    }

                    _InsurancesP.CreatedDate = _Insurances.CreatedDate;
                    _InsurancesP.CreatedUser = _Insurances.CreatedUser;
                    _InsurancesP.DocumentName = _Insurances.DocumentName;
                    _InsurancesP.DocumentTypeName = _Insurances.DocumentTypeName;



                    var updateresult = await Update(_InsurancesP.InsurancesId, _InsurancesP);

                    if (file == null & _Insurances.Path == null)
                {
                   
                        return await Task.Run(() => BadRequest($"Seleccione una Imagen."));
                    
                }

                
                    
                }

                 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_InsurancesP);
        }

        // POST: CustomerDocument/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsurancesDTO>> Insert(InsurancesDTO _InsurancesDTO)
        {
            InsurancesDTO _custo = new InsurancesDTO();
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsurancesDTO.CreatedUser = HttpContext.Session.GetString("user");
                _InsurancesDTO.CreatedDate = DateTime.Now;
                _InsurancesDTO.ModifiedUser = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Insurances/Insert", _InsurancesDTO);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _custo = JsonConvert.DeserializeObject<InsurancesDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_custo);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerDocument }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsurancesDTO>> Update(Int64 id, InsurancesDTO _InsurancesDocument)
        {
            InsurancesDTO _InsurancesDTO = new InsurancesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsurancesDocument.ModifiedDate = DateTime.Now;
                _InsurancesDocument.ModifiedUser = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Insurances/Update", _InsurancesDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesDTO = JsonConvert.DeserializeObject<InsurancesDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesDocument }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Insurances>> Delete([FromBody]Insurances _InsurancesDocument)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Insurances/Delete", _InsurancesDocument);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesDocument = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesDocument }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<Insurances>> DeleteInsuranceDocument(Int64 Id, [FromBody]Insurances _InsurancesP)
        {
            Insurances _Insurances = _InsurancesP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Insurances/Delete", _Insurances);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                    if (System.IO.File.Exists(_Insurances.Path))
                    {
                        System.IO.File.Delete(_Insurances.Path);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _Insurances }, Total = 1 });
        }


    }
}