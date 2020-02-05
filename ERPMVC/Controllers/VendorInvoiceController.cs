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

        public IActionResult Index()
        {
            ViewData["permisoActualizarRecibido"] = _principal.HasClaim("Compras.Factura Proveedores.Actualizar a Recibido", "true");
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
                    _VendorInvoice = new VendorInvoiceDTO { OrderDate = DateTime.Now, ReceivedDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(30), BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")), editar = 1 };
                }
                else
                {
                   // _VendorInvoice.NumeroDEIString = $"{_VendorInvoice.Sucursal}-{_VendorInvoice.Caja}-01-{_VendorInvoice.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_VendorInvoice);

        }



        //COMIENZA APROVACIÓN
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<VendorInvoiceDTO>> Recibido([FromBody]VendorInvoiceDTO _VendorInvoice)
        {
            VendorInvoiceDTO _so = new VendorInvoiceDTO();
            if (_VendorInvoice != null)
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/VendorInvoice/GetVendorInvoiceById/" + _VendorInvoice.VendorInvoiceId);
                    string jsonresult = "";
                    jsonresult = JsonConvert.SerializeObject(_VendorInvoice);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<VendorInvoiceDTO>(valorrespuesta);
                        _so.ReceivedDate = DateTime.Now;

                        var resultsalesorder = await Update(_so.VendorInvoiceId, _so);

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        VendorInvoice resultado = ((VendorInvoice)(value));
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

        public async Task<DataSourceResult> GetVendorInvoiceByVendorId([DataSourceRequest]DataSourceRequest request, Int64 VendorId)
        {
            List<VendorInvoiceDTO> _VendorInvoice = new List<VendorInvoiceDTO>();
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
                    _VendorInvoice = JsonConvert.DeserializeObject<List<VendorInvoiceDTO>>(valorrespuesta);
                    _VendorInvoice = _VendorInvoice.Where(p => p.VendorId == VendorId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _VendorInvoice.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
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
                    _VendorInvoice.FechaCreacion = DateTime.Now;
                    _VendorInvoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _VendorInvoice.IdEstado = 1;
                    _VendorInvoice.Estado = "Activo";
                    var insertresult = await Insert(_VendorInvoice);
                    var value = (insertresult.Result as ObjectResult).Value;
                    
                    VendorInvoiceDTO resultado = ((VendorInvoiceDTO)(value));
                    if (resultado.VendorInvoiceId <= 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero la factura!"));
                    }
                    else
                    {
                       // _VendorInvoice.NumeroDEIString = $"{resultado.Sucursal}-01-{resultado.NumeroDEI.ToString().PadLeft(8, '0')} ";
                    }

                }
                else
                {
                    var updateresult = await Update(_VendorInvoice.VendorInvoiceId, _VendorInvoice);
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

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
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