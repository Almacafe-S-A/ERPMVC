using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class VendorInvoiceController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public VendorInvoiceController(ILogger<VendorInvoiceController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Proveedores.Factura Proveedores")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwVendorInvoice([FromBody]VendorInvoice _VendorInvoicep)
        {
            VendorInvoiceDTO _VendorInvoice = new VendorInvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoiceById/" + _VendorInvoicep.VendorInvoiceId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoiceDTO>(valorrespuesta);
                   
                }

                if (_VendorInvoice == null)
                {
                    _VendorInvoice = new VendorInvoiceDTO { VendorInvoiceDate = DateTime.Now, ReceivedDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(30), BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")), editar = 1 };
                }

                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_VendorInvoice);

        }




        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<VendorInvoice> _VendorInvoice = new List<VendorInvoice>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoice");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<List<VendorInvoice>>(valorrespuesta);
                    _VendorInvoice = _VendorInvoice.OrderByDescending(q => q.VendorInvoiceId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _VendorInvoice.ToDataSourceResult(request);

        }

        public async Task<ActionResult> VendorInvoiceById([FromBody]VendorInvoiceDTO _sarpara)
        {
            VendorInvoiceDTO _VendorInvoice = new VendorInvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoiceById/" + _sarpara.VendorInvoiceId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoiceDTO>(valorrespuesta);
                }
                if(_VendorInvoice == null)
                {
                    _VendorInvoice = new VendorInvoiceDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_VendorInvoice));
        }

        public async Task<ActionResult> Aprobar([FromBody] VendorInvoiceDTO _sarpara)
        {
            VendorInvoiceDTO _VendorInvoice = new VendorInvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/Aprobar/" + _sarpara.VendorInvoiceId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoiceDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
            return await Task.Run(() => Ok(_VendorInvoice));
        }


        public async Task<ActionResult> Anular([FromBody] VendorInvoiceDTO _sarpara)
        {
            VendorInvoiceDTO _VendorInvoice = new VendorInvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/Anular/" + _sarpara.VendorInvoiceId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoiceDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
            return await Task.Run(() => Ok(_VendorInvoice));
        }
        public async Task<DataSourceResult> GetVendorInvoiceByVendorId([DataSourceRequest]DataSourceRequest request, Int64 VendorId)
        {
            List<VendorInvoice> _VendorInvoice = new List<VendorInvoice>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoice");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<List<VendorInvoice>>(valorrespuesta);
                    _VendorInvoice = (from c in _VendorInvoice
                                           .Where(q => q.VendorId == VendorId)
                                            select new VendorInvoice
                                            {
                                                VendorInvoiceId = c.VendorInvoiceId,
                                                VendorInvoiceName = $"Fecha:{c.FechaCreacion} || No: {c.NumeroDEI} ||  CAI:{ c.CAI } || Total:{c.Total}",
                                                VendorId = c.VendorId,
                                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _VendorInvoice.ToDataSourceResult(request);
        }

        public async Task<DataSourceResult> GetVendorInvoiceByVendorPendienteRetencion([DataSourceRequest] DataSourceRequest request, Int64 VendorId)
        {
            List<VendorInvoice> _VendorInvoice = new List<VendorInvoice>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoicePendienteRetencion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<List<VendorInvoice>>(valorrespuesta);
                    _VendorInvoice = (from c in _VendorInvoice
                                           .Where(q => q.VendorId == VendorId)
                                      select new VendorInvoice
                                      {
                                          VendorInvoiceId = c.VendorInvoiceId,
                                          VendorInvoiceName = $"Fecha:{c.FechaCreacion} || No: {c.NumeroDEI} ||  CAI:{c.CAI} || Total:{c.Total}",
                                          VendorId = c.VendorId,
                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
            return _VendorInvoice.ToDataSourceResult(request);
        }


        [HttpPost/*("[action]")*/]
        public async Task<ActionResult<VendorInvoiceDTO>> SaveVendorInvoice([FromBody]VendorInvoiceDTO _VendorInvoice)
        {

            try
            {
                VendorInvoice _listVendorInvoice = new VendorInvoice();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoiceById/" + _VendorInvoice.VendorInvoiceId);
                string valorrespuesta = "";
                _VendorInvoice.FechaModificacion = DateTime.Now;
                _VendorInvoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listVendorInvoice = JsonConvert.DeserializeObject<VendorInvoice>(valorrespuesta);
                }

                if (_listVendorInvoice == null) { _listVendorInvoice = new VendorInvoice(); }

                if (_listVendorInvoice.VendorInvoiceId == 0)
                {
                    var insertresult = await Insert(_VendorInvoice);

                    if (insertresult.Result is BadRequestObjectResult)
                    {

                        return await Task.Run(() => BadRequest(insertresult.Result));
                    }

                }
                else
                {
                    var updateresult = await Update(_VendorInvoice.VendorInvoiceId, _VendorInvoice);

                    if (updateresult.Result is BadRequestObjectResult)
                    {

                        return await Task.Run(() => BadRequest(updateresult.Result));
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"No se genero la factura : {ex.ToString()}"));
            
                throw ex;
            }

            return Json(_VendorInvoice);
        }

        // POST: VendorInvoice/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<VendorInvoice>> Insert(VendorInvoiceDTO _VendorInvoice)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _VendorInvoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                _VendorInvoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorInvoice/Insert", _VendorInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoiceDTO>(valorrespuesta);
                }
                else
                {
                    string d =  await (result.Content.ReadAsStringAsync());
                    throw  new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (ex);
                // return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_VendorInvoice);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _VendorInvoice }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendorInvoice>> Update(Int64 id, VendorInvoice _VendorInvoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/VendorInvoice/Update", _VendorInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoice>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"{ex.Message}"));
            }

            return await Task.Run(() => Ok(_VendorInvoice));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _VendorInvoice }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<VendorInvoice>> Delete([FromBody]VendorInvoice _VendorInvoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorInvoice/Delete", _VendorInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorInvoice = JsonConvert.DeserializeObject<VendorInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(() => Ok(_VendorInvoice));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _VendorInvoice }, Total = 1 });
        }



        [HttpGet]
        public async Task<ActionResult> SFVendorInvoice(Int32 id)
        {
            try
            {
                VendorInvoiceDTO _VendorInvoicedto = new VendorInvoiceDTO { VendorInvoiceId = id, };
                return await Task.Run(()=> View(_VendorInvoicedto));
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