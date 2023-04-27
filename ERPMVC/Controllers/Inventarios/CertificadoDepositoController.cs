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
    public class CertificadoDepositoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        public CertificadoDepositoController(
            IHostingEnvironment hostingEnvironment,
            ILogger<CertificadoDepositoController> logger,
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
        public async Task<ActionResult> pvwCertificadoDeposito([FromBody] CertificadoDepositoDTO _CertificadoDepositoDTO)
        {
            CertificadoDepositoDTO _CertificadoDeposito = new CertificadoDepositoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _CertificadoDepositoDTO.IdCD);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(valorrespuesta);

                }

                if (_CertificadoDeposito.IdCD == 0)
                {
                    _CertificadoDeposito = new CertificadoDepositoDTO
                    {
                        IdCD = 0,
                        FechaCertificado = DateTime.Now,
                        //FechaVencimiento = DateTime.Now.AddDays(60),
                        FechaVencimientoDeposito = DateTime.Now.AddDays(30),
                        //FechaFirma = DateTime.Now,
                        //FechaInicioComputo = DateTime.Now,
                        //FechaPagoBanco = DateTime.Now,                        
                        BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")),
                        PolizaPropia = true,
                        Seguro = "S/Tarifa",
                        OtrosCargos = "Si",

                        SujetasAPago = 0,
                    };
                }
                else
                {
                    _CertificadoDeposito.editar = 0;
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }



            return PartialView(_CertificadoDeposito);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCertificadosPendientesAutorizar([DataSourceRequest] DataSourceRequest request, [FromQuery(Name = "clienteid")] int clienteid, [FromQuery(Name = "servicioid")] int servicioid, [FromQuery(Name = "pendienteliquidacion")] int pendienteliquidacion)
        {
            List<CertificadoDeposito> certificados = new List<CertificadoDeposito>();
            try

            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/CertificadoDeposito/GetCertificadosPendientesAutorizar/{clienteid}/{servicioid}/{pendienteliquidacion}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    certificados = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    certificados = certificados.OrderByDescending(q => q.IdCD).ToList();
                    certificados = (from certificado in certificados
                                    select new CertificadoDeposito()
                                    {
                                        IdCD = certificado.IdCD,
                                        Comentario = certificado.IdCD + " - " + certificado.Comentario,
                                    }).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return certificados.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCertificadosNoEndosados([DataSourceRequest] DataSourceRequest request, int CustomerId)
        {
            List<CertificadoDeposito> certificados = new List<CertificadoDeposito>();
            try

            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/CertificadoDeposito/GetCertificadosNoEndosados/{CustomerId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    certificados = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    certificados = certificados.OrderByDescending(q => q.IdCD).ToList();
                    certificados = (from ce in certificados
                                    select new CertificadoDeposito
                                    {
                                        IdCD = ce.IdCD,
                                        Comentario = $"No: {ce.IdCD} || Servicio :{ce.ServicioName} || Fecha: {ce.FechaCertificado}",
                                        CustomerName = ce.CustomerName,
                                        CustomerId = ce.CustomerId,


                                    }
                                            ).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return certificados.ToDataSourceResult(request);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCertificadosCustomerService([DataSourceRequest] DataSourceRequest request, [FromQuery(Name = "clienteid")] int clienteid, [FromQuery(Name = "servicioid")] int servicioid, [FromQuery(Name = "pendienteliquidacion")] int pendienteliquidacion)
        {
            List<CertificadoDeposito> certificados = new List<CertificadoDeposito>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/CertificadoDeposito/CertificadosPendientesCustomerService/{clienteid}/{servicioid}/{pendienteliquidacion}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    certificados = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    certificados = certificados.OrderByDescending(q => q.IdCD).ToList();
                    certificados = (from certificado in certificados
                                    select new CertificadoDeposito()
                                    {
                                        IdCD = certificado.IdCD,
                                        Comentario = certificado.IdCD + " - " + certificado.Comentario,
                                    }).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return certificados.ToDataSourceResult(request);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCertificadoDeposito([DataSourceRequest] DataSourceRequest request)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDeposito");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    _CertificadoDeposito = _CertificadoDeposito.OrderByDescending(q => q.IdCD).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _CertificadoDeposito.ToDataSourceResult(request);

        }




        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetCertificadoDepositoByCustomer([DataSourceRequest] DataSourceRequest request, Int64 CustomerId)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoByCustomer/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    _CertificadoDeposito = (from ce in _CertificadoDeposito
                                            select new CertificadoDeposito {
                                                IdCD = ce.IdCD,
                                                Comentario = $"No: {ce.IdCD} || Servicio :{ce.ServicioName} || Fecha: {ce.FechaCertificado}",
                                                CustomerName = ce.CustomerName,
                                                CustomerId = ce.CustomerId,


                                            }
                                            ).ToList();

                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _CertificadoDeposito.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetCertificadoDepositoById([DataSourceRequest] DataSourceRequest request, [FromBody] CertificadoDeposito _Certificado)
        {
            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _Certificado.IdCD);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);

                }

                if (_CertificadoDeposito == null)
                {
                    _CertificadoDeposito = new CertificadoDeposito();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return Json(_CertificadoDeposito);
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetCertificadoDepositoByIdKardex([DataSourceRequest] DataSourceRequest request, [FromBody] CertificadoDepositoDTO _listado)
        {

            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito();
            try
            {
                if (_listado != null)
                {
                    foreach (var _Certificado in _listado.CertificadosList)
                    {

                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _Certificado);
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);

                        }

                        if (_CertificadoDeposito == null)
                        {
                            _CertificadoDeposito = new CertificadoDeposito();
                        }

                        KardexDTO _kardexparam = new KardexDTO
                        {
                            Ids = _listado.CertificadosList,
                            DocumentName = "CD"
                            ,
                            SalesOrderId = _listado.SalesOrderId,
                            EndDate = _listado.EndDate,
                            StartDate = _listado.EndDate
                        };
                        //  List<KardexLine> _kardexsaldo = new List<KardexLine>();
                        _kardexparam.UsuarioCreacion = HttpContext.Session.GetString("user");
                        _kardexparam.UsuarioModificacion = HttpContext.Session.GetString("user");
                        result = await _client.PostAsJsonAsync(baseadress + "api/Kardex/GetMovimientosCertificados", _kardexparam);
                        valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());

                        }


                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("No se anulo el documento!"));
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return Json(_CertificadoDeposito);
        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<List<CertificadoDeposito>>> AgruparCertificados([FromBody] GoodsDeliveryAuthorizationParams _params)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();
            if (_params != null)
                if (_params.CertificadosSeleccionados != null)
                {

                    try
                    {
                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/AgruparCertificados", _params.CertificadosSeleccionados);
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                        throw ex;
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el documento!"));
                }

            return await Task.Run(() => Json(_CertificadoDeposito));
        }


        //[HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CertificadoDeposito>> Aprobar([FromBody] CertificadoDepositoDTO _CertificadoDeposito)
        {
            try
            {
                if (_CertificadoDeposito != null)
                {
                    CertificadoDeposito _listCertificadoDeposito = new CertificadoDeposito();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + $"api/CertificadoDeposito/AprobarCertificado/{_CertificadoDeposito.IdCD}");
                    string valorrespuesta = "";
                    _CertificadoDeposito.FechaModificacion = DateTime.Now;
                    _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listCertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
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
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return await Task.Run(() => Json(_CertificadoDeposito));
        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CertificadoDeposito>> AnularCD([FromBody] CertificadoDepositoDTO _CertificadoDeposito)
        //  public async Task<ActionResult<CertificadoDeposito>> SaveCertificadoDeposito([FromBody]dynamic dto)
        {
            // CertificadoDepositoDTO _CertificadoDeposito = new CertificadoDepositoDTO(); 
            try
            {
                // _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(dto.ToString());
                if (_CertificadoDeposito != null)
                {
                    CertificadoDeposito _listCertificadoDeposito = new CertificadoDeposito();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    _CertificadoDeposito.IdEstado = 3;
                    _CertificadoDeposito.Estado = "Anulado";
                    var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/AnularCD", _CertificadoDeposito);
                    string valorrespuesta = "";
                    _CertificadoDeposito.FechaModificacion = DateTime.Now;
                    _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listCertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                    }
                    else
                    {
                        return await Task.Run(() => BadRequest(result.Content.ReadAsStringAsync()));
                    }

                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return await Task.Run(() => Json(_CertificadoDeposito));
        }

        CertificadoDeposito ToCertificado(dynamic dto) {
            CertificadoDeposito certificadoDeposito = new CertificadoDeposito();
            try
            {
                certificadoDeposito.IdCD = dto.IdCD;
                certificadoDeposito.NoCD = dto.NoCD;
                certificadoDeposito.Seguro = dto.Seguro;
                certificadoDeposito.Aduana = dto.Aduana;
                certificadoDeposito.Almacenaje = dto.Almacenaje;
                certificadoDeposito.BranchName = dto.BranchName;
                certificadoDeposito.BranchId = dto.BranchId;
                certificadoDeposito.Comentario = dto.Comentario;
                certificadoDeposito.CustomerId = dto.CustomerId;
                certificadoDeposito.CustomerName = dto.CustomerName;
                certificadoDeposito.Direccion = dto.Direccion;
                certificadoDeposito.EmpresaSeguro = dto.EmpresaSeguro;
                certificadoDeposito.Endoso = dto.Endoso;
                certificadoDeposito.Estado = dto.Estado;
                certificadoDeposito.FechaCertificado = dto.FechaCertificado;
                certificadoDeposito.FechaVencimientoCertificado = dto.FechaVencimientoCertificado;
                certificadoDeposito.FechaVencimientoDeposito = dto.FechaVencimientoDeposito;
                certificadoDeposito.IdEstado = dto.IdEstado;
                certificadoDeposito.InsurancePolicyId = dto.InsurancePolicyId;
                certificadoDeposito.PolizaPropia = dto.PolizaPropia;
                certificadoDeposito.ServicioId = dto.ServicioId;
                certificadoDeposito.ServicioName = dto.ServicioName;
                //certificadoDeposito.SujetasAPago = dto.SujetasAPago;
                certificadoDeposito.Total = dto.Total;
                certificadoDeposito.TotalDerechos = dto.TotalDerechos;
                certificadoDeposito._CertificadoLine = dto._CertificadoLine;


            }
            catch (Exception ex)
            {

                throw;
            }


            return certificadoDeposito;


        }

        [HttpPost("[controller]/[action]")]
        // public async Task<ActionResult<CertificadoDeposito>>      CertificadoDeposito([FromBody]CertificadoDepositoDTO _CertificadoDeposito)
        public async Task<ActionResult<CertificadoDeposito>> SaveCertificadoDeposito([FromBody] dynamic dto)
        {
            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito();
            try
            {
                _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(dto.ToString());
                //_CertificadoDeposito = ToCertificado(dto);
                if (_CertificadoDeposito != null)
                {
                    foreach (var item in _CertificadoDeposito._CertificadoLine)
                    {
                        if (item.Price == 0)
                        {
                            return await Task.Run(() => BadRequest("Ingrese un precio valido!"));
                        }
                        else if (item.Quantity == 0)
                        {
                            return await Task.Run(() => BadRequest("Ingrese un precio valido!"));
                        }
                        /*  else if (item.Price * item.Quantity != item.Amount)
                        {
                            return await Task.Run(() => BadRequest("El calculo de precio por cantidad no coincide!"));
                        }*/
                    }


                    CertificadoDeposito _listCertificadoDeposito = new CertificadoDeposito();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _CertificadoDeposito.IdCD);
                    string valorrespuesta = "";
                    _CertificadoDeposito.FechaModificacion = DateTime.Now;
                    _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listCertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                    }


                    if (_listCertificadoDeposito == null) { _listCertificadoDeposito = new CertificadoDeposito(); }
                    if (_listCertificadoDeposito.IdCD == 0)
                    {
                        _CertificadoDeposito.FechaCreacion = DateTime.Now;
                        _CertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_CertificadoDeposito);
                        if (insertresult.Result is BadRequestObjectResult)
                        {
                            return await Task.Run(() => BadRequest(((BadRequestObjectResult)insertresult.Result).Value));
                        }
                        var value = (insertresult.Result as ObjectResult).Value;
                        _CertificadoDeposito = ((CertificadoDepositoDTO)(value));
                        if (_CertificadoDeposito.IdCD <= 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero el documento!"));
                        }
                    }
                    else
                    {
                        var updateresult = await Update(_CertificadoDeposito.IdCD, _CertificadoDeposito);
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return await Task.Run(() => Json(_CertificadoDeposito));
        }

        // POST: CertificadoDeposito/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CertificadoDeposito>> Insert(CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Insert", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return await Task.Run(() => Ok(_CertificadoDeposito));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }

        [HttpPost("[controller]/[action]/{id}")]
        public async Task<ActionResult<CertificadoDeposito>> Update(Int64 id, CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Update", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CertificadoDeposito>> Delete([FromBody] CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Delete", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
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
                    if (values.Contains(order.IdCD))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<CertificadoDeposito>> GetCertificadosByCustomer(Customer Customer)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoByCustomer/" + Customer.CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    _CertificadoDeposito = (from c in _CertificadoDeposito
                                            .Where(q => q.CustomerId == Customer.CustomerId)
                                            select new CertificadoDeposito
                                            {
                                                IdCD = c.IdCD,
                                                CustomerName = "Id:" + c.IdCD + " || Número de certificado:" + c.NoCD + "  || Nombre:" + c.CustomerName + "|| Fecha:" + c.FechaCertificado + "|| Total:" + c.Total,
                                                CustomerId = c.CustomerId,
                                            }).ToList();



                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return _CertificadoDeposito;
        }




        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request, Int64 CustomerId)
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
                    if (values.Contains(order.IdCD))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<CertificadoDeposito>> GetCertificados(Int64 CustomerId)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoLiberados/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());


                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    _CertificadoDeposito = (from c in _CertificadoDeposito
                                            select new CertificadoDeposito
                                            {
                                                IdCD = c.IdCD,
                                                CustomerName = "Id:" + c.IdCD + " || Número de certificado:" + c.NoCD + "  || Nombre:" + c.CustomerName + "|| Fecha:" + c.FechaCertificado + "|| Total:" + c.Total,
                                                CustomerId = c.CustomerId,
                                            }).ToList();

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetCertificadoDeposito");
                    valorrespuesta = "";
                    //if (result.IsSuccessStatusCode)
                    //{
                    //    List<Int64> _endosos = 

                    //}

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return _CertificadoDeposito;
        }


        [HttpGet]
        public async Task<ActionResult> SFCertificadoDeposito(Int64 id)
        {
            CertificadoDepositoDTO _CertificadoDepositoDTO = new CertificadoDepositoDTO { IdCD = id, };
            try
            {

                string basePath = _hostingEnvironment.WebRootPath;
                FileStream inputStream = new FileStream(basePath + "/ReportsTemplate/CertificadoDeDeposito.rdl", FileMode.Open, FileAccess.Read);
                ReportWriter reportWriter = new ReportWriter(inputStream);
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter() { Name = "IdCD", Labels = new List<string>() { _CertificadoDepositoDTO.IdCD.ToString() }, Values = new List<string>() { _CertificadoDepositoDTO.IdCD.ToString() } });
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
                await MarcarImpresion(_CertificadoDepositoDTO.IdCD, false);
                //return View(_CertificadoDepositoDTO);

                return new FileStreamResult(ms, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return await Task.Run(() => View(_CertificadoDepositoDTO));
        }

        [HttpGet]
        public async Task<ActionResult> SFImprimirTalon(Int64 id)
        {
            CertificadoDepositoDTO _CertificadoDepositoDTO = new CertificadoDepositoDTO { IdCD = id, };
            try
            {

                string basePath = _hostingEnvironment.WebRootPath;
                FileStream inputStream = new FileStream(basePath + "/ReportsTemplate/CertificadoDeDepositoTalon.rdl", FileMode.Open, FileAccess.Read);
                ReportWriter reportWriter = new ReportWriter(inputStream);
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter() { Name = "IdCD", Labels = new List<string>() { _CertificadoDepositoDTO.IdCD.ToString() }, Values = new List<string>() { _CertificadoDepositoDTO.IdCD.ToString() } });
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

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                MarcarImpresion(_CertificadoDepositoDTO.IdCD, true);

                return new FileStreamResult(ms, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return await Task.Run(() => View(_CertificadoDepositoDTO));
        }



        public async Task<ActionResult> MarcarImpresion(Int64 id, bool talon)
        {
            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito { IdCD = id, };
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(valorrespuesta);

                }

                if (talon)
                {
                    _CertificadoDeposito.impresionesTalon = _CertificadoDeposito.impresionesTalon + 1;

                }
                else
                {
                    _CertificadoDeposito.Impresiones = _CertificadoDeposito.Impresiones + 1;
                }

                var resultadoUpdate = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Update", _CertificadoDeposito);

                if (!result.IsSuccessStatusCode)
                {

                }

                //await Update(id,_CertificadoDeposito);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return await Task.Run(() => View(_CertificadoDeposito));
        }

        [HttpGet]
        public ActionResult SFCertificadosEmitidosDetallado()
        {

            return View();


        }


        [HttpGet]
        public ActionResult SFKardexCertificado(int id)
        {
            CertificadoDeposito certificadoDeposito =
                new CertificadoDeposito {IdCD = id };
            return View(certificadoDeposito);


        }





    }
}