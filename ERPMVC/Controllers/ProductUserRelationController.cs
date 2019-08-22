using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class ProductUserRelationController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ProductUserRelationController(ILogger<ProductUserRelationController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwProductUserRelation(Int64 Id = 0)
        {
            ProductUserRelation _ProductUserRelation = new ProductUserRelation();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductUserRelation/GetProductUserRelationById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductUserRelation = JsonConvert.DeserializeObject<ProductUserRelation>(valorrespuesta);

                }

                if (_ProductUserRelation == null)
                {
                    _ProductUserRelation = new ProductUserRelation();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ProductUserRelation);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ProductUserRelation> _ProductUserRelation = new List<ProductUserRelation>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductUserRelation/GetProductUserRelation");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductUserRelation = JsonConvert.DeserializeObject<List<ProductUserRelation>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProductUserRelation.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProductUserRelation>> SaveProductUserRelation([FromBody]ProductUserRelation _ProductUserRelation)
        {

            try
            {
                ProductUserRelation _listProductUserRelation = new ProductUserRelation();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProductUserRelation/GetProductUserRelationById/" + _ProductUserRelation.ProductUserRelationId);
                string valorrespuesta = "";
                _ProductUserRelation.FechaModificacion = DateTime.Now;
                _ProductUserRelation.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listProductUserRelation = JsonConvert.DeserializeObject<ProductUserRelation>(valorrespuesta);
                }

                if (_listProductUserRelation.ProductUserRelationId == 0)
                {
                    _ProductUserRelation.FechaCreacion = DateTime.Now;
                    _ProductUserRelation.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProductUserRelation);
                }
                else
                {
                    var updateresult = await Update(_ProductUserRelation.ProductUserRelationId, _ProductUserRelation);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProductUserRelation);
        }

        // POST: ProductUserRelation/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProductUserRelation>> Insert(ProductUserRelation _ProductUserRelation)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProductUserRelation.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ProductUserRelation.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductUserRelation/Insert", _ProductUserRelation);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductUserRelation = JsonConvert.DeserializeObject<ProductUserRelation>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_ProductUserRelation);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ProductUserRelation }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductUserRelation>> Update(Int64 id, ProductUserRelation _ProductUserRelation)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ProductUserRelation/Update", _ProductUserRelation);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductUserRelation = JsonConvert.DeserializeObject<ProductUserRelation>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductUserRelation }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProductUserRelation>> Delete([FromBody]ProductUserRelation _ProductUserRelation)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ProductUserRelation/Delete", _ProductUserRelation);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProductUserRelation = JsonConvert.DeserializeObject<ProductUserRelation>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ProductUserRelation }, Total = 1 });
        }





    }
}