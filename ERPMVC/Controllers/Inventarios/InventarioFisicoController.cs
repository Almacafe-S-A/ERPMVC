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
using Syncfusion.ReportWriter;
using Syncfusion.Report;
using Syncfusion.Pdf;
using Syncfusion.DocIORenderer;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class InventarioFisicoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public InventarioFisicoController(
            IHostingEnvironment hostingEnvironment,
            ILogger<InventarioFisicoController> logger,
            IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Solicitud Certificado Deposito")]
        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwInventarioFisico([FromBody]InventarioFisico _InventarioFisico)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisicoById/" + _InventarioFisico.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);

                }

                if (_InventarioFisico.Id == 0)
                {
                    _InventarioFisico = new InventarioFisico
                    {
                        Id = 0,
                        Fecha = DateTime.Now,
                    };
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InventarioFisico);

        }
        



        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCertificadosCustomerService([DataSourceRequest] DataSourceRequest request, [FromQuery(Name = "clienteid")] int clienteid, [FromQuery(Name = "servicioid")] int servicioid, [FromQuery(Name = "pendienteliquidacion")] int pendienteliquidacion)
        {
            List<InventarioFisico> certificados = new List<InventarioFisico>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/InventarioFisico/CertificadosPendientesCustomerService/{clienteid}/{servicioid}/{pendienteliquidacion}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    certificados = JsonConvert.DeserializeObject<List<InventarioFisico>>(valorrespuesta);
                    certificados = certificados.OrderByDescending(q => q.Id).ToList();
                    certificados = (from certificado in certificados
                                      select new InventarioFisico()
                                      {
                                      }).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return certificados.ToDataSourceResult(request);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetInventarioFisico([DataSourceRequest]DataSourceRequest request)
        {
            List<InventarioFisico> _InventarioFisico = new List<InventarioFisico>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisico");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<List<InventarioFisico>>(valorrespuesta);
                    _InventarioFisico = _InventarioFisico.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InventarioFisico.ToDataSourceResult(request);

        }
        



       [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetInventarioFisicoByCustomer([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<InventarioFisico> _InventarioFisico = new List<InventarioFisico>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisicoByCustomer/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<List<InventarioFisico>>(valorrespuesta);
                    _InventarioFisico = (from ce in _InventarioFisico
                                            select new InventarioFisico {
                                                Id = ce.Id,
                                                CustomerId = ce.CustomerId,
                                                
                                                
                                            }
                                            ).ToList();

                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InventarioFisico.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetInventarioFisicoById([DataSourceRequest]DataSourceRequest request, [FromBody] InventarioFisico _Certificado)
        {
            InventarioFisico _InventarioFisico = new InventarioFisico();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisicoById/" + _Certificado.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);

                }

                if (_InventarioFisico == null)
                {
                    _InventarioFisico = new InventarioFisico();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InventarioFisico);
        }


       

        //[HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InventarioFisico>> Aprobar([FromBody] InventarioFisico _InventarioFisico)
        {
            try
            {
                if (_InventarioFisico != null)
                {
                    InventarioFisico _listInventarioFisico = new InventarioFisico();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));                   
                    var result = await _client.GetAsync(baseadress + $"api/InventarioFisico/AprobarCertificado/{ _InventarioFisico.Id}");
                    string valorrespuesta = "";
                    _InventarioFisico.FechaModificion = DateTime.Now;
                    _InventarioFisico.UsuarioModificion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listInventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest("No se anulo el documento!"));
                    }

                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_InventarioFisico));
        }




        [HttpPost("[controller]/[action]")]
       // public async Task<ActionResult<InventarioFisico>>      InventarioFisico([FromBody]InventarioFisico _InventarioFisico)
         public async Task<ActionResult<InventarioFisico>> SaveInventarioFisico([FromBody]dynamic dto)
        {
             InventarioFisico _InventarioFisico = new InventarioFisico(); 
            try
            {
                _InventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(dto.ToString());
                //_InventarioFisico = ToCertificado(dto);
                if (_InventarioFisico != null)
                {
                    InventarioFisico _listInventarioFisico = new InventarioFisico();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisicoById/" + _InventarioFisico.Id);
                    string valorrespuesta = "";
                    _InventarioFisico.FechaModificion = DateTime.Now;
                    _InventarioFisico.UsuarioModificion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listInventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);
                    }

                    if (_listInventarioFisico == null) { _listInventarioFisico = new InventarioFisico(); }
                    if (_listInventarioFisico.Id == 0)
                    {
                        _InventarioFisico.FechaCreacion = DateTime.Now;
                        _InventarioFisico.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_InventarioFisico);
                        var value = (insertresult.Result as ObjectResult).Value;
                        _InventarioFisico = ((InventarioFisico)(value));
                        if (_InventarioFisico.Id <= 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero el documento!"));
                        }
                    }
                    else
                    {
                        var updateresult = await Update(_InventarioFisico.Id, _InventarioFisico);
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_InventarioFisico));
        }

        // POST: InventarioFisico/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InventarioFisico>> Insert(InventarioFisico _InventarioFisico)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InventarioFisico.UsuarioCreacion = HttpContext.Session.GetString("user");
                _InventarioFisico.UsuarioModificion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InventarioFisico/Insert", _InventarioFisico);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);
                }
                else
                {
                    _InventarioFisico.Id = 0;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return await Task.Run(() => Ok(_InventarioFisico));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InventarioFisico }, Total = 1 });
        }

        [HttpPost("[controller]/[action]/{id}")]
        public async Task<ActionResult<InventarioFisico>> Update(Int64 id, InventarioFisico _InventarioFisico)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InventarioFisico/Update", _InventarioFisico);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InventarioFisico }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InventarioFisico>> Delete([FromBody]InventarioFisico _InventarioFisico)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InventarioFisico/Delete", _InventarioFisico);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<InventarioFisico>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InventarioFisico }, Total = 1 });
        }


        public async Task<ActionResult> Virtualization_ReadByCustomer([DataSourceRequest] DataSourceRequest request, Customer Customer)
        {
            var res = await GetCertificadosByCustomer(Customer);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapperByCustomer(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetCertificados(0))
                {
                    if (values.Contains(order.Id))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<InventarioFisico>> GetCertificadosByCustomer(Customer Customer)
        {
            List<InventarioFisico> _InventarioFisico = new List<InventarioFisico>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisicoByCustomer/" + Customer.CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                     valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisico = JsonConvert.DeserializeObject<List<InventarioFisico>>(valorrespuesta);
                    _InventarioFisico = (from c in _InventarioFisico
                                            .Where(q => q.CustomerId == Customer.CustomerId)
                                            select new InventarioFisico
                                            {
                                                Id = c.Id,
                                                CustomerId = c.CustomerId,
                                            }).ToList();

                    

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _InventarioFisico;
        }




        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request,Int64 CustomerId)
        {
            var res = await GetCertificados(CustomerId);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetCertificados(0))
                {
                    if (values.Contains(order.Id))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<InventarioFisico>> GetCertificados(Int64 CustomerId)
        {
            List<InventarioFisico> _InventarioFisico = new List<InventarioFisico>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisico/GetInventarioFisicoLiberados/"+CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                     

                    _InventarioFisico = JsonConvert.DeserializeObject<List<InventarioFisico>>(valorrespuesta);
                    _InventarioFisico = (from c in _InventarioFisico
                                            select new InventarioFisico
                                            {
                                                Id = c.Id,
                                                CustomerId = c.CustomerId,
                                            }).ToList();

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetInventarioFisico");
                    valorrespuesta = "";
                    //if (result.IsSuccessStatusCode)
                    //{
                    //    List<Int64> _endosos = 

                    //}
                
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _InventarioFisico;
        }


        [HttpGet]
        public async Task<ActionResult> SFInventarioFisico(int id)
        {
            InventarioFisico _InventarioFisico = new InventarioFisico { Id = id, };
            try
            {
              
                string basePath = _hostingEnvironment.WebRootPath;
                FileStream inputStream = new FileStream(basePath + "/ReportsTemplate/CertificadoDeDeposito.rdl", FileMode.Open, FileAccess.Read);
                ReportWriter reportWriter = new ReportWriter(inputStream);
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter() { Name = "Id", Labels = new List<string>() { _InventarioFisico.Id.ToString() }, Values = new List<string>() { _InventarioFisico.Id.ToString() } });
                reportWriter.SetParameters(parameters);
                Syncfusion.Report.DataSourceCredentials[] dscarray = new Syncfusion.Report.DataSourceCredentials[1];
                Syncfusion.Report.DataSourceCredentials dsc = new Syncfusion.Report.DataSourceCredentials();
                dsc.ConnectionString = Utils.ConexionReportes;
                dsc.Name = "ERP";
                dscarray[0] = dsc;
                reportWriter.SetDataSourceCredentials(dscarray);
                var format = Syncfusion.ReportWriter.WriterFormat.PDF;
                string completepath = basePath + $"/CertificadosDeposito/CertificadoDeDeposito{id}.pdf";
                MemoryStream ms = new MemoryStream();

                reportWriter.Save(ms, format);
                ms.Position = 0;

                using (FileStream file = new FileStream(completepath, FileMode.Create, System.IO.FileAccess.Write))
                    ms.WriteTo(file);

                ViewBag.pathcontrato = completepath;
                var stream = new FileStream(completepath, FileMode.Open);
                return new FileStreamResult(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
           
            return await Task.Run(()=> View(_InventarioFisico));
        }


  




    }
}