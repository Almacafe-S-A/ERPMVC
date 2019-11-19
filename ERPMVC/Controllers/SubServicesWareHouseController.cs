﻿using System;
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
        public SubServicesWareHouseController(ILogger<SubServicesWareHouseController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddSubServicesWareHouse([FromBody]SubServicesWareHouse _SubServicesWareHousep)
        {
            SubServicesWareHouse _SubServicesWareHouse = new SubServicesWareHouse();
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
                    _SubServicesWareHouse = JsonConvert.DeserializeObject<SubServicesWareHouse>(valorrespuesta);
                }

                if (_SubServicesWareHouse == null)
                {
                    _SubServicesWareHouse = new SubServicesWareHouse { SubServicesWareHouseId = 0, StartTime = DateTime.Now, EndTime = DateTime.Now , BranchId = _SubServicesWareHousep.BranchId };
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





    }
}