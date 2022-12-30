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
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class SubServicesWareHouseController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public SubServicesWareHouseController(ILogger<SubServicesWareHouseController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Servicios Utilizados")]
        public async Task<IActionResult> Index()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(() => View());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddSubServicesWareHouse([FromBody]SubServicesWareHouse _SubServicesWareHousep)
        {
            SubServicesWareHouseDTO _SubServicesWareHouse = new SubServicesWareHouseDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubServicesWareHouse/GetSubServicesWareHouseById/" + _SubServicesWareHousep.SubServicesWareHouseId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubServicesWareHouse = JsonConvert.DeserializeObject<SubServicesWareHouseDTO>(valorrespuesta);
                }

                if (_SubServicesWareHouse == null || _SubServicesWareHousep.SubServicesWareHouseId==0)
                {
                    _SubServicesWareHouse = new SubServicesWareHouseDTO { 
                        SubServicesWareHouseId = 0, 
                        StartTime = DateTime.Now, 
                        EndTime = new DateTime() , 
                        BranchId = _SubServicesWareHousep.BranchId , 
                        DocumentDate = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => PartialView(_SubServicesWareHouse));

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<SubServicesWareHouse> _SubServicesWareHouse = new List<SubServicesWareHouse>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SubServicesWareHouse/GetSubServicesWareHouse");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubServicesWareHouse = JsonConvert.DeserializeObject<List<SubServicesWareHouse>>(valorrespuesta);
                    _SubServicesWareHouse = _SubServicesWareHouse.OrderByDescending(e => e.SubServicesWareHouseId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => _SubServicesWareHouse.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubServicesWareHouse>> SaveSubServicesWareHouse([FromBody]SubServicesWareHouse _SubServicesWareHouse)
        {

            try
            {
                SubServicesWareHouse _listSubServicesWareHouse = new SubServicesWareHouse();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                TimeSpan t = _SubServicesWareHouse.EndTime.Subtract(_SubServicesWareHouse.StartTime);
                _SubServicesWareHouse.QuantityHours = t.TotalHours;
                var result = await _client.GetAsync(baseadress + "api/SubServicesWareHouse/GetSubServicesWareHouseById/" + _SubServicesWareHouse.SubServicesWareHouseId);
                string valorrespuesta = "";
                _SubServicesWareHouse.FechaModificacion = DateTime.Now;
                _SubServicesWareHouse.UsuarioModificacion = HttpContext.Session.GetString("user");
            
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listSubServicesWareHouse = JsonConvert.DeserializeObject<SubServicesWareHouse>(valorrespuesta);
                }

                if (_listSubServicesWareHouse == null) { _listSubServicesWareHouse = new SubServicesWareHouse(); }

                if (_listSubServicesWareHouse.SubServicesWareHouseId == 0)
                {
                    _SubServicesWareHouse.FechaCreacion = DateTime.Now;
                    _SubServicesWareHouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_SubServicesWareHouse);
                }
                else
                {
                    var updateresult = await Update(_SubServicesWareHouse.SubServicesWareHouseId, _SubServicesWareHouse);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_SubServicesWareHouse));
        }

        // POST: SubServicesWareHouse/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SubServicesWareHouse>> Insert(SubServicesWareHouse _SubServicesWareHouse)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _SubServicesWareHouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                _SubServicesWareHouse.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/SubServicesWareHouse/Insert", _SubServicesWareHouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubServicesWareHouse = JsonConvert.DeserializeObject<SubServicesWareHouse>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_SubServicesWareHouse));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubServicesWareHouse>> Update(Int64 id, SubServicesWareHouse _SubServicesWareHouse)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/SubServicesWareHouse/Update", _SubServicesWareHouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubServicesWareHouse = JsonConvert.DeserializeObject<SubServicesWareHouse>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_SubServicesWareHouse));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubServicesWareHouse>> Delete([FromBody]SubServicesWareHouse _SubServicesWareHouse)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/SubServicesWareHouse/Delete", _SubServicesWareHouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SubServicesWareHouse = JsonConvert.DeserializeObject<SubServicesWareHouse>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error: {ex.Message}"));
            }



            return await Task.Run(() => Ok(_SubServicesWareHouse));
        }


        [HttpGet]
        public async Task<ActionResult> SFServiciosBodega(Int32 id)
        {
            try
            {
                SubServicesWareHouse subServicesWareHouse = new SubServicesWareHouse { SubServicesWareHouseId = id, };
                
                return await Task.Run(() => View(subServicesWareHouse));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }
        }





    }
}