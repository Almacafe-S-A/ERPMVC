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
    public class ProductRelationController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public ProductRelationController(ILogger<ProductRelationController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: ProductRelation
        public ActionResult ProductRelation()
        {
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetProductRelation([DataSourceRequest]DataSourceRequest request)
        {
            List<ProductRelation> _cais = new List<ProductRelation>();
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
                    _cais = JsonConvert.DeserializeObject<List<ProductRelation>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _cais.ToDataSourceResult(request);

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
                var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetProductRelation/" + _sarpara.RelationProductId);
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ProductRelation);

        }

        


        


        [HttpPost]
        public async Task<ActionResult<ProductRelation>> SaveProductRelation([FromBody]ProductRelationDTO _ProductRelationp)
        {
            ProductRelation _ProductRelation = _ProductRelationp;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductRelation/GetProductRelationById/" + _ProductRelation.RelationProductId);
                string valorrespuesta = "";
                _ProductRelation.FechaModificacion = DateTime.Now;
                _ProductRelation.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductRelation = JsonConvert.DeserializeObject<ProductRelation>(valorrespuesta);
                }

                if (_ProductRelation == null) { _ProductRelation = new Models.ProductRelation(); }
                //_ProductRelation.Id.ToString("N");
                // _ProductRelation = _ProductRelation.Where(q => q.Id == Id).ToList();


                if (_ProductRelationp.RelationProductId.ToString() == "00000000-0000-0000-0000-000000000000")

                {
                    _ProductRelation.FechaCreacion = DateTime.Now;
                    _ProductRelation.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProductRelationp);
                }
                else
                {
                    _ProductRelationp.UsuarioCreacion = _ProductRelation.UsuarioCreacion;
                    _ProductRelationp.FechaCreacion = _ProductRelation.FechaCreacion;
                    var updateresult = await Update(_ProductRelationp.RelationProductId, _ProductRelationp);
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
        public async Task<ActionResult> Insert(ProductRelation _ProductRelationp)
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
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProducRelation/Insert", _ProductRelation);
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

        [HttpPut]
        public async Task<IActionResult> Update(Int64 id, ProductRelation _ProductRelation)
        {

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProductRelation.FechaModificacion = DateTime.Now;
                _ProductRelation.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/ProducRelation/Update", _ProductRelation);
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


        [HttpDelete("Id")]
        public async Task<ActionResult<ProductRelation>> Delete(Int64 Id, ProductRelation _ProductRelation)
        {

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProducRelation/Delete", _ProductRelation);
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