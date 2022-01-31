﻿using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.ReportWriter;
using Syncfusion.Report;
using Syncfusion.Pdf;
using Syncfusion.DocIORenderer;
using System.IO;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    //[Route("GoodsDeliveryAuthorization")]
    public class GoodsDeliveryAuthorizationController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        private readonly IHostingEnvironment _hostingEnvironment;
        public GoodsDeliveryAuthorizationController(ILogger<GoodsDeliveryAuthorizationController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {
            this.config = config;
            this._logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Entrega de Mercaderia")]
        [HttpGet("[controller]/[action]")]
        [HttpGet("[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwGoodsDeliveryAuthorizationDetailMant([FromBody]GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLineP)
        {
            GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLine = new GoodsDeliveryAuthorizationLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/GetGoodsDeliveryAuthorizationLineById/" + _GoodsDeliveryAuthorizationLineP.GoodsDeliveryAuthorizationLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationLine>(valorrespuesta);

                }

                if (_GoodsDeliveryAuthorizationLine == null)
                {
                    _GoodsDeliveryAuthorizationLine = new GoodsDeliveryAuthorizationLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsDeliveryAuthorizationLine);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> AutorizacionesPendientes([DataSourceRequest] DataSourceRequest request,
             int CustomerId,
             int ServicioId,
             int BranchId)
        {
            List<GoodsDeliveryAuthorization> goodsDeliveryAuthorizations = new List<GoodsDeliveryAuthorization>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/GoodsDeliveryAuthorization/AutorizacionesPendientes/{CustomerId}/{ServicioId}/{BranchId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    goodsDeliveryAuthorizations = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorization>>(valorrespuesta);
                    goodsDeliveryAuthorizations = goodsDeliveryAuthorizations.OrderByDescending(q => q.GoodsDeliveryAuthorizationId).ToList();
                    goodsDeliveryAuthorizations = (from ar in goodsDeliveryAuthorizations
                                                   select new GoodsDeliveryAuthorization()
                                      {
                                          GoodsDeliveryAuthorizationId = ar.GoodsDeliveryAuthorizationId,
                                          Comments = $"{ar.GoodsDeliveryAuthorizationId} || Fecha:{ ar.AuthorizationDate}"
                                      }).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return goodsDeliveryAuthorizations.ToDataSourceResult(request);
        }




        [HttpPost("[controller]/[action]")]
        [HttpPost("[action]")]
        public async Task<ActionResult> pvwGoodsDeliveryAuthorization([FromBody]GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorizationDTO)
        {
            GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorizationDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationById/" + _GoodsDeliveryAuthorizationDTO.GoodsDeliveryAuthorizationId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationDTO>(valorrespuesta);

                }

                if (_GoodsDeliveryAuthorization == null)
                {
                    _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorizationDTO
                    {
                        AuthorizationDate = DateTime.Now,
                        DocumentDate = DateTime.Now,
                        editar = 1
                        ,
                        BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                else
                {
                    _GoodsDeliveryAuthorization.editar = 0;

                }

                ViewData["permisos"] = _principal;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsDeliveryAuthorization);

        }



        [HttpGet("[controller]/[action]")]
        [HttpGet("[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsDeliveryAuthorization> _GoodsDeliveryAuthorization = new List<GoodsDeliveryAuthorization>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorization");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorization>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsDeliveryAuthorization.ToDataSourceResult(request);

        }

        //[HttpGet("GoodsDeliveryAuthorization/GetDeliveryAuthorizationById/{Id}")]
        //[HttpGet("/GoodsDeliveryAuthorization/GetDeliveryAuthorizationById/{Id}")]
        //[HttpGet("[action]/{Id}")]
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetDeliveryAuthorizationById([FromBody]GoodsDeliveryAuthorization _GoodsDeliveryAuthorizationp)
        {
            GoodsDeliveryAuthorization _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationById/" + _GoodsDeliveryAuthorizationp.GoodsDeliveryAuthorizationId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);

                }

                if (_GoodsDeliveryAuthorization == null)
                {
                    _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_GoodsDeliveryAuthorization);
        }





        [HttpPost("[controller]/[action]")]
        [HttpPost("[action]")]
        // public async Task<ActionResult<GoodsDeliveryAuthorization>> SaveGoodsDeliveryAuthorization([FromBody]GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorization)
        public async Task<ActionResult<GoodsDeliveryAuthorization>> SaveGoodsDeliveryAuthorization([FromBody]dynamic dto)
        {
            GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorizationDTO();

            try
            {
                _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationDTO>(dto.ToString());
                if (_GoodsDeliveryAuthorization != null)
                {
                    GoodsDeliveryAuthorization _listGoodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationById/" + _GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId);
                    string valorrespuesta = "";
                    _GoodsDeliveryAuthorization.FechaModificacion = DateTime.Now;
                    _GoodsDeliveryAuthorization.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listGoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);

                    }

                    if (_listGoodsDeliveryAuthorization == null) { _listGoodsDeliveryAuthorization = new GoodsDeliveryAuthorization(); }

                    if (_listGoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId == 0)
                    {
                        _GoodsDeliveryAuthorization.FechaCreacion = DateTime.Now;
                        _GoodsDeliveryAuthorization.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_GoodsDeliveryAuthorization);
                        var value = (insertresult.Result as ObjectResult).Value;
                        _GoodsDeliveryAuthorization = ((GoodsDeliveryAuthorizationDTO)(value));
                        if (_GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId == 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero el documento!"));
                        }
                    }
                    else
                    {
                        //var updateresult = await Update(_GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId, _GoodsDeliveryAuthorization);
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

            return Json(_GoodsDeliveryAuthorization);
        }



        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request)
        {
            var res = await GetGoodsDeliveryAuthorizationNoSelectedBoletaSalida();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetGoodsDeliveryAuthorizationNoSelectedBoletaSalida())
                {
                    if (values.Contains(order.GoodsDeliveryAuthorizationId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        
        [HttpGet("[controller]/[action]")]
        [HttpGet("[action]")]
        private async Task<List<GoodsDeliveryAuthorization>> GetGoodsDeliveryAuthorizationNoSelectedBoletaSalida()
        {
            List<GoodsDeliveryAuthorization> _GoodsDeliveryAuthorization = new List<GoodsDeliveryAuthorization>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationNoSelectedBoletaSalida");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorization>>(valorrespuesta);
                    _GoodsDeliveryAuthorization = (from c in _GoodsDeliveryAuthorization
                                                   select new GoodsDeliveryAuthorization
                                                   {
                                                       GoodsDeliveryAuthorizationId = c.GoodsDeliveryAuthorizationId,
                                                       CustomerName = "Numero de autorización: " + c.GoodsDeliveryAuthorizationId + "  ||Nombre:" + c.CustomerName + " ||Fecha: "
                                                      + c.DocumentDate

                                                   }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _GoodsDeliveryAuthorization;
        }


        [HttpGet("[controller]/[action]")]
        [HttpGet("[action]")]
        private async Task<List<GoodsDeliveryAuthorization>> GetGoodsDeliveryAuthorization()
        {
            List<GoodsDeliveryAuthorization> _GoodsDeliveryAuthorization = new List<GoodsDeliveryAuthorization>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorization/GetGoodsDeliveryAuthorizationNoSelected");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorization>>(valorrespuesta);
                    _GoodsDeliveryAuthorization = (from c in _GoodsDeliveryAuthorization
                                                   select new GoodsDeliveryAuthorization
                                                   {
                                                       GoodsDeliveryAuthorizationId = c.GoodsDeliveryAuthorizationId,
                                                       CustomerName = "Numero de autorización: " + c.GoodsDeliveryAuthorizationId + "  ||Nombre:" + c.CustomerName + " ||Fecha: "
                                             + c.DocumentDate

                                                   }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _GoodsDeliveryAuthorization;
        }



        // POST: GoodsDeliveryAuthorization/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> Insert(GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorization)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsDeliveryAuthorization.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsDeliveryAuthorization.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorization/Insert", _GoodsDeliveryAuthorization);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_GoodsDeliveryAuthorization);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorization }, Total = 1 });
        }

        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> Update(Int64 id, GoodsDeliveryAuthorization _GoodsDeliveryAuthorization)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorization/Update", _GoodsDeliveryAuthorization);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorization }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorization>> Delete([FromBody]GoodsDeliveryAuthorization _GoodsDeliveryAuthorization)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorization/Delete", _GoodsDeliveryAuthorization);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorization = JsonConvert.DeserializeObject<GoodsDeliveryAuthorization>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorization }, Total = 1 });
        }



        [HttpGet("[controller]/[action]/{id}")]
        public ActionResult SFGoodsDeliveryAuthorization(Int64 id)
        {

            

            //return View(_GoodsDeliveryAuthorization);

            GoodsDeliveryAuthorizationDTO _GoodsDeliveryAuthorization = new GoodsDeliveryAuthorizationDTO { GoodsDeliveryAuthorizationId = id, };
            try
            {

                string basePath = _hostingEnvironment.WebRootPath;
                FileStream inputStream = new FileStream(basePath + "/ReportsTemplate/GoodsDeliveryAuthorization.rdl", FileMode.Open, FileAccess.Read);
                ReportWriter reportWriter = new ReportWriter(inputStream);
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter() { Name = "IdCD", Labels = new List<string>() { _GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId.ToString() }, Values = new List<string>() { _GoodsDeliveryAuthorization.GoodsDeliveryAuthorizationId.ToString() } });
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
        }




    }



    public class GoodsDeliveryAuthorizationParams
    {
        public Int64[] values { get; set; }

        public Int64 CustomerId { get; set; }

        public List<Int64> CertificadosSeleccionados { get; set; }

    }





}