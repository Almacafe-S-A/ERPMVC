using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ERPMVC.DTO;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class EmployeesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        public EmployeesController(IHostingEnvironment hostingEnvironment, ILogger<EmployeesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Listado Vacaciones
        public ActionResult Vacaciones()
        {
            return PartialView();
        }

        //Listado de Incapacidades.
        public ActionResult Incapacidades()
        {
            return PartialView();
        }
       

        public async Task<ActionResult> pvwEditEmployees(Int64 IdEmpleado)
        {
            Employees _Employees = new Employees();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<Employees>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_Employees));
        }

        //Vista de Edición/Ingreso
        [HttpPost("[action]")]
        public async Task<ActionResult> Empleado([FromBody]Employees _empleado)
        {
            Employees _Employees = new Employees();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _empleado.IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<Employees>(valorrespuesta);   
                }

                if (_Employees == null)
                {
                    _Employees = new Employees();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return PartialView(_Employees);

        }

        //[HttpPost]
        //public async Task<ActionResult<Employees>> SaveEmployees(IEnumerable<IFormFile> files, EmployeesDTO _EmployeesP)
        //{

        //    Employees _Employees = _EmployeesP;
        //    try
        //    {
        //        // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _Employees.IdEmpleado);
        //        string valorrespuesta = "";
        //        foreach (var file in files)
        //        {
        //            FileInfo info = new FileInfo(file.FileName);
        //            if (info.Extension.Equals(".jpg") || info.Extension.Equals(".png") || info.Extension.Equals(".jpeg"))
        //            {
        //                //_Employees.FechaModificacion = DateTime.Now;
        //                //_Employees.Usuariomodificacion = HttpContext.Session.GetString("user");
        //                if (result.IsSuccessStatusCode)
        //                {
        //                    valorrespuesta = await (result.Content.ReadAsStringAsync());
        //                    _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
        //                }

        //                if (_Employees == null) { _Employees = new Models.Employees(); }

        //                if (_EmployeesP.IdEmpleado == 0)
        //                {
        //                    _Employees.FechaCreacion = DateTime.Now;
        //                    _EmployeesP.ApplicationUserId = Guid.Parse("FC405B7D-9FE3-43C9-97B5-D87A174CAB8A");
        //                    _EmployeesP.Foto = file.FileName;
        //                    _Employees.Usuariocreacion = HttpContext.Session.GetString("user");
        //                    var insertresult = await Insert(_EmployeesP);
        //                }
        //                else
        //                {
        //                    _EmployeesP.Usuariocreacion = _Employees.Usuariocreacion;
        //                    _EmployeesP.FechaCreacion = _Employees.FechaCreacion;
        //                    var updateresult = await Update(_Employees.IdEmpleado, _EmployeesP);
        //                }
        //                var filePath = _hostingEnvironment.WebRootPath + "/images/emp/"
        //                        + file.FileName.Replace(info.Extension, "")
        //                        + info.Extension;

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await file.CopyToAsync(stream);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    return Json(_Employees);
        //}

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Employees>> SaveEmployees(IEnumerable<IFormFile> Foto, [FromBody]EmployeesDTO _EmployeesDTO)
        {
            try
            {
                Employees _listEmployees = new Employees();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _EmployeesDTO.IdEmpleado);
                string valorrespuesta = "";
                foreach (var file in Foto)
                {
                    FileInfo info = new FileInfo(file.FileName);
                    if (info.Extension.Equals(".jpg") || info.Extension.Equals(".png") || info.Extension.Equals(".jpeg"))
                    {
                        _EmployeesDTO.FechaModificacion = DateTime.Now;
                        _EmployeesDTO.Usuariomodificacion = HttpContext.Session.GetString("user");
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _listEmployees = JsonConvert.DeserializeObject<Employees>(valorrespuesta);
                        }

                        if (_listEmployees == null) { _listEmployees = new Models.Employees(); }
                        if (_listEmployees.IdEmpleado == 0)
                        {
                            _EmployeesDTO.FechaCreacion = DateTime.Now;
                            _EmployeesDTO.Usuariocreacion = HttpContext.Session.GetString("user");
                            var insertresult = await Insert(_EmployeesDTO);
                            var value = (insertresult.Result as ObjectResult).Value;
                            _EmployeesDTO = ((EmployeesDTO)(value));
                        }
                        else
                        {
                            if (System.IO.File.Exists(_listEmployees.Foto))
                            {
                                System.IO.File.Delete(_listEmployees.Foto);
                            }
                            _EmployeesDTO.Usuariocreacion = _listEmployees.Usuariocreacion;
                            _EmployeesDTO.FechaCreacion = _listEmployees.FechaCreacion;
                            var updateresult = await Update(_EmployeesDTO.IdEmpleado, _EmployeesDTO);
                        }
                        if (_EmployeesDTO.IdEmpleado > 0) { 
                        var filePath = _hostingEnvironment.WebRootPath + "/images/emp/" + _EmployeesDTO.IdEmpleado + "_"
                                + file.FileName.Replace(info.Extension, "") + info.Extension;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        _EmployeesDTO.Foto = filePath;
                        var updateresult2 = await Update(_EmployeesDTO.IdEmpleado, _EmployeesDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_EmployeesDTO);
        }

        //[HttpPost]
        //public async Task<ActionResult<Employees>> SaveEmployees([FromBody]EmployeesDTO _EmployeesP)
        //{

        //    Employees _Employees = _EmployeesP;
        //    try
        //    {
        //        // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _Employees.IdEmpleado);
        //        string valorrespuesta = "";
        //        _Employees.FechaModificacion = DateTime.Now;
        //        _Employees.Usuariomodificacion = HttpContext.Session.GetString("user");
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
        //        }

        //        if (_Employees == null) { _Employees = new Models.Employees(); }

        //        if (_EmployeesP.IdEmpleado == 0)
        //        {
        //            _Employees.FechaCreacion = DateTime.Now;
        //            _Employees.Usuariocreacion = HttpContext.Session.GetString("user");
        //            var insertresult = await Insert(_EmployeesP);
        //        }
        //        else
        //        {
        //            _EmployeesP.Usuariocreacion = _Employees.Usuariocreacion;
        //            _EmployeesP.FechaCreacion = _Employees.FechaCreacion;
        //            var updateresult = await Update(_Employees.IdEmpleado, _EmployeesP);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    return Json(_Employees);
        //}



        //[HttpPost("[controller]/[action]")]
        //public async Task<ActionResult<Employees>> SaveEmployees([FromBody]EmployeesDTO _EmployeesP)
        ////public async Task<ActionResult<Employees>> SaveEmployees([FromBody]EmployeesDTO _EmployeesP)
        //{

        //    Employees _Employees = _EmployeesP;
        //    try
        //    {
        //        // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _Employees.IdEmpleado);
        //        string valorrespuesta = "";
        //        _Employees.FechaModificacion = DateTime.Now;
        //        _Employees.Usuariomodificacion = HttpContext.Session.GetString("user");
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
        //        }

        //        if (_Employees == null) { _Employees = new Models.Employees(); }

        //        if (_EmployeesP.IdEmpleado == 0)
        //        {
        //            _Employees.FechaCreacion = DateTime.Now;
        //            _Employees.Usuariocreacion = HttpContext.Session.GetString("user");
        //            var insertresult = await Insert(_EmployeesP);
        //        }
        //        else
        //        {
        //            _EmployeesP.Usuariocreacion = _Employees.Usuariocreacion;
        //            _EmployeesP.FechaCreacion = _Employees.FechaCreacion;
        //            var updateresult = await Update(_Employees.IdEmpleado, _EmployeesP);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    return Json(_Employees);
        //}


        //public async Task<ActionResult> SaveEmployees([FromBody]dynamic dto)
        ////public async Task<ActionResult> SaveEmployees([FromBody]ProductRelation _ProductRelationp)
        //{
        //    Employees _Employees = new Employees();
        //    List<Employees> _Employeeslist = new List<Employees>();
        //    try
        //    {



        //        ProductRelation _Employeesp = JsonConvert.DeserializeObject<ProductRelation>(dto);
        //        //ProductRelation _ProductRelationp = dto;
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.PostAsJsonAsync(baseadress + "api/ProductRelation/GetProductRelationByProductIDSubProductId", _Employeesp);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _Employeeslist = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);



        //        }


        //        if (_Employeeslist.Count == 0)
        //        {
        //            _Employeesp.FechaCreacion = DateTime.Now;
        //            _Employeesp.UsuarioCreacion = HttpContext.Session.GetString("user");
        //            var insertresult = await Insert(_Employeesp);
        //        }
        //        else
        //        {
        //            _Employeesp.UsuarioModificacion = HttpContext.Session.GetString("user");
        //            _Employeesp.FechaModificacion = DateTime.Now;
        //            var updateresult = await Update(_Employeesp.RelationProductId, _Employeesp);
        //        }







        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }



        //    return Json(_ProductRelation);
        //}



        //[HttpPost("[controller]/[action]")]
        //public async Task<ActionResult<Employees>> SaveEmployees([FromBody]EmployeesDTO _EmployeesP)
        ////public async Task<ActionResult> SaveEmployees([FromBody]dynamic dto)
        //{
        //    Employees _Employeesd = new Employees();
        //    try
        //    {


        //        //Employees _Employeesp = JsonConvert.DeserializeObject<Employees>(dto);
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _Employeesd.IdEmpleado);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _Employeesd = JsonConvert.DeserializeObject<Employees>(valorrespuesta);


        //        }


        //        if (_Employeesd == null)
        //        {
        //            _Employeesd = new Employees();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }


        //    return Json(_Employeesd);
        //}


        ////public async Task<ActionResult<Employees>> SaveEmployees([FromBody]EmployeesDTO _EmployeesP)
        ////public async Task<ActionResult> SaveEmployees([FromBody]dynamic dto)
        //{
        //    Employees _Employees = new Employees();
        //    Employees _Employeesp = JsonConvert.DeserializeObject<Employees>(dto);
        //    try
        //    {

        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _Employeesp.IdEmpleado);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _Employeesp = JsonConvert.DeserializeObject<Employees>(valorrespuesta);



        //        }



        //        if (_Employeesp == null)
        //        {
        //            _Employeesp = new Employees();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }



        //    return Json(_Employeesp);
        //}


        //--------------------------------------------------------------------------------------
        // POST: Employees/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EmployeesDTO>> Insert(EmployeesDTO _EmployeesDTO)
        {
            EmployeesDTO _Employees = new EmployeesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EmployeesDTO.Usuariocreacion = HttpContext.Session.GetString("user");
                _EmployeesDTO.Usuariomodificacion = HttpContext.Session.GetString("user");
                _EmployeesDTO.FechaCreacion = DateTime.Now;
                _EmployeesDTO.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Employees/Insert", _EmployeesDTO);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Employees);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _Employees }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeesDTO>> Update(Int64 Id, EmployeesDTO _EmployeesDTO)
        {
            EmployeesDTO _Employees = new EmployeesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EmployeesDTO.FechaModificacion = DateTime.Now;
                _EmployeesDTO.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Employees/Update", _EmployeesDTO);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            //return await Task.Run(() => Ok(_Employees));
            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeesDTO }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult<Employees>> Delete(Employees _Employeesp)
        {
            Employees _Employees = _Employeesp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Employees/Delete", _Employees);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<Employees>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


            return await Task.Run(() => Ok(_Employees));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _Employees }, Total = 1 });
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddEmployees([FromBody]EmployeesDTO _sarpara)

        {
            EmployeesDTO _Employees = new EmployeesDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _sarpara.IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);

                }

                if (_Employees == null)
                {
                    _Employees = new EmployeesDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => PartialView(_Employees));

        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Employees> _Employees = new List<Employees>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployees");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => _Employees.ToDataSourceResult(request));

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetEmployees([DataSourceRequest]DataSourceRequest request)
        {
            List<Employees> _Employees = new List<Employees>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployees");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<List<Employees>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(()=>  Json(_Employees.ToDataSourceResult(request)));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionIdentidad1([FromBody]Employees employee)
        {
            Employees _Employees = new Employees();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/ValidacionIdentidad/" + employee.Identidad + "/" + employee.IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<Employees>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Ok(_Employees));
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionRTN1([FromBody]Employees employee)
        {
            Employees _Employees = new Employees();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/ValidacionRTN/" + employee.RTN + "/" + employee.IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<Employees>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Ok(_Employees));
        }
    }
}
