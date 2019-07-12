﻿using System;
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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class ProductController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ProductController(ILogger<ProductController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Product()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddProduct([FromBody]ProductDTO _sarpara)

        {
            ProductDTO _Product = new ProductDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Product/GetProductById/" + _sarpara.ProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Product = JsonConvert.DeserializeObject<ProductDTO>(valorrespuesta);

                }

                if (_Product == null)
                {
                    _Product = new ProductDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Product);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Product> _Product = new List<Product>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Product/GetProduct");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Product = JsonConvert.DeserializeObject<List<Product>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Product.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Product>> SaveProduct([FromBody]ProductDTO _ProductS)
        {
            Product _Product = _ProductS;
            try
            {
                Product _listProduct= new Product();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Product/GetProductById/" + _Product.ProductId);
                string valorrespuesta = "";
                _Product.FechaModificacion = DateTime.Now;
                _Product.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Product = JsonConvert.DeserializeObject<ProductDTO>(valorrespuesta);
                }

                if (_Product == null) { _Product = new Models.Product(); }

                if (_ProductS.ProductId == 0)
                {
                    _ProductS.FechaCreacion = DateTime.Now;
                    _ProductS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProductS);
                }
                else
                {
                    _ProductS.UsuarioCreacion = _Product.UsuarioCreacion;
                    _ProductS.FechaCreacion = _Product.FechaCreacion;
                    var updateresult = await Update(_Product.ProductId, _ProductS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Product);
        }

        // POST: Product/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Product>> Insert(Product _Productp)
        {
            Product _Product = _Productp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Productp.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Productp.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Productp.FechaCreacion = DateTime.Now;
                _Productp.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Product/Insert", _Product);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Product = JsonConvert.DeserializeObject<Product>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Product }, Total = 1 });
        }

        [HttpPut("{ProductId}")]
        public async Task<ActionResult<Product>> Update(Int64 ProductId, Product _Productp)
        {
            Product _Product = _Productp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Product.FechaModificacion = DateTime.Now;
                _Product.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Product/Update", _Product);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Product = JsonConvert.DeserializeObject<Product>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Product }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<Product>> Delete(Int64 ProductId, Product _Product)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Product/Delete", _Product);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Product = JsonConvert.DeserializeObject<Product>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Product }, Total = 1 });
        }





    }
}