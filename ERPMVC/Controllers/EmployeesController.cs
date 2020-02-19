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
            EmployeesDTO _Employees = new EmployeesDTO();
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
                    _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
                    _Employees.QtySalary = Convert.ToDecimal(_Employees.Salario);

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

         [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EmployeesDTO>> SaveEmployees(IEnumerable<IFormFile> files, EmployeesDTO _EmployeesP)
        {
            Employees _Employees = new Employees();
            try
            {
                _EmployeesP.Genero = _EmployeesP.GeneroName;
                _EmployeesP.TipoSangre = _EmployeesP.TipoSangreName;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _EmployeesP.IdEmpleado);
                string valorrespuesta = "";
                
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Employees = JsonConvert.DeserializeObject<EmployeesDTO>(valorrespuesta);
                }
                
                if (_Employees == null) { _Employees = new Models.Employees(); }

                if (_EmployeesP.IdEmpleado == 0)
                {
                    _EmployeesP.FechaCreacion = DateTime.Now;
                    _EmployeesP.Usuariocreacion = HttpContext.Session.GetString("user");
                    if (_EmployeesP.QtySalary > 0) {
                        _EmployeesP.Salario = _EmployeesP.QtySalary;
                        EmployeeSalaryDTO _Salary = new EmployeeSalaryDTO();
                        _Salary.QtySalary = _EmployeesP.QtySalary;
                        _Salary.DayApplication = _EmployeesP.DayApplication;
                        _Salary.CreatedUser = HttpContext.Session.GetString("user");
                        _Salary.CreatedDate = DateTime.Now;
                        _Salary.IdFrequency = 0;
                        _Salary.ModifiedDate = DateTime.Now;
                        _Salary.ModifiedUser = HttpContext.Session.GetString("user");
                        _Salary.IdEstado = 1;

                        _EmployeesP._EmployeeSalary.Add(_Salary);
                    }

                    var insertresult = await Insert(_EmployeesP);
                    var value = (insertresult.Result as ObjectResult).Value;
                    EmployeesDTO resultado = ((EmployeesDTO)(value));

                    IFormFile file = files.FirstOrDefault();
                    if (file != null)
                    {
                        FileInfo info = new FileInfo(file.FileName);
                        string filename = resultado.IdEmpleado + "_" + _EmployeesP.NombreEmpleado.Trim(' ') + info.Extension;
                        _EmployeesP.Foto = filename;
                        var filePath = _hostingEnvironment.WebRootPath + "/images/emp/" + filename;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        _EmployeesP.IdEmpleado = resultado.IdEmpleado;
                        var updateresult1 = await Update(_EmployeesP);

                    }
                }
                else
                {
                    _EmployeesP.Salario = _EmployeesP.QtySalary;
                    _EmployeesP.Foto = _Employees.Foto;
                    _EmployeesP.Usuariocreacion = _Employees.Usuariocreacion;
                    _EmployeesP.FechaCreacion = _Employees.FechaCreacion;
                    _EmployeesP.FechaModificacion = DateTime.Now;
                    _EmployeesP.Usuariomodificacion = HttpContext.Session.GetString("user");
                    
                    IFormFile file = files.FirstOrDefault();
                    if (file != null)
                    {
                        if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + "/images/emp/" + _EmployeesP.Foto))
                        {
                            System.IO.File.Delete(_hostingEnvironment.WebRootPath + "/images/emp/" + _EmployeesP.Foto);
                        }

                        FileInfo info = new FileInfo(file.FileName);
                        string filename = _EmployeesP.IdEmpleado + "_" + _EmployeesP.NombreEmpleado.Trim(' ') + info.Extension;
                        _EmployeesP.Foto = filename;
                        var filePath = _hostingEnvironment.WebRootPath + "/images/emp/" + filename;

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    var updateresult = await Update(_EmployeesP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;

            }
            return Json(_EmployeesP);
        }

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
        public async Task<ActionResult<EmployeesDTO>> Update( EmployeesDTO _EmployeesDTO)
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
        public async Task<ActionResult<Employees>> Delete([FromBody]Employees _Employeesp)
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
