using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class ProductTypeController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public ProductTypeController(ILogger<ProductTypeController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public IActionResult ProductType()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Catalogos.Tipo de Producto.Agregar Tipo de Producto", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Catalogos.Tipo de Producto.Editar Tipo de Producto", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Catalogos.Tipo de Producto.Eliminar Tipo de Producto", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Catalogos.Tipo de Producto.Exportar Tipo de Producto", "true");
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddProductType([FromBody]ProductTypeDTO _sarpara)

        {
            ProductTypeDTO _ProductType = new ProductTypeDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductType/GetProductTypeById/" + _sarpara.ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<ProductTypeDTO>(valorrespuesta);

                }

                if (_ProductType == null)
                {
                    _ProductType = new ProductTypeDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ProductType);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ProductType> _ProductType = new List<ProductType>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductType/GetProductType");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<List<ProductType>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProductType.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetProductTypeByID([DataSourceRequest]DataSourceRequest request, Int64 Id)
        {
            List<ProductType> _ProductType = new List<ProductType>();
            try
            {
                
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductType/GetProductTypeById/"+Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<List<ProductType>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return (_ProductType.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<JsonResult> GetTypeJson([DataSourceRequest]DataSourceRequest request)
        {
            List<ProductType> _ProductType = new List<ProductType>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductType/GetProductType");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<List<ProductType>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_ProductType.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProductType>> SaveProductType([FromBody]ProductTypeDTO _ProductTypeS)
        {
            ProductType _ProductType = _ProductTypeS;
            try
            {
                ProductType _listProductType = new ProductType();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductType/GetProductTypeById/" + _ProductType.ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<ProductTypeDTO>(valorrespuesta);
                }

                if (_ProductType == null) { _ProductType = new Models.ProductType(); }

                if (_ProductTypeS.ProductTypeId == 0)
                {
                    var insertresult = await Insert(_ProductTypeS);
                }
                else
                {
                    var updateresult = await Update(_ProductType.ProductTypeId, _ProductTypeS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProductType);
        }

        // POST: ProductType/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProductType>> Insert(ProductType _ProductTypep)
        {
            ProductType _ProductType = _ProductTypep;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
             
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductType/Insert", _ProductType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<ProductType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductType }, Total = 1 });
        }



        [HttpPut("{ProductTypeId}")]
        public async Task<ActionResult<ProductType>> Update(Int64 ProductTypeId, ProductType _ProductTypep)
        {
            ProductType _ProductType = _ProductTypep;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductType/Update", _ProductType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<ProductType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductType }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<ProductType>> Delete(Int64 ProductTypeId, ProductType _ProductType)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductType/Delete", _ProductType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductType = JsonConvert.DeserializeObject<ProductType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductType }, Total = 1 });
        }





    }
}