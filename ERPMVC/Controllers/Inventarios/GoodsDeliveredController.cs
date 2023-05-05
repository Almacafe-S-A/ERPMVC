using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Syncfusion.ReportWriter;
using Syncfusion.Report;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [EnableCors("AllowAllOrigins")]
    public class GoodsDeliveredController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        private readonly IHostingEnvironment _hostingEnvironment;
        public GoodsDeliveredController(ILogger<GoodsDeliveredController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Policy = "Inventarios.Entrega de Mercaderia")]
        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwGoodsDelivered([FromBody]GoodsDeliveredDTO _GoodsDeliveredDTO)
        {
            GoodsDeliveredDTO _GoodsDelivered = new GoodsDeliveredDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredById/" + _GoodsDeliveredDTO.GoodsDeliveredId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDeliveredDTO>(valorrespuesta);

                }

                if (_GoodsDelivered == null)
                {
                    _GoodsDelivered = new GoodsDeliveredDTO { DocumentDate=DateTime.Now, ExpirationDate = DateTime.Now, OrderDate=DateTime.Now, editar=1
                        ,
                        BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                else
                {
                    _GoodsDelivered.editar = 0;
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsDelivered);

        }


        [HttpGet/*("[controller]/[action]")*/]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsDelivered> _GoodsDelivered = new List<GoodsDelivered>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDelivered");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<List<GoodsDelivered>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsDelivered.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetGoodsDeliveredById([FromBody]GoodsDelivered _GoodsDeliveredp)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            GoodsDelivered _GoodsDelivered = new GoodsDelivered();
            if(_GoodsDeliveredp != null) { 
            try
            {

                //GoodsDelivered _GoodsDeliveredp = JsonConvert.DeserializeObject<GoodsDelivered>(dto);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredById/" + _GoodsDeliveredp.GoodsDeliveredId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);

                }

                if (_GoodsDelivered == null)
                {
                    _GoodsDelivered = new GoodsDelivered();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            }

            return Json(_GoodsDelivered);
        }

        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request)
        {
            var res = await GetGoodsDelivered();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetGoodsDelivered())
                {
                    if (values.Contains(order.GoodsDeliveredId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        [HttpGet("[controller]/[action]")]
        private async Task<List<GoodsDelivered>> GetGoodsDelivered()
        {
            List<GoodsDelivered> _GoodsDelivered = new List<GoodsDelivered>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredNoSelected");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<List<GoodsDelivered>>(valorrespuesta);
                    _GoodsDelivered = (from c in _GoodsDelivered
                                       select new GoodsDelivered
                                       {
                                                  GoodsDeliveredId = c.GoodsDeliveredId,
                                                       CustomerName = "Numero de recibo de entrega: " + c.GoodsDeliveredId + "  ||Nombre:" + c.CustomerName + " ||Fecha: "
                                             +           c.DocumentDate + " ||Fecha de documento:" + c.DocumentDate + " || Boleta de peso:" + c.WeightBallot + " || Cliente:" + c.CustomerName,
                                                       DocumentDate = c.DocumentDate,

                                        }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _GoodsDelivered;
        }



             [HttpPost("[controller]/[action]")]
             public async Task<ActionResult<GoodsDelivered>> SaveGoodsDelivered([FromBody]GoodsDelivered _GoodsDelivered)
            // public async Task<ActionResult<GoodsDelivered>> SaveGoodsDelivered([FromBody]dynamic dto)
        {

            //GoodsDelivered _GoodsDelivered = new GoodsDelivered();
            try
            {
              //  _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(dto.ToString());
                GoodsDelivered _listGoodsDelivered = new GoodsDelivered();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDeliveredById/" + _GoodsDelivered.GoodsDeliveredId);
                string valorrespuesta = "";
                _GoodsDelivered.FechaModificacion = DateTime.Now;
                _GoodsDelivered.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

                if (_listGoodsDelivered == null) { _listGoodsDelivered = new GoodsDelivered(); }


                if (_listGoodsDelivered.GoodsDeliveredId == 0)
                {
                    _GoodsDelivered.FechaCreacion = DateTime.Now;
                    _GoodsDelivered.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GoodsDelivered);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _GoodsDelivered = ((GoodsDelivered)(value));
                    if (_GoodsDelivered.GoodsDeliveredId == 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero el documento!"));
                    }

                }
                else
                {
                    var updateresult = await Update(_GoodsDelivered.GoodsDeliveredId, _GoodsDelivered);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_GoodsDelivered);
        }

        // POST: GoodsDelivered/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsDelivered>> Insert(GoodsDelivered _GoodsDelivered)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsDelivered.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsDelivered.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDelivered/Insert", _GoodsDelivered);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_GoodsDelivered);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDelivered }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsDelivered>> Update(Int64 id, GoodsDelivered _GoodsDelivered)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsDelivered/Update", _GoodsDelivered);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

            }
            catch (Exception ex)
            { 
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDelivered }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDelivered>> Delete([FromBody]GoodsDelivered _GoodsDelivered)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDelivered/Delete", _GoodsDelivered);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDelivered = JsonConvert.DeserializeObject<GoodsDelivered>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDelivered }, Total = 1 });
        }


        [HttpGet]
        public ActionResult SFGoodsDelivered(Int64 id)
        {

            GoodsDeliveredDTO _GoodsDelivered = new GoodsDeliveredDTO { GoodsDeliveredId = id, };

            return View(_GoodsDelivered);
        }

        [HttpGet]
        public ActionResult SFComprobanteEntregasEmitido()
        {

            return View();


        }

        public ActionResult SFEntregaMercaderia(Int64 id)
        {

            GoodsDelivered goodDelivered = new GoodsDelivered { GoodsDeliveredId = id };
            try
            {
                string basePath = _hostingEnvironment.WebRootPath;
                FileStream inputStream = new FileStream(basePath + "/ReportsTemplate/EntregaDeMercaderia.rdl", FileMode.Open, FileAccess.Read);
                ReportWriter reportWriter = new ReportWriter(inputStream);
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter() { Name = "GoodDeliveredID", Labels = new List<string>() { goodDelivered.GoodsDeliveredId.ToString() }, Values = new List<string>() { goodDelivered.GoodsDeliveredId.ToString() } });
                reportWriter.SetParameters(parameters);
                Syncfusion.Report.DataSourceCredentials[] dscarray = new Syncfusion.Report.DataSourceCredentials[1];
                Syncfusion.Report.DataSourceCredentials dsc = new Syncfusion.Report.DataSourceCredentials();
                dsc.ConnectionString = Utils.ConexionReportes;
                dsc.Name = "ERP";
                dscarray[0] = dsc;
                reportWriter.SetDataSourceCredentials(dscarray);
                var format = Syncfusion.ReportWriter.WriterFormat.PDF;
                string completepath = basePath + $"/AutorizacionesEntregas/Autorizacion_{id}.pdf";
                MemoryStream ms = new MemoryStream();

                reportWriter.Save(ms, format);
                ms.Position = 0;
                return new FileStreamResult(ms, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
        }


        public async Task<ActionResult> Virtualization_ReadSalida([DataSourceRequest] DataSourceRequest request, Customer _customerp)
        {
            var res = await GetBoletaEntradaSalida(_customerp);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapperSalida(Int64[] values, Customer _customerp)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetBoletaEntradaSalida(_customerp))
                {
                    if (values.Contains(order.clave_e))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<Boleto_Ent>> GetBoletaEntradaSalida(Customer _customerp)
        {
            List<Boleto_Ent> _Boleto_Ent = new List<Boleto_Ent>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Sal/GetBoletoSalida");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<List<Boleto_Ent>>(valorrespuesta);

                    Customer _customer = new Customer();

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerById/" + _customerp.CustomerId);
                    valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _customer = JsonConvert.DeserializeObject<Customer>(valorrespuesta);
                    }

                    if (_customer != null)
                    {
                        if (_customerp.CustomerTypeId == 1)
                        {          
                            _Boleto_Ent = (from c in _Boleto_Ent
                                           .Where(q => q.clave_C == _customer.CustomerRefNumber)                               
                                            .Where(q => q.completo == false)
                                            .OrderByDescending(q => q.clave_e)
                                           select new Boleto_Ent
                                           {
                                               clave_e = c.clave_e,
                                               observa_e = "Placas:" + c.placas + " || Boleta de peso No.:" + c.clave_e + "  || Conductor:" + c.conductor + "|| Fecha:" + c.fecha_e + "|| Hora:" + c.hora_e,
                                               Boleto_Sal = c.Boleto_Sal,
                                               peso_e = c.peso_e
                                           }).ToList();

                            _client = new HttpClient();
                            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                            List<Int64> _boletapeso = new List<long>();
                            result = await _client.GetAsync(baseadress + "api/GoodsDelivered/GetGoodsDelivered/");
                            if (result.IsSuccessStatusCode)
                            {
                                valorrespuesta = await (result.Content.ReadAsStringAsync());
                                List<GoodsDelivered> _controlpallets = JsonConvert.DeserializeObject<List<GoodsDelivered>>(valorrespuesta);
                                _boletapeso = _controlpallets.Select(q => q.WeightBallot).ToList();
                                _Boleto_Ent = _Boleto_Ent.Where(q => !_boletapeso.Contains(q.clave_e)).ToList();
                            }        
                        }
                        else
                        {
                            _Boleto_Ent = (from c in _Boleto_Ent

                                            .Where(q => q.completo == false)
                                            .Where(q => q.clave_C == _customer.CustomerRefNumber)

                                             .OrderByDescending(q => q.clave_e)
                                           select new Boleto_Ent
                                           {
                                               clave_e = c.clave_e,
                                               observa_e = "Placas:" + c.placas + " || Boleta de peso No.:" + c.clave_e + "  || Conductor:" + c.conductor + "|| Fecha:" + c.fecha_e + "|| Hora:" + c.hora_e,
                                               Boleto_Sal = c.Boleto_Sal,
                                               peso_e = c.peso_e
                                           }).ToList();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _Boleto_Ent;
        }



    }
}