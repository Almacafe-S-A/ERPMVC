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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class VendorController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public VendorController(ILogger<VendorController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }

        
        public async Task<ActionResult> Proveedores(Int64 VendorId)
        {
            Vendor _Vendor = new Vendor();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Vendor/GetVendorById/" + VendorId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<Vendor>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_Vendor));
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> GetVendor([DataSourceRequest]DataSourceRequest request)
        {
            List<Vendor> _Vendor = new List<Vendor>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Vendor/GetVendor");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<List<Vendor>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Vendor.ToDataSourceResult(request));

        }




        [HttpPost]
        public async Task<ActionResult> Insert(Vendor _Vendor)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Vendor.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Vendor.FechaCreacion = DateTime.Now;
                _Vendor.UsuarioModificacion= HttpContext.Session.GetString("user");
                _Vendor.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Vendor/Insert", _Vendor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<Vendor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Vendor }, Total = 1 });
        }


        [HttpPut("VendorId")]
        public async Task<IActionResult> Update(Int64 VendorId, Vendor _Vendor)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Vendor/Update", _Vendor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<Vendor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Vendor }, Total = 1 });
        }

        // POST: Vendor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult<Vendor>> SaveVendor([FromBody]VendorDTO _VendorP)
        {
            Vendor _Vendor = _VendorP;
            try
            {
                Vendor _Vendorlist = new Vendor();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Vendor/GetVendorById/" + _Vendor.VendorId);
                string valorrespuesta = "";
                _Vendor.FechaModificacion = DateTime.Now;
                _Vendor.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<Vendor>(valorrespuesta);
                }

                if (_Vendor == null) { _Vendor = new Models.Vendor(); }

                if (_VendorP.VendorId == 0)
                {
                    //cambio para ingresar un valor en Idendtidad
                    _VendorP.Identidad = HttpContext.Session.GetString("user");
                    _VendorP.FechaCreacion = DateTime.Now;
                    _VendorP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_VendorP);
                }
                else
                {
                    _VendorP.Identidad = _Vendor.Identidad;
                   _VendorP.UsuarioCreacion = _Vendor.UsuarioCreacion;
                    _VendorP.FechaCreacion = _Vendor.FechaCreacion;
                    var updateresult = await Update(_Vendor.VendorId, _VendorP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_VendorP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddVendor([FromBody]VendorDTO _sarpara)
        {
            VendorDTO _Vendor = new VendorDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Vendor/GetVendorById/" + _sarpara.VendorId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<VendorDTO>(valorrespuesta);

                }

                if (_Vendor == null)
                {
                    _Vendor = new VendorDTO();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            /*  try
             {
                 string baseadress = config.Value.urlbase;
                 HttpClient _client = new HttpClient();
                 _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                 this.ViewBag.ConfigurationVendor = await _client.GetAsync(baseadress + "api/ConfigurationVendor/GetConfigurationVendorActive");


             }
             catch (Exception ex)
             {
                 _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                 throw ex;
             }
             */
            return await Task.Run(() => PartialView(_Vendor));
           // return PartialView(_Vendor);

        }
        // GET: Customer/Details/5

        /*
                [HttpGet("[action]")]
                public async Task<ActionResult> Proveedores(Int64 PurchId)
                {
                    Purch _customers = new Purch();
                    try
                    {
                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + PurchId);
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _customers = JsonConvert.DeserializeObject<Purch>(valorrespuesta);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                        throw ex;
                    }



                    return await Task.Run(() => View(_customers));
                }
          */
        [HttpGet("[action]")]
        public async Task<ActionResult> GetVendorById(Int64 VendorId)
        {
            Vendor _Vendor = new Vendor();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Vendor/GetVendorById/" + VendorId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Vendor = JsonConvert.DeserializeObject<Vendor>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_Vendor));
        }


    
    async Task<IEnumerable<VendorType>> ObtenerVendorTypes()
    {
        IEnumerable<VendorType> vendortypes = null;
        try
        {
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + "api/VendorTypes/GetVendorType");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                    vendortypes = JsonConvert.DeserializeObject<IEnumerable<VendorType>>(valorrespuesta);

            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            throw ex;
        }
        ViewData["defaultvendor"] = vendortypes.FirstOrDefault();
        return vendortypes;

    }
   }
}