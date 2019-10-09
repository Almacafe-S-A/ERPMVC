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
        public async Task<ActionResult<Insurances>> SaveInsurancesDocument(IEnumerable<IFormFile> files, InsurancesDTO _InsurancesDTO)
        {

            try
            {

                Insurances _listInsurances = new Insurances();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _InsurancesDTO.InsurancesId);
                string valorrespuesta = "";

                foreach (var file in files)
                {


                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpg")
                        || info.Extension.Equals(".png")
                       || info.Extension.Equals(".xls") || info.Extension.Equals(".xlsx"))
                    {

                        _InsurancesDTO.ModifiedDate = DateTime.Now;
                        _InsurancesDTO.ModifiedUser = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listInsurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                        }

                        if (_listInsurances == null) { _listInsurances = new Models.Insurances(); }
                        if (_listInsurances.InsurancesId == 0)
                        {
                            _InsurancesDTO.CreatedDate = DateTime.Now;
                            _InsurancesDTO.DocumentName = file.FileName;
                            _InsurancesDTO.CreatedUser = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_InsurancesDTO);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _InsurancesDTO = ((InsurancesDTO)(value));
                        }
                        else
                        {
                            var updateresult = await Update(_InsurancesDTO.InsurancesId, _InsurancesDTO);
                        }



                        var filePath = _hostingEnvironment.WebRootPath + "/Insurances/" + _InsurancesDTO.InsurancesId + "_"
                            + file.FileName.Replace(info.Extension, "") + "_" + _InsurancesDTO.DocumentTypeId + "_" + _InsurancesDTO.DocumentTypeName
                            + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // MemoryStream mstream = new MemoryStream();
                            //mstream.WriteTo(stream);
                        }

                        _InsurancesDTO.Path = filePath;
                        var updateresult2 = await Update(_InsurancesDTO.InsurancesId, _InsurancesDTO);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InsurancesDTO);
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





    }
}