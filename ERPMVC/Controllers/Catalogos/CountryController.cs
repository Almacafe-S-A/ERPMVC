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
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CountryController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public CountryController(ILogger<CountryController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        // GET: Customer
        [Authorize(Policy = "Configuracion.Pais")]
        public ActionResult Country()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Configuracion.Pais.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Configuracion.Pais.Editar", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Configuracion.Pais.Eliminar", "true");
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request,bool GAFI)
        {
            List<Country> _country = new List<Country>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _country = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta);
                    _country = _country.Where(q => q.GAFI == GAFI).OrderBy(q=>q.Name).ToList();
                    _country = _country.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_country.ToDataSourceResult(request));

        }


        public async Task<JsonResult> GetActivos([DataSourceRequest] DataSourceRequest request, bool GAFI)
        {
            List<Country> _country = new List<Country>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _country = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta).Where(w => w.IdEstado ==1).ToList();
                    _country = _country.Where(q => q.GAFI == GAFI).OrderBy(q => q.Name).ToList();
                    _country = _country.OrderByDescending(q => q.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_country.ToDataSourceResult(request));
        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<Country> _Country = new List<Country>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Country);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCountry([FromBody]CountryDTO _sarpara)
        {
            CountryDTO _Country = new CountryDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountryById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<CountryDTO>(valorrespuesta);

                }

                if (_Country == null)
                {
                    _Country = new CountryDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Country);

        }


        [HttpPost]
        public async Task<ActionResult<Country>> SaveCountry([FromBody]CountryDTO _CountryP)
       // public async Task<ActionResult<Country>> SaveCountry([FromBody]dynamic dto)
        {
            //CountryDTO _CountryP = new CountryDTO();
            Country _Country = _CountryP;
            try
            {
                // _CountryP = JsonConvert.DeserializeObject<CountryDTO>(dto.ToString());
                Country _Paises = new Country();   

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountryById/" + _Country.Id);
                string valorrespuesta = "";
                _Country.FechaModificacion = DateTime.Now;
                _Country.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Paises = JsonConvert.DeserializeObject<CountryDTO>(valorrespuesta);
                }

                if (_Paises == null) { _Paises = new Models.Country(); }


                if (_Paises.Id == 0)
                {
                    _Country.FechaCreacion = DateTime.Now;                    
                    _Country.Usuariocreacion = HttpContext.Session.GetString("user");
                    Country _CountryDuplicate = new Models.Country();
                    //string baseadress = config.Value.urlbase;
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado = await _client.GetAsync(baseadress + "api/Country/GetCountryByName/" + _Country.Name + "/" + _Country.GAFI);
                    string valorrespuesta2 = "";
                    if (resultado.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                        _CountryDuplicate = JsonConvert.DeserializeObject<Country>(valorrespuesta2);

                    }

                    if (_CountryDuplicate != null)
                    {

                        string error = await result.Content.ReadAsStringAsync();
                        if (_CountryDuplicate.GAFI && _Country.GAFI)
                        {
                            return await Task.Run(() => BadRequest($"El nombre del país ya esta registrado... como país GAFI ."));
                        }
                        else if (!_CountryDuplicate.GAFI && !_Country.GAFI)
                        {
                            return await Task.Run(() => BadRequest($"El nombre del país ya esta registrado."));
                        }

                    }

                    var insertresult = await Insert(_CountryP);
                }
                else
                {
                    Country _CountryDuplicated = new Country();
                    //string baseadress = config.Value.urlbase;
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado = await _client.GetAsync(baseadress + "api/Country/GetCountryByName/" + _Country.Name + "/" + _Country.GAFI);
                    string valorrespuesta2 = "";

                    if (resultado.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                        _CountryDuplicated = JsonConvert.DeserializeObject<Country>(valorrespuesta2);

                    }
                    if (_CountryDuplicated != null)
                    {
                        if (_CountryDuplicated.Id != _Country.Id)

                        {
                            string error = await result.Content.ReadAsStringAsync();

                            if (_CountryDuplicated.GAFI && _Country.GAFI)
                            {
                                return await Task.Run(() => BadRequest($"El nombre del país ya esta registrado... como país GAFI ."));
                            }
                            else if (!_CountryDuplicated.GAFI && !_Country.GAFI)
                            {
                                return await Task.Run(() => BadRequest($"El nombre del país ya esta registrado."));
                            }
                        }

                    }
                    _CountryP.Usuariocreacion = _Paises.Usuariocreacion;
                    _CountryP.FechaCreacion = _Paises.FechaCreacion;
                    var updateresult = await Update(_Country.Id, _CountryP);
                }



                //if (_CountryP.SortName == null) { _CountryP.SortName = _CountryP.Name; }
                //if (_CountryP.Id == 0)
                //{
                //    _Country.FechaCreacion = DateTime.Now;
                //    _Country.Usuariocreacion = HttpContext.Session.GetString("user");
                //    var insertresult = await Insert(_CountryP);
                //    var value = (insertresult.Result as ObjectResult).Value;
                //    try
                //    {                       
                //        Country resultado = ((Country)(value));
                //        if (resultado.Id <= 0)
                //        {
                //            return await Task.Run(() => BadRequest($"Ocurrio un error"));
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //        _logger.LogError($"Ocurrio un error desde metodo que va al api: {value.ToString()}");
                //        return await Task.Run(() => BadRequest($"Ocurrio un error{value.ToString()}"));
                //    }
                  
                //}
                //else
                //{
                //    _CountryP.Usuariocreacion = _Country.Usuariocreacion;
                //    _CountryP.FechaCreacion = _Country.FechaCreacion;
                //    var updateresult = await Update(_Country.Id, _CountryP);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Country);
        }


        //--------------------------------------------------------------------------------------
        // POST: Country/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Country>> Insert(Country _CountryP)
        {
            Country _Country = _CountryP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Country.Usuariocreacion = HttpContext.Session.GetString("user");
                _Country.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Country.FechaCreacion = DateTime.Now;
                _Country.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Country/Insert", _Country);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<Country>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_Country);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _Country }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, Country _CountryP)
        {
            Country _Country = _CountryP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Country.FechaModificacion = DateTime.Now;
                _Country.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Country/Update", _Country);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<Country>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Country }, Total = 1 });
        }

        
        
        [HttpPost]
        public async Task<ActionResult<Country>> Delete(Int64 Id, [FromBody]Country _CountryP)
        {
            Country _Country = _CountryP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Country/Delete", _Country);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<Country>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    if (d == "No se puede eliminar este registro porque está siendo utilizado.")
                    {
                        return await Task.Run(() => BadRequest("No se puede eliminar este registro porque está siendo utilizado."));
                    }
                    //throw  new Exception(d);
                    return await Task.Run(() => BadRequest($"{d}"));
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Country }, Total = 1 });
        }



        [HttpPost("[action]")]
        public async Task<ActionResult> PaisesGAFIByName([FromBody]Country country)
        {
            List<Country> _ListPaisesGafi = new List<Country>();
            Country _PaisGafi = new Country();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ListPaisesGafi = JsonConvert.DeserializeObject<List<Country>>(valorrespuesta);
                    _ListPaisesGafi = _ListPaisesGafi.Where(p => p.GAFI == true).ToList();
                }
                if(_ListPaisesGafi.Count > 0)
                {
                    _PaisGafi = _ListPaisesGafi.Where(p => p.Name == country.Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_PaisGafi));
        }
    }
}