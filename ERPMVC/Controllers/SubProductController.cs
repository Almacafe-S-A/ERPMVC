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
    public class SubProductController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public SubProductController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult SubProduct()
        {
            return View();
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<SubProduct> _SubProduct = new List<SubProduct>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProduct");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _SubProduct.ToDataSourceResult(request);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipo(Int64 ProductTypeId)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeId/"+ ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SubProducto);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddSubProduct([FromBody]SubProductDTO _sarpara)

        {
            SubProductDTO _SubProduct = new SubProductDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Escala/GetSubProductById/" + _sarpara.SubproductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<SubProductDTO>(valorrespuesta);

                }

                if (_SubProduct == null)
                {
                    _SubProduct = new SubProductDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_SubProduct);

        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipoByCustomer([DataSourceRequest]DataSourceRequest request, CustomerTypeSubProduct _CustomerTypeSubProduct)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
               // CustomerTypeSubProduct _CustomerTypeSubProduct = new CustomerTypeSubProduct();
                _CustomerTypeSubProduct.ProductTypeId= _CustomerTypeSubProduct.ProductTypeId ==0 ? 2 : _CustomerTypeSubProduct.ProductTypeId;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/GetSubProductoByTipoByCustomer", _CustomerTypeSubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_SubProducto.ToDataSourceResult(request));
           // return Json(_SubProducto);
        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetProductoById(Int64 SubProductId)
        {
            SubProduct _SubProducts = new SubProduct();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbById/" + SubProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducts = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);

                }

                if(_SubProducts== null) { _SubProducts = new SubProduct(); }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_SubProducts);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubProduct>> SaveSubProduct([FromBody]SubProductDTO _SubProductS)
        {
            SubProduct _SubProduct = _SubProductS;
            try
            {
                SubProduct _listProduct = new SubProduct();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductById/" + _SubProduct.SubproductId);
                string valorrespuesta = "";
                _SubProduct.FechaModificacion = DateTime.Now;
                _SubProduct.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<SubProductDTO>(valorrespuesta);
                }

                if (_SubProduct == null) { _SubProduct = new Models.SubProduct(); }

                if (_SubProductS.SubproductId == 0)
                {
                    _SubProductS.FechaCreacion = DateTime.Now;
                    _SubProductS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_SubProductS);
                }
                else
                {
                    _SubProductS.UsuarioCreacion = _SubProduct.UsuarioCreacion;
                    _SubProductS.FechaCreacion = _SubProduct.FechaCreacion;
                    var updateresult = await Update(_SubProduct.SubproductId, _SubProductS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SubProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SubProduct>> Insert(SubProduct _SubProductp)
        {
            SubProduct _SubProduct = _SubProductp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _SubProductp.UsuarioCreacion = HttpContext.Session.GetString("user");
                _SubProductp.UsuarioModificacion = HttpContext.Session.GetString("user");
                _SubProductp.FechaCreacion = DateTime.Now;
                _SubProductp.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/Insert", _SubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SubProduct }, Total = 1 });
        }


        [HttpPut("{SubProductId}")]
        public async Task<ActionResult<SubProduct>> Update(Int64 SubProductId, SubProduct _SubProductp)
        {
            SubProduct _SubProduct = _SubProductp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _SubProduct.FechaModificacion = DateTime.Now;
                _SubProduct.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/SubProduct/Update", _SubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SubProduct }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<SubProduct>> Delete(Int64 SubProductId, SubProduct _SubProduct)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/Delete", _SubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _SubProduct }, Total = 1 });
        }






    }


}