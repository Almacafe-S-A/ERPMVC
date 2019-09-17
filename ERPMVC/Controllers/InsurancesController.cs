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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class InsurancesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;

        public InsurancesController(IHostingEnvironment hostingEnvironment,ILogger<InsurancesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Insurances()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetInsurances([DataSourceRequest]DataSourceRequest request)
        {
            List<Insurances> _Insurances = new List<Insurances>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurances" );
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



            return Json(_Insurances.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult<InsurancesDTO>> Insert(Insurances _Insurances)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Insurances.CreatedUser = HttpContext.Session.GetString("user");
                _Insurances.CreatedDate = DateTime.Now;
                _Insurances.ModifiedUser = HttpContext.Session.GetString("user");
                _Insurances.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Insurances/Insert", _Insurances);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Insurances }, Total = 1 });
        }


        [HttpPut("InsurancesId")]
        public async Task<IActionResult> Update(Int64 InsurancesId, Insurances _Insurances)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Insurances/Update", _Insurances);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Insurances }, Total = 1 });
        }
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Insurances>> SaveInsurances(IEnumerable<IFormFile> files, InsurancesDTO _Insurances)
        {
             try
            {
                //JournalEntry _listItems = new JournalEntry();
                Insurances _listInsurances = new Insurances();

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _Insurances.InsurancesId);
                string valorrespuesta = "";
                _Insurances.ModifiedDate = DateTime.Now;
                _Insurances.ModifiedUser = HttpContext.Session.GetString("user");
                foreach (var file in files)
                {


                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".pdf") || info.Extension.Equals(".jpg")
                        || info.Extension.Equals(".png")
                       || info.Extension.Equals(".xls") || info.Extension.Equals(".xlsx"))
                    {

                        _Insurances.ModifiedDate = DateTime.Now;
                        _Insurances.ModifiedUser = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {

                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listInsurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                        }

                        if (_listInsurances == null) { _listInsurances = new Models.Insurances(); }
                        if (_listInsurances.InsurancesId == 0)
                        {
                            _Insurances.CreatedDate = DateTime.Now;
                            _Insurances.DocumentName = file.FileName;
                            _Insurances.CreatedUser = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_Insurances);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _Insurances = ((InsurancesDTO)(value));
                        }
                        else
                        {
                            var updateresult = await Update(_Insurances.InsurancesId, _Insurances);
                        }



                        var filePath = _hostingEnvironment.WebRootPath + "/Insurances/" + _Insurances.InsurancesId + "_"
                            + file.FileName.Replace(info.Extension, "") + "_" + _Insurances.DocumentTypeId + "_" + _Insurances.DocumentTypeName
                            + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            // MemoryStream mstream = new MemoryStream();
                            //mstream.WriteTo(stream);
                        }

                        _Insurances.Path = filePath;
                        var updateresult2 = await Update(_Insurances.InsurancesId, _Insurances);
                    }
                }

                /* if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);
                }

                if (_Insurances == null) { _Insurances = new Models.Insurances(); }

                if (_InsurancesP.InsurancesId == 0)
                {
                    _Insurances.CreatedDate = DateTime.Now;
                    _Insurances.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsurancesP);
                }
                else
                {
                    _InsurancesP.CreatedUser = _Insurances.CreatedUser;
                    _InsurancesP.CreatedDate = _Insurances.CreatedDate;
                    var updateresult = await Update(_Insurances.InsurancesId, _InsurancesP);
                }*/

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Insurances);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddInsurances([FromBody]InsurancesDTO _sarpara)
        {
            InsurancesDTO _Insurances = new InsurancesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + _sarpara.InsurancesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<InsurancesDTO>(valorrespuesta);

                }

                if (_Insurances == null)
                {
                    _Insurances = new InsurancesDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Insurances);

        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetInsurancesById(Int64 InsurancesId)
        {
            Insurances _Insurances = new Insurances();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Insurances/GetInsurancesById/" + InsurancesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Insurances = JsonConvert.DeserializeObject<Insurances>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_Insurances));
        }

    }
}