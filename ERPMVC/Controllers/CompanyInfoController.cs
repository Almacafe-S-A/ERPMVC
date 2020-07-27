using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class CompanyInfoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public CompanyInfoController(IHostingEnvironment hostingEnvironment
            , ILogger<CompanyInfoController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _principal = httpContextAccessor.HttpContext.User;
        }
        //public CompanyInfoController(ILogger<CompanyInfoController> logger, IOptions<MyConfig> config)
        //{
        //    this.config = config;
        //    this._logger = logger;
        //}
        [Authorize(Policy = "Configuracion.Informacion de la Empresa")]
        public IActionResult CompanyInfo()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Configuracion.Informacion de la Empresa.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Configuracion.Informacion de la Empresa.Editar", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Configuracion.Informacion de la Empresa.Eliminar", "true");
            return View();
        }
        [Authorize(Policy = "Configuracion.Informacion de la Empresa")]
        public async Task<ActionResult> pvwAddCompanyInfo()

        {
            CompanyInfo _sarpara = new CompanyInfo { CompanyInfoId = 1 };
            CompanyInfoDTO _CompanyInfo = new CompanyInfoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfoById/" + _sarpara.CompanyInfoId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfoDTO>(valorrespuesta);

                }
                if (_CompanyInfo == null)
                {
                    _CompanyInfo = new CompanyInfoDTO();
                }          
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return View(_CompanyInfo);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CompanyInfo> _CompanyInfo = new List<CompanyInfo>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<List<CompanyInfo>>(valorrespuesta);
                    _CompanyInfo= _CompanyInfo.OrderByDescending(q => q.CompanyInfoId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CompanyInfo.ToDataSourceResult(request);

        }


        [HttpPost]
        public async Task<ActionResult> GetCompanyByRTN([FromBody]CompanyInfo _Companyp)
        {
            CompanyInfo _Company = new CompanyInfo();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CompanyInfo/GetCompanyByRTN", _Companyp);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Company = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }
                if (_Company == null)
                {
                    _Company = new CompanyInfo();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_Company);
        }


        [HttpPost]
        public async Task<ActionResult> GetCompanyByRTNMANAGER([FromBody]CompanyInfo _Companyp)
        {
            CompanyInfo _Company = new CompanyInfo();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CompanyInfo/GetCompanyByRTNMANAGER", _Companyp);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Company = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }
                if (_Company == null)
                {
                    _Company = new CompanyInfo();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_Company);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CompanyInfoDTO>> SaveCompanyInfo(IEnumerable<IFormFile> files, CompanyInfoDTO _CompanyInfoS)
        {
            CompanyInfo _CompanyInfo = _CompanyInfoS;
            List<CompanyInfo> _listCompanyInfoValidation = new List<CompanyInfo>();
            try
            {               
                CompanyInfo _listCompanyInfo = new CompanyInfo();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfoById/" + _CompanyInfo.CompanyInfoId);
                string valorrespuesta = "";            
                _CompanyInfo.FechaModificacion = DateTime.Now;
                _CompanyInfo.UsuarioModificacion = HttpContext.Session.GetString("user");
                IFormFile file = files.FirstOrDefault();
                
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }
                if (file != null)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    var filename = _CompanyInfoS.CompanyInfoId + "_" + _CompanyInfoS.Company_Name + info.Extension;
                    var filePath = _hostingEnvironment.WebRootPath + "/CompanyImages/" + filename;
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    _CompanyInfoS.image = filename;
                    _CompanyInfoS.imageDir = filePath;
                   
                }
                var updateresult2 = await Update(_CompanyInfoS);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CompanyInfoS);
        }

        // POST: CompanyInfo/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CompanyInfo>> Insert(CompanyInfo _CompanyInfo)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CompanyInfo.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CompanyInfo.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CompanyInfo/Insert", _CompanyInfo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return _CompanyInfo;
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }

        [HttpPut("{CompanyInfoId}")]
        public async Task<ActionResult<CompanyInfo>> Update( CompanyInfo _CompanyInfop)
        {
            CompanyInfo _CompanyInfo = _CompanyInfop;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CompanyInfo.FechaModificacion = DateTime.Now;
                _CompanyInfo.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/CompanyInfo/Update", _CompanyInfo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<CompanyInfo>> Delete(Int64 CompanyInfo, CompanyInfo _CompanyInfo)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CompanyInfo/Delete", _CompanyInfo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(valorrespuesta);
                }
                else
                {
                    return BadRequest("El registro esta siendo utilizado, no se puede eliminar");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CompanyInfo }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionCompanyName([FromBody]CompanyInfo CompanyInfo)
        {
            List<CompanyInfo> _companyInfo = new List<CompanyInfo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CompanyInfo/GetCompanyInfo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _companyInfo = JsonConvert.DeserializeObject<List<CompanyInfo>>(valorrespuesta);
                    if (CompanyInfo.CompanyInfoId > 0)
                    {
                        _companyInfo = _companyInfo.Where(q => q.Company_Name == CompanyInfo.Company_Name && q.CompanyInfoId != CompanyInfo.CompanyInfoId).ToList();
                    }
                    else
                    {
                        _companyInfo = _companyInfo.Where(q => q.Company_Name == CompanyInfo.Company_Name).ToList();
                    }
                    if (_companyInfo.Count > 0)
                    {
                        return await Task.Run(() => BadRequest("Ya existe una Empresa creada con el mismo Nombre."));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_companyInfo));
        }
    }
}