using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERPMVC.Models;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using ERPMVC.DTO;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class InsuranceEndorsementController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public InsuranceEndorsementController(IHostingEnvironment hostingEnvironment
            , ILogger<InsuranceEndorsementController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Contabilidad.Seguros.Seguros Endosados")]
        public IActionResult InsuranceEndorsement()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SFSegurosEndosados()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwAddInsuranceEndorsement([FromBody]InsuranceEndorsement _InsuranceEndorsementp)
        {
            InsuranceEndorsement _InsuranceEndorsement = new InsuranceEndorsement();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsement/GetInsuranceEndorsementById/" + _InsuranceEndorsementp.InsuranceEndorsementId);
                string valorrespuesta = "";
                _InsuranceEndorsement.DateGenerated = DateTime.Now;
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<InsuranceEndorsement>(valorrespuesta);

                }

                if (_InsuranceEndorsement == null)
                {
                    _InsuranceEndorsement = new InsuranceEndorsement ();
                    _InsuranceEndorsement.DateGenerated = DateTime.Now;
                }
                else
                {
                    // _InsuranceEndorsement.NumeroDEIString = $"{_InsuranceEndorsement.Sucursal}-{_InsuranceEndorsement.Caja}-01-{_InsuranceEndorsement.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InsuranceEndorsement);

        }



        //COMIENZA APROVACIÓN
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InsuranceEndorsement>> Recibido([FromBody]InsuranceEndorsement _InsuranceEndorsement)
        {
            InsuranceEndorsement _so = new InsuranceEndorsement();
            if (_InsuranceEndorsement != null)
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsement/GetInsuranceEndorsementById/" + _InsuranceEndorsement.InsuranceEndorsementId);
                    string jsonresult = "";
                    jsonresult = JsonConvert.SerializeObject(_InsuranceEndorsement);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<InsuranceEndorsement>(valorrespuesta);
                        _so.DateGenerated = DateTime.Now;

                        var resultsalesorder = await Update(_so.InsuranceEndorsementId, _so);

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        InsuranceEndorsement resultado = ((InsuranceEndorsement)(value));
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

            return await Task.Run(() => Ok(_so));
        }

        //TERMINA

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InsuranceEndorsement> _InsuranceEndorsement = new List<InsuranceEndorsement>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsement/GetInsuranceEndorsement");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<List<InsuranceEndorsement>>(valorrespuesta);
                    _InsuranceEndorsement = _InsuranceEndorsement.OrderByDescending(q => q.InsuranceEndorsementId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsuranceEndorsement.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetByInsuracePolicyId([DataSourceRequest] DataSourceRequest request, int insurancePolicyId)
        {
            List<InsuranceEndorsement> _InsuranceEndorsement = new List<InsuranceEndorsement>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsement/GetInsuranceEndorsementByInsurancePolicyId/"+ insurancePolicyId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<List<InsuranceEndorsement>>(valorrespuesta);
                    _InsuranceEndorsement = _InsuranceEndorsement.OrderByDescending(q => q.InsuranceEndorsementId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsuranceEndorsement.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuranceEndorsementDTO>> SaveInsuranceEndorsement(IEnumerable<IFormFile> files, InsuranceEndorsement _InsuranceEndorsement)
        {

            try
            {
                InsuranceEndorsement _listInsuranceEndorsement = new InsuranceEndorsement();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsement/GetInsuranceEndorsementById/" + _InsuranceEndorsement.InsuranceEndorsementId);
                string valorrespuesta = "";
                IFormFile file = files.FirstOrDefault();
                _InsuranceEndorsement.FechaModificacion = DateTime.Now;
                _InsuranceEndorsement.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInsuranceEndorsement = JsonConvert.DeserializeObject<InsuranceEndorsement>(valorrespuesta);
                }

                if (_listInsuranceEndorsement == null) { _listInsuranceEndorsement = new InsuranceEndorsement(); }

                if (_listInsuranceEndorsement.InsuranceEndorsementId == 0)
                {
                    _InsuranceEndorsement.FechaCreacion = DateTime.Now;
                    _InsuranceEndorsement.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsuranceEndorsement);
                    var value = (insertresult.Result as ObjectResult).Value;

                    InsuranceEndorsementDTO resultado = ((InsuranceEndorsementDTO)(value));
                    if (resultado.InsuranceEndorsementId <= 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero la factura!"));
                    }
                    else
                    {
                        // _InsuranceEndorsement.NumeroDEIString = $"{resultado.Sucursal}-01-{resultado.NumeroDEI.ToString().PadLeft(8, '0')} ";
                    }

                }
                else
                {
                    var updateresult = await Update(_InsuranceEndorsement.InsuranceEndorsementId, _InsuranceEndorsement);
                }
                if (file != null)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    var filePath = _hostingEnvironment.WebRootPath + "\\InsuranceEndorsement\\Endoso_" + _InsuranceEndorsement.InsuranceEndorsementId + info.Extension;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _InsuranceEndorsement.AttachmentURL = filePath;
                    _InsuranceEndorsement.AttachementFileName = file.FileName;
                    _listInsuranceEndorsement = _InsuranceEndorsement;
                    var updateresult2 = await Update(_InsuranceEndorsement.InsuranceEndorsementId, _InsuranceEndorsement);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"No se genero la factura : {ex.ToString()}"));

                throw ex;
            }

            return Json(_InsuranceEndorsement);
        }

        // POST: InsuranceEndorsement/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsuranceEndorsement>> Insert(InsuranceEndorsement _InsuranceEndorsement)
        {
            try
            {
                // TODO: Add insert logic here
                _InsuranceEndorsement.EstadoId = 1;
                _InsuranceEndorsement.Estado = "Activo";
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsuranceEndorsement.UsuarioCreacion = HttpContext.Session.GetString("user");
                _InsuranceEndorsement.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuranceEndorsement/Insert", _InsuranceEndorsement);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<InsuranceEndorsementDTO>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (ex);
                // return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_InsuranceEndorsement);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuranceEndorsement }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsuranceEndorsement>> Update(Int64 id, InsuranceEndorsement _InsuranceEndorsement)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InsuranceEndorsement/Update", _InsuranceEndorsement);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<InsuranceEndorsement>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_InsuranceEndorsement));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuranceEndorsement }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuranceEndorsement>> Delete([FromBody]InsuranceEndorsement _InsuranceEndorsement)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuranceEndorsement/Delete", _InsuranceEndorsement);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuranceEndorsement = JsonConvert.DeserializeObject<InsuranceEndorsement>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(() => Ok(_InsuranceEndorsement));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuranceEndorsement }, Total = 1 });
        }



        [HttpGet]
        public async Task<ActionResult> SFInsuranceEndorsement(Int32 id)
        {
            try
            {
                InsuranceEndorsement _InsuranceEndorsement = new InsuranceEndorsement { InsuranceEndorsementId = id, };
                return await Task.Run(() => View(_InsuranceEndorsement));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }


        public async Task<IActionResult> SFLibroCompras()
        {
            return await Task.Run(() => View());

        }
    }
}