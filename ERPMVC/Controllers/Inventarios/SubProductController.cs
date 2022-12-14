using System;
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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class SubProductController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public SubProductController(ILogger<HomeController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult SubProduct()
        {
            return View();
        }

        [Authorize(Policy = "Catalogos.Productos de Cliente")]
        [HttpGet("[controller]/[action]")]
        public IActionResult SubProductClientes()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Catalogos.Productos de Cliente.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Catalogos.Productos de Cliente.Editar", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Catalogos.Productos de Cliente.Eliminar", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Catalogos.Productos de Cliente.Exportar", "true");
            return View();
        }

        [Authorize(Policy = "Catalogos.SubServicios")]
        [HttpGet("[controller]/[action]")]
        public IActionResult SubProductPropios()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Catalogos.SubServicios.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Catalogos.SubServicios.Editar", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Catalogos.SubServicios.Eliminar", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Catalogos.SubServicios.Exportar", "true");
            return View();
        }

        [Authorize(Policy = "Monitoreo.Productos Prohibidos")]
        [HttpGet("[controller]/[action]")]
        public IActionResult SubProductProhibidos()
        {
            ViewData["permisos"] = _principal;
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
                    _SubProduct = _SubProduct.OrderByDescending(q=>q.SubproductId).Where(q => q.ProductTypeId != 11).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _SubProduct.ToDataSourceResult(request);

        }

        [HttpGet]
        public async Task<DataSourceResult> GetProductosCliente([DataSourceRequest]DataSourceRequest request)
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
                    _SubProduct = _SubProduct.OrderByDescending(q => q.SubproductId).Where(q => q.ProductTypeId == 2).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _SubProduct.ToDataSourceResult(request);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetByProductType([DataSourceRequest]DataSourceRequest request)
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
                    _SubProduct = _SubProduct.OrderByDescending(q => q.SubproductId).Where(q => q.ProductTypeId == 11).ToList();
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

        public async Task<DataSourceResult> GetSubProductoByTipoGrid([DataSourceRequest]DataSourceRequest request, Int64 ProductTypeId)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeId/" + ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return (_SubProducto.ToDataSourceResult(request));
        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetSubProductoByTipoGridPropios([DataSourceRequest]DataSourceRequest request, Int64 ProductTypeId)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeIdPropios/" + ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return (_SubProducto.ToDataSourceResult(request));
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipoJson([DataSourceRequest]DataSourceRequest request, Int64 ProductTypeId)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeId/" + ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SubProducto.ToDataSourceResult(request));
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipoJsonActivos([DataSourceRequest] DataSourceRequest request, Int64 ProductTypeId)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeId/" + ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta).Where(w => w.IdEstado==1).ToList();
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_SubProducto.ToDataSourceResult(request));
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
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductbByProductTypeId/" + ProductTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SubProducto);
        }


        public async Task<ActionResult<UnitOfMeasure>> GetProductoUnitOfMeasure(int SubProductId,int CustomerId)
        {
            UnitOfMeasure _UnitOfMeasure = new UnitOfMeasure();
            try
            {
                //int ProductId = dto.SubProductId;
                //int customerId = dto.CustomerId;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/UnitOfMeasure/GetProductoUnitOfMeasure/{SubProductId}/{CustomerId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UnitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(valorrespuesta);
                    //_UnitOfMeasure = _UnitOfMeasure.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_UnitOfMeasure);
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
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductById/" + _sarpara.SubproductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProduct = JsonConvert.DeserializeObject<SubProductDTO>(valorrespuesta);
                   
                }

                if (_SubProduct == null)
                {

                    _SubProduct = new SubProductDTO {  IsEnable= _sarpara.IsEnable , TituloVentana = _sarpara.TituloVentana };
                }

                _SubProduct.TituloVentana = _sarpara.TituloVentana;
                _SubProduct.TipoProducto = _sarpara.TipoProducto;
                _SubProduct.ProductTypeId = _sarpara.ProductTypeId;
                _SubProduct.IsEnable = _sarpara.IsEnable;
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
                _CustomerTypeSubProduct.ProductTypeId = _CustomerTypeSubProduct.ProductTypeId == 0 ? 2 : _CustomerTypeSubProduct.ProductTypeId;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/GetSubProductoByTipoByCustomer", _CustomerTypeSubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
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
        public async Task<ActionResult<SubProduct>> GetSubProductoByTipoByCustomerActivos([DataSourceRequest] DataSourceRequest request, CustomerTypeSubProduct _CustomerTypeSubProduct)
        {
            List<SubProduct> _SubProducto = new List<SubProduct>();
            try
            {
                // CustomerTypeSubProduct _CustomerTypeSubProduct = new CustomerTypeSubProduct();
                _CustomerTypeSubProduct.ProductTypeId = _CustomerTypeSubProduct.ProductTypeId == 0 ? 2 : _CustomerTypeSubProduct.ProductTypeId;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/GetSubProductoByTipoByCustomer", _CustomerTypeSubProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducto = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta).Where(w => w.IdEstado == 1).ToList();
                    _SubProducto = _SubProducto.OrderByDescending(q => q.SubproductId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_SubProducto.ToDataSourceResult(request));
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
                var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductById/" + SubProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubProducts = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);

                }

                if (_SubProducts == null) { _SubProducts = new SubProduct(); }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_SubProducts);
        }

        [HttpPost("[action]")]
        //[HttpPost("[controller]/[action]")]
        public async Task<ActionResult<SubProduct>> SaveSubProduct([FromBody]SubProductDTO _SubProductS)
        {
            //   SubProduct _SubProductS = new SubProduct(); //JsonConvert.DeserializeObject<SubProductDTO>(dto.ToString());
            SubProduct _SubProduct = _SubProductS;
            try
                {
                    // _SubProductS = JsonConvert.DeserializeObject<SubProductDTO>(dto.ToString());
                    SubProduct _listProduct = new SubProduct();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result1 = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductByProductName/" + _SubProduct.ProductName);
                string valorrespuesta1 = "";
                if (result1.IsSuccessStatusCode)
                {
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        _SubProduct = JsonConvert.DeserializeObject<SubProductDTO>(valorrespuesta1);
                }

                if (_SubProduct == null) { _SubProduct = new Models.SubProduct(); }

                if (_SubProduct.SubproductId > 0)
                {
                    if (_SubProduct.SubproductId != _SubProductS.SubproductId)
                        return await Task.Run(() => BadRequest($"Ya existe un Producto con el mismo Nombre."));
                }

                if (_SubProductS.SubproductId == 0)
                {
                        _SubProductS.FechaCreacion = DateTime.Now;
                        _SubProductS.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_SubProductS);

                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductById/" + _SubProductS.SubproductId);
                    string valorrespuesta = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _SubProduct = JsonConvert.DeserializeObject<SubProductDTO>(valorrespuesta);
                    }
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

            return Json(_SubProductS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SubProduct>> Insert(SubProduct _SubProduct)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _SubProduct.UsuarioCreacion = HttpContext.Session.GetString("user");
                _SubProduct.UsuarioModificacion = HttpContext.Session.GetString("user");
                _SubProduct.FechaCreacion = DateTime.Now;
                _SubProduct.FechaModificacion = DateTime.Now;
                _SubProduct.ProductName = _SubProduct.ProductName.ToUpper();
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
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return Ok(_SubProduct);
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
                _SubProduct.ProductName = _SubProduct.ProductName.ToUpper();
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/Update", _SubProduct);
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


        [HttpDelete]
        public async Task<ActionResult<SubProduct>> Delete(SubProduct _subproduct)
        {
            SubProduct _SubProduct = new SubProduct();
            List<InvoiceLine> _InvoiceLine = new List<InvoiceLine>();
            List<ProformaInvoiceLine> _ProformaInvoiceLine = new List<ProformaInvoiceLine>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/SubProduct/ValidationDelete/" + _subproduct);
                string valorrespuesta1 = "";

                if (result1.IsSuccessStatusCode)
                {

                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                }
                
                if (valorrespuesta1 == "0")
                {
                    var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/Delete", _subproduct);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _SubProduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);
                    }

                    else
                    {
                        return await Task.Run(() => BadRequest("Este registro tiene referencia a otros datos,No se puede Eliminar"));
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("Este registro tiene referencia a otros datos,No se puede Eliminar"));
                }


            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SubProduct }, Total = 1 });
            //try
            //{
            //    string baseadress = config.Value.urlbase;
            //    HttpClient _client = new HttpClient();
            //    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

            //    var result = await _client.PostAsJsonAsync(baseadress + "api/SubProduct/Delete", _SubProduct);
            //    string valorrespuesta = "";
            //    if (result.IsSuccessStatusCode)
            //    {
            //        valorrespuesta = await (result.Content.ReadAsStringAsync());
            //        _SubProduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            //    return BadRequest($"Ocurrio un error: {ex.Message}");
            //}



            //return new ObjectResult(new DataSourceResult { Data = new[] { _SubProduct }, Total = 1 });
        }






    }


}