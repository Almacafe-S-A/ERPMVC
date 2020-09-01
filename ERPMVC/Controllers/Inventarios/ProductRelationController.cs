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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProductRelationController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public ProductRelationController(ILogger<ProductRelationController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        // GET: ProductRelation
        [Authorize(Policy = "Catalogos.Relacion Servicio")]
        public ActionResult ProductRelation()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Catalogos.Relacion Servicio.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Catalogos.Relacion Servicio.Editar", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Catalogos.Relacion Servicio.Exportar", "true");
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetProductRelation([DataSourceRequest]DataSourceRequest request)
        {
            List<ProductRelation> _ProductRelation = new List<ProductRelation>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetProductRelation");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductRelation = JsonConvert.DeserializeObject<List<ProductRelation>>(valorrespuesta);
                    _ProductRelation = _ProductRelation.OrderByDescending(q => q.RelationProductId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProductRelation.ToDataSourceResult(request);

        }


        public async Task<ActionResult> GetSubProductByProductIdRelation([DataSourceRequest]DataSourceRequest request, Int64 ProductId)
        {
            List<SubProduct> _clientes = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetSubProductByProductId/" + ProductId);
                //  var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetSubProductByProductId/" + ProductId.ProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _clientes.Add(new SubProduct { SubproductId = 0, ProductName = "Impuesto sobre ventas" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            // return Json(_clientes);
            return Json(_clientes.ToDataSourceResult(request));

        }


        public async Task<ActionResult> GetSubProductByProductId([DataSourceRequest]DataSourceRequest request, Int64 ProductId)
        {
            List<SubProduct> _clientes = new List<SubProduct>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetSubProductByProductId/" + ProductId);
                //  var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetSubProductByProductId/" + ProductId.ProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<SubProduct>>(valorrespuesta);
                    _clientes = _clientes.OrderBy(q => q.ProductName).ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_clientes);
           // return Json(_clientes.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddProductRelation([FromBody]ProductRelationDTO _sarpara)
        {
            ProductRelation _ProductRelation = new ProductRelationDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                

                //if (_sarpara.RelationProductId > 0)
                //{
                    var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetProductRelationById/" + _sarpara.RelationProductId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _ProductRelation = JsonConvert.DeserializeObject<ProductRelationDTO>(valorrespuesta);

                    }

                    if (_ProductRelation == null)
                    {
                        _ProductRelation = new ProductRelationDTO();
                    }
                //}
                //else
                //{
                //    var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetProductRelation/" + _sarpara.RelationProductId);
                //    string valorrespuesta = "";
                //    if (result.IsSuccessStatusCode)
                //    {
                //        valorrespuesta = await (result.Content.ReadAsStringAsync());
                //        _ProductRelation = JsonConvert.DeserializeObject<ProductRelationDTO>(valorrespuesta);

                //    }

                //    if (_ProductRelation == null)
                //    {
                //        _ProductRelation = new ProductRelationDTO();
                //    }
                //}
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ProductRelation);

        }

        public async Task<ActionResult> SaveProductRelation([FromBody]ProductRelation _ProductRelationp)
        {
            ProductRelation _ProductRelation = new ProductRelation();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/ProductRelation/GetProductRelationById/{_ProductRelationp.RelationProductId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductRelation = JsonConvert.DeserializeObject<ProductRelation>(valorrespuesta);
                }


                if (_ProductRelation==null)
                {
                    _ProductRelationp.FechaCreacion = DateTime.Now;
                    _ProductRelationp.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult =  await Insert(_ProductRelationp);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(((BadRequestObjectResult)insertresult.Result).Value);
                    }
                }
                else
                {
                    _ProductRelationp.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _ProductRelationp.FechaModificacion = DateTime.Now;
                    var updateresult = await Update( _ProductRelationp);
                    if (updateresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(((BadRequestObjectResult)updateresult.Result).Value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_ProductRelation);
        }
        [HttpPost]
        public async Task<ActionResult<ProductRelation>> Insert(ProductRelation _ProductRelationp)
        {
            ProductRelation _ProductRelation = _ProductRelationp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProductRelation.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ProductRelation.UsuarioModificacion = HttpContext.Session.GetString("user");
                _ProductRelation.FechaCreacion = DateTime.Now;
                _ProductRelation.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductRelation/Insert", _ProductRelation);

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductRelation = JsonConvert.DeserializeObject<ProductRelation>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    if (error.Length > 100)
                    {
                        error = "Error al Guardar";
                    }
                    return BadRequest($"{error}");
                }
                

            }
            catch (Exception ex)
            {
                string error = ex.Message                    ;
                if (error.Length > 100)
                {
                    error = "Error al Guardar";
                }
                return BadRequest($"{error}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductRelation }, Total = 1 });
        }

        [HttpPut]
        public async Task<ActionResult<ProductRelation>> Update(ProductRelation _ProductRelation)
        {

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProductRelation.FechaModificacion = DateTime.Now;
                _ProductRelation.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductRelation/Update", _ProductRelation);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductRelation = JsonConvert.DeserializeObject<ProductRelation>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    if (error.Length > 100)
                    {
                        error = "Error al Guardar";
                    }
                    return BadRequest($"{error}");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductRelation }, Total = 1 });
        }


        [HttpDelete]
        public async Task<ActionResult<ProductRelation>> Delete(Int64 Id, ProductRelation _ProductRelation)
        {

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.DeleteAsync(baseadress + $"api/ProductRelation/Delete/{_ProductRelation.RelationProductId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductRelation = JsonConvert.DeserializeObject<ProductRelation>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductRelation }, Total = 1 });
        }
    }
}