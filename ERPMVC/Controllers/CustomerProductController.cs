using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class CustomerProductController : Controller
    {
        //Variables y opciones
        private readonly IOptions<MyConfig> _config;
        private readonly IMapper mapper;
        private readonly ILogger _logger;

        public CustomerProductController(ILogger<CustomerProductController> logger, IOptions<MyConfig> config, IMapper mapper)
        {
            this._config = config;
            this.mapper = mapper;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        //Vista de Edición/Ingreso
        [HttpPost("[action]")]
        public async Task<ActionResult> Productos([FromBody]CustomerProduct _cliente)
        {
            CustomerProduct _CustomerProduct = new CustomerProduct();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerProduct/GetCustomerProductById/" + _cliente.CustomerProductId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerProduct = JsonConvert.DeserializeObject<CustomerProduct>(valorrespuesta);
                    //
                    //Obtener los estados. (Activo/Inactivo)
                    var result2 = await _client.GetAsync(baseadress + "api/Estados/GetEstadosByGrupo/" + 1);
                    if (result2.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result2.Content.ReadAsStringAsync());
                        var estados = JsonConvert.DeserializeObject<List<Estados>>(valorrespuesta);
                        if (_CustomerProduct == null)
                        {
                            ViewData["estados"] = new SelectList(estados, "IdEstado", "NombreEstado");
                            _CustomerProduct = new CustomerProduct();
                        }
                        else
                        {
                            ViewData["estados"] = new SelectList(estados, "IdEstado", "NombreEstado", _CustomerProduct.IdEstado);
                            //ViewData["estadoUnidad"] = _UnitOfMeasure.IdEstado;
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return PartialView(_CustomerProduct);

        }


        /// <summary>
        /// Listado de Productos por Cliente
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCustomerProduct([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerProduct> _CustomerProduct = new List<CustomerProduct>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerProduct/GetCustomerProduct");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerProduct = JsonConvert.DeserializeObject<List<CustomerProduct>>(valorrespuesta);

                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerProduct.ToDataSourceResult(request);
        }
        
        // POST: Guardar
        [HttpPost]
        public async Task<ActionResult<CustomerProduct>> Insert([FromBody]CustomerProduct _CustomerProduct)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerProduct.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerProduct.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerProduct/Insert", _CustomerProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerProduct = JsonConvert.DeserializeObject<CustomerProduct>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_CustomerProduct);
        }

        /// <summary>
        /// Actualizar relación.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_CustomerProduct"></param>
        /// <returns></returns>
        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<CustomerProduct>> Update(Int64 id, [FromBody]CustomerProduct _CustomerProduct)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerProduct/Update", _CustomerProduct);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerProduct = JsonConvert.DeserializeObject<CustomerProduct>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_CustomerProduct);
        }
        

    }
}
