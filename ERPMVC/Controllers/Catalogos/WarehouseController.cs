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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class WarehouseController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public WarehouseController(ILogger<WarehouseController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionWarehouseName([FromBody]Warehouse Warehouse)
        {
            List<Warehouse> _Warehouse = new List<Warehouse>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Warehouse/GetWarehouse");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<List<Warehouse>>(valorrespuesta);
                    _Warehouse = _Warehouse.Where(q => q.WarehouseName == Warehouse.WarehouseName && q.BranchId == Warehouse.BranchId ).ToList();
                    if (_Warehouse.Count > 0)
                    {
                        return await Task.Run(() => BadRequest("Ya exíste una Bodega creada con el mismo Nombre en esta Sucursal"));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_Warehouse));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwWarehouse([FromBody]Warehouse _warehouse)
        {
            Warehouse _Warehouse = new Warehouse();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Warehouse/GetWarehouseById/" + _warehouse.WarehouseId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<Warehouse>(valorrespuesta);

                }

                if (_Warehouse == null)
                {
                    _Warehouse = new Warehouse();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView("~/Views/Branch/pvwWarehouse.cshtml",_Warehouse);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request,Warehouse _BranchId)
        {
            List<Warehouse> _Warehouse = new List<Warehouse>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Warehouse/GetWarehouseByBranchId/" + _BranchId.BranchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<List<Warehouse>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Warehouse.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetActivos([DataSourceRequest] DataSourceRequest request, int BranchId)
        {
            List<Warehouse> _Warehouse = new List<Warehouse>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Warehouse/GetWarehouseByBranchId/" + BranchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<List<Warehouse>>(valorrespuesta).Where(w => w.IdEstado==1).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _Warehouse.ToDataSourceResult(request);
        }



        [HttpGet]
        public async Task<DataSourceResult> GetWarehouse([DataSourceRequest]DataSourceRequest request)
        {
            List<Warehouse> _Warehouse = new List<Warehouse>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Warehouse/GetWarehouse");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<List<Warehouse>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Warehouse.ToDataSourceResult(request);

        }




        public async Task<ActionResult<Warehouse>> SaveWarehouse([FromBody]Warehouse _Warehouse)
        {
            List<Warehouse> Warehouse = new List<Warehouse>();
            Warehouse _Warehouses = _Warehouse;


            Warehouse validate = new Warehouse();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));               
                var result = await _client.GetAsync(baseadress + "api/Warehouse/GetWarehouse/");

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());                  
                    Warehouse = JsonConvert.DeserializeObject<List<Warehouse>>(valorrespuesta);
                    Warehouse = Warehouse.Where(x => x.BranchId == _Warehouse.BranchId && x.WarehouseName == _Warehouse.WarehouseName).ToList();

                }
                if (Warehouse.Count == 0) { _Warehouses = new Models.Warehouse(); }

                if (Warehouse.Count > 0)
                {
                                     


                    foreach (var bodegas in Warehouse)
                    {
                        if (bodegas.WarehouseId !=_Warehouse.WarehouseId)
                        {
                            return await Task.Run(() => BadRequest($"Ya éxiste una bodega registrada con este nombre para esta Sucursal."));
                        }
                    }
  
                }

                if (Warehouse.Count == 0)
                {
                    _Warehouse.FechaCreacion = DateTime.Now;
                    _Warehouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Warehouse);
                }
                else
                {
                    _Warehouse.UsuarioCreacion = _Warehouse.UsuarioCreacion;
                    _Warehouse.FechaCreacion = _Warehouse.FechaCreacion;
                    _Warehouse.FechaModificacion = DateTime.Now;
                    _Warehouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_Warehouse.WarehouseId, _Warehouse);                  
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Warehouse);
        }

        // POST: Warehouse/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Warehouse>> Insert(Warehouse _Warehouse)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Warehouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Warehouse.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Warehouse/Insert", _Warehouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<Warehouse>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Warehouse }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Warehouse>> Update(Int64 id, Warehouse _Warehouse)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Warehouse.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Warehouse.FechaModificacion = DateTime.Now;
                var result = await _client.PutAsJsonAsync(baseadress + "api/Warehouse/Update", _Warehouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<Warehouse>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Warehouse);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _Warehouse }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Warehouse>> Delete([FromBody]Warehouse _Warehouse)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Warehouse/Delete", _Warehouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Warehouse = JsonConvert.DeserializeObject<Warehouse>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Warehouse }, Total = 1 });
        }





    }
}