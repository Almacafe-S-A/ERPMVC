﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class ConditionsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        public ConditionsController(IOptions<MyConfig> config)
        {
            this.config = config;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwConditions(Int64 ConditionId=0)
        {
            Conditions _Conditions = new Conditions();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + "api/Conditions/GetConditionsById/" + ConditionId);
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                _Conditions = JsonConvert.DeserializeObject<Conditions>(valorrespuesta);

            }

            if(_Conditions==null)
            {
                _Conditions = new Conditions();
            }


            return PartialView(_Conditions);

        }


        public async Task<ActionResult<Conditions>> SaveConditions([FromBody]Conditions _condition)
        {

            try
            {
                Conditions _listcondi = new Conditions();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Conditions/GetConditionsById/"+ _condition.ConditionId);
                string valorrespuesta = "";            
                _condition.FechaModificacion = DateTime.Now;
                _condition.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                  
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listcondi = JsonConvert.DeserializeObject<Conditions>(valorrespuesta);
                }

                if(_listcondi.ConditionId==0)
                {
                    _condition.FechaCreacion = DateTime.Now;
                    _condition.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_condition);
                }
                else
                {
                    var updateresult = await Update(_condition.ConditionId,_condition);
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(_condition);
        }

        // POST: Customer/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Conditions>> Insert(Conditions _condition)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _condition.UsuarioCreacion = HttpContext.Session.GetString("user");
                _condition.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Conditions/Insert", _condition);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _condition = JsonConvert.DeserializeObject<Conditions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _condition }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Conditions>> Update(Int64 id, Conditions _condition)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Conditions/Update", _condition);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _condition = JsonConvert.DeserializeObject<Conditions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _condition }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Conditions>> Delete([FromBody]Conditions _condition)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Conditions/Delete", _condition);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _condition = JsonConvert.DeserializeObject<Conditions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


            //  return new ObjectResult(new DataSourceResult { Data = new[] { RoleId }, Total = 1 });
            return new ObjectResult(new DataSourceResult { Data = new[] { _condition }, Total = 1 });
        }





    }
}