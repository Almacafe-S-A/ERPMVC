using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class EmployeeSalaryController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EmployeeSalaryController(ILogger<EmployeeSalaryController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetEmployeeSalaryByIdEmployees([DataSourceRequest]DataSourceRequest request, EmployeeSalary _EmployeeSalaryP)
        {
            List<EmployeeSalary> _EmployeeSalary = new List<EmployeeSalary>();
            try
            {
                if (HttpContext.Session.Get("listadoEmployeeSalary") == null
                   || HttpContext.Session.GetString("listadoEmployeeSalary") == "")
                {
                    if (_EmployeeSalaryP.IdEmpleado > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_EmployeeSalaryP).ToString();
                        HttpContext.Session.SetString("listadoEmployeeSalary", serialzado);
                    }
                }
                else
                {
                    _EmployeeSalary = JsonConvert.DeserializeObject<List<EmployeeSalary>>(HttpContext.Session.GetString("listadoEmployeeSalary"));
                }


                if (_EmployeeSalaryP.IdEmpleado > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/EmployeeSalary/GetEmployeeSalaryByIdEmployees/" + _EmployeeSalaryP.IdEmpleado);
                    string valorrespuesta = "";

                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _EmployeeSalary = JsonConvert.DeserializeObject<List<EmployeeSalary>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoEmployeeSalary", JsonConvert.SerializeObject(_EmployeeSalary).ToString());
                    }
                }
                else
                {
                    if (_EmployeeSalaryP.QtySalary > 0)
                    {
                        _EmployeeSalary.Add(_EmployeeSalaryP);
                        HttpContext.Session.SetString("listadoEmployeeSalary", JsonConvert.SerializeObject(_EmployeeSalary).ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EmployeeSalary.ToDataSourceResult(request);

        }
        [HttpPost("PostEmployeeSalary")]
        public async Task<DataSourceResult> PostEmployeeSalary([DataSourceRequest]DataSourceRequest request, EmployeeSalary _EmployeeSalaryP)
        {
            List<EmployeeSalary> _EmployeeSalary = new List<EmployeeSalary>();
            Employees _Employees = new Employees();
            List<ElementoConfiguracion> elementoConfiguracion = new List<ElementoConfiguracion>();
            //HoursWorkedDetail _HoursWorkedDetailP = new HoursWorkedDetail();
            if (_EmployeeSalaryP.IdEmpleado == 0)
            {
                try
                {
                    //  _HoursWorkedDetailP = JsonConvert.DeserializeObject<HoursWorkedDetail>(dto);
                    if (HttpContext.Session.Get("listadoEmployeedSalary") == null
                       || HttpContext.Session.GetString("listadoEmployeedSalary") == "")
                    {
                        if (_EmployeeSalaryP.IdEmpleado > 0)
                        {
                            string serialzado = JsonConvert.SerializeObject(_EmployeeSalaryP).ToString();
                            HttpContext.Session.SetString("listadoEmployeedSalary", serialzado);
                        }
                    }
                    else
                    {
                        _EmployeeSalary = JsonConvert.DeserializeObject<List<EmployeeSalary>>(HttpContext.Session.GetString("listadoEmployeedSalary"));
                    }


                    if (_EmployeeSalaryP.IdEmpleado > 0)
                    {

                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.GetAsync(baseadress + "api/EmployeeSalary/GetEmployeeSalaryByIdEmployees/" + _EmployeeSalaryP.IdEmpleado);
                        string valorrespuesta = "";

                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _EmployeeSalary = JsonConvert.DeserializeObject<List<EmployeeSalary>>(valorrespuesta);
                            HttpContext.Session.SetString("listadoEmployeeSalary", JsonConvert.SerializeObject(_EmployeeSalary).ToString());
                        }
                    }
                    else
                    {
                        Convert.ToDecimal(_EmployeeSalaryP.QtySalary);
                        
                           
                        if (_EmployeeSalaryP.QtySalary > 0)
                        {
                            _EmployeeSalaryP.CreatedUser = HttpContext.Session.GetString("user");
                            _EmployeeSalaryP.CreatedDate = DateTime.Now;
                            _EmployeeSalaryP.ModifiedUser = HttpContext.Session.GetString("user");
                            _EmployeeSalaryP.ModifiedDate = DateTime.Now;
                            _EmployeeSalary.Add(_EmployeeSalaryP);
                            HttpContext.Session.SetString("listadoEmployeeSalary", JsonConvert.SerializeObject(_EmployeeSalary).ToString());
                            HttpContext.Session.SetString("listadoEmployeeSalary", "");
                        }
                    }



                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

                return _EmployeeSalary.ToDataSourceResult(request);
            }
            else
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    _EmployeeSalaryP.CreatedUser = HttpContext.Session.GetString("user");
                    _EmployeeSalaryP.CreatedDate = DateTime.Now;
                    _EmployeeSalaryP.ModifiedDate = DateTime.Now;
                    _EmployeeSalaryP.ModifiedUser = HttpContext.Session.GetString("user");

                    var result1 = await _client.GetAsync(baseadress + "api/Employees/GetEmployeesById/" + _EmployeeSalaryP.IdEmpleado);
                    string valorrespuesta1 = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        _Employees = JsonConvert.DeserializeObject<Employees>(valorrespuesta1);
                    }

                    var result2 = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionByIdConfiguracion/" + 81);
                    string valorrespuesta2 = "";
                    if (result2.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (result2.Content.ReadAsStringAsync());
                        elementoConfiguracion = JsonConvert.DeserializeObject<List<ElementoConfiguracion>>(valorrespuesta2);
                        elementoConfiguracion = elementoConfiguracion.Where(q => q.IdEstado == 1).ToList();
                    }

                    var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeSalary/Insert", _EmployeeSalaryP);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _EmployeeSalaryP = JsonConvert.DeserializeObject<EmployeeSalary>(valorrespuesta);
                        _EmployeeSalary.Add(_EmployeeSalaryP);

                      if(elementoConfiguracion.Count > 0) {  

                            foreach (var item in elementoConfiguracion) {  
                        MailMessage correo = new MailMessage();
                        // correo.From = new MailAddress("cumplimiento@almacafehn.com", "Actualización de Salario", System.Text.Encoding.UTF8);//Correo de salida
                        correo.From = new MailAddress(config.Value.emailsender);//Correo de salida
                        correo.To.Add(item.Nombre); //Correo destino?
                        correo.Subject = "Gestión de Salario de Empleado"; //Asunto
                        correo.Body = "Se modifico el Salario de " + _Employees.NombreEmpleado + " a : " + _EmployeeSalaryP.QtySalary + " Lps."; //Mensaje del correo
                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.Normal;
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        smtp.Host = config.Value.smtp; //Host del servidor de correo
                        smtp.Port = Convert.ToInt32(config.Value.port); //Puerto de salida
                        smtp.Credentials = new NetworkCredential(config.Value.emailsender, config.Value.passwordsmtp);
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                        smtp.EnableSsl = true;//True si el servidor de correo permite ssl
                        smtp.Send(correo);
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

                return _EmployeeSalary.ToDataSourceResult(request);
            }

        }
        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }

        [HttpPut("PutEmployeeSalary")]
        public async Task<IActionResult> PutEmployeeSalary(Int64 Id, EmployeeSalary _EmployeeSalary)
        {
            List<EmployeeSalary> _EmployeeSalaryP = new List<EmployeeSalary>();
            Employees _Employees = new Employees();

            try
            {
                if (_EmployeeSalary.IdEmpleado == 0)
                {

                    _EmployeeSalaryP.Add(_EmployeeSalary);
                    HttpContext.Session.SetString("listadoEmployeeSalary", JsonConvert.SerializeObject(_EmployeeSalaryP).ToString());
                    HttpContext.Session.SetString("listadoEmployeeSalary", "");
                }
                else
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    _EmployeeSalary.ModifiedDate = DateTime.Now;
                    _EmployeeSalary.ModifiedUser = HttpContext.Session.GetString("user");
                    var result = await _client.PutAsJsonAsync(baseadress + "api/EmployeeSalary/Update", _EmployeeSalary);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _EmployeeSalary = JsonConvert.DeserializeObject<EmployeeSalary>(valorrespuesta);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeSalary }, Total = 1 });
        }

        [HttpDelete("DeleteEmployeeSalary")]
        public async Task<ActionResult<EmployeeSalary>> DeleteEmployeeSalary(Int64 Id, EmployeeSalary _EmployeeSalary)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/HoursWorkedDetail/Delete", _EmployeeSalary);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeSalary = JsonConvert.DeserializeObject<EmployeeSalary>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeSalary }, Total = 1 });
        }
    }
}