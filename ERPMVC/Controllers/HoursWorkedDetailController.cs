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
    public class HoursWorkedDetailController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public HoursWorkedDetailController(ILogger<HoursWorkedDetailController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetHoursWorkedDetailByIdHorasTrabajadas([DataSourceRequest]DataSourceRequest request, HoursWorkedDetail _HoursWorkedDetailP)
        {
            List<HoursWorkedDetail> _HoursWorkedDetail = new List<HoursWorkedDetail>();
            try
            {
                if (HttpContext.Session.Get("listadoHoursWorkedDetail") == null
                   || HttpContext.Session.GetString("listadoHoursWorkedDetail") == "")
                {
                    if (_HoursWorkedDetailP.IdHorasTrabajadas > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_HoursWorkedDetailP).ToString();
                        HttpContext.Session.SetString("listadoHoursWorkedDetail", serialzado);  
                    }
                }
                else
                {
                    _HoursWorkedDetail = JsonConvert.DeserializeObject<List<HoursWorkedDetail>>(HttpContext.Session.GetString("listadoHoursWorkedDetail"));
                }


                if (_HoursWorkedDetailP.IdHorasTrabajadas > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/HoursWorkedDetail/GetHoursWorkedDetailByIdHorasTrabajadas/" + _HoursWorkedDetailP.IdHorasTrabajadas);
                    string valorrespuesta = "";
                    
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _HoursWorkedDetail = JsonConvert.DeserializeObject<List<HoursWorkedDetail>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoHoursWorkedDetail", JsonConvert.SerializeObject(_HoursWorkedDetail).ToString());
                    }
                }
                else
                {
                    if (_HoursWorkedDetailP.Multiplicahoras > 0)
                    {
                        _HoursWorkedDetail.Add(_HoursWorkedDetailP);
                        HttpContext.Session.SetString("listadoHoursWorkedDetail", JsonConvert.SerializeObject(_HoursWorkedDetail).ToString());
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _HoursWorkedDetail.ToDataSourceResult(request);

        }

       //POST: EndososCertificadosLine/Insert
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<ActionResult<HoursWorkedDetail>> Insert(HoursWorkedDetail _HoursWorkedDetail)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/HoursWorkedDetail/Insert", _HoursWorkedDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorkedDetail = JsonConvert.DeserializeObject<HoursWorkedDetail>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_HoursWorkedDetail);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HoursWorkedDetail>> Update(Int64 id, HoursWorkedDetail _HoursWorkedDetail)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/HoursWorkedDetail/Update", _HoursWorkedDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorkedDetail = JsonConvert.DeserializeObject<HoursWorkedDetail>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _HoursWorkedDetail }, Total = 1 });
        }

        [HttpPost("PostHoursWorkedDetail")]
        public async Task<DataSourceResult> PostHoursWorkedDetail([DataSourceRequest]DataSourceRequest request, HoursWorkedDetail _HoursWorkedDetailP)
        //  public async Task<DataSourceResult> PostHoursWorkedDetail([DataSourceRequest]DataSourceRequest request,[FromBody]dynamic dto)
        {
            List<HoursWorkedDetail> _HoursWorkedDetail = new List<HoursWorkedDetail>();
            //HoursWorkedDetail _HoursWorkedDetailP = new HoursWorkedDetail();
            if (_HoursWorkedDetailP.IdHorasTrabajadas == 0 || _HoursWorkedDetailP.IdHorasTrabajadas == null)
            {
                
                try
                {
                    //  _HoursWorkedDetailP = JsonConvert.DeserializeObject<HoursWorkedDetail>(dto);
                    if (HttpContext.Session.Get("listadoHoursWorkedDetail") == null
                       || HttpContext.Session.GetString("listadoHoursWorkedDetail") == "")
                    {
                        if (_HoursWorkedDetailP.IdHorasTrabajadas > 0)
                        {
                            string serialzado = JsonConvert.SerializeObject(_HoursWorkedDetailP).ToString();
                            HttpContext.Session.SetString("listadoHoursWorkedDetail", serialzado);
                        }
                    }
                    else
                    {
                        _HoursWorkedDetail = JsonConvert.DeserializeObject<List<HoursWorkedDetail>>(HttpContext.Session.GetString("listadoHoursWorkedDetail"));
                    }


                    if (_HoursWorkedDetailP.IdHorasTrabajadas > 0)
                    {

                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();

                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.GetAsync(baseadress + "api/HoursWorkedDetail/GetHoursWorkedDetailByIdHorasTrabajadas/" + _HoursWorkedDetailP.IdHorasTrabajadas);
                        string valorrespuesta = "";

                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _HoursWorkedDetail = JsonConvert.DeserializeObject<List<HoursWorkedDetail>>(valorrespuesta);
                            HttpContext.Session.SetString("listadoHoursWorkedDetail", JsonConvert.SerializeObject(_HoursWorkedDetail).ToString());
                        }
                    }
                    else
                    {
                        if (_HoursWorkedDetailP.Multiplicahoras > 0)
                        {
                            _HoursWorkedDetailP.UsuarioCreacion = HttpContext.Session.GetString("user");
                            _HoursWorkedDetailP.FechaCreacion = DateTime.Now;
                            _HoursWorkedDetail.Add(_HoursWorkedDetailP);
                            HttpContext.Session.SetString("listadoHoursWorkedDetail", JsonConvert.SerializeObject(_HoursWorkedDetail).ToString());
                            HttpContext.Session.SetString("listadoHoursWorkedDetail", "");
                        }
                    }



                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

                return _HoursWorkedDetail.ToDataSourceResult(request);
            }
            else
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    _HoursWorkedDetailP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _HoursWorkedDetailP.FechaCreacion = DateTime.Now;
                    var result = await _client.PostAsJsonAsync(baseadress + "api/HoursWorkedDetail/Insert", _HoursWorkedDetailP);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _HoursWorkedDetailP = JsonConvert.DeserializeObject<HoursWorkedDetail>(valorrespuesta);
                        _HoursWorkedDetail.Add(_HoursWorkedDetailP);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

                return _HoursWorkedDetail.ToDataSourceResult(request);
            }
        }

        [HttpPut("PutHoursWorkedDetail")]
        public async Task<IActionResult> PutHoursWorkedDetail(Int64 Id, HoursWorkedDetail _HoursWorkedDetail)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _HoursWorkedDetail.FechaModificacion = DateTime.Now;
                _HoursWorkedDetail.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/HoursWorkedDetail/Update", _HoursWorkedDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorkedDetail = JsonConvert.DeserializeObject<HoursWorkedDetail>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _HoursWorkedDetail }, Total = 1 });
        }

        [HttpDelete("DeleteHoursWorkedDetail")]
        public async Task<ActionResult<HoursWorkedDetail>> DeleteHoursWorkedDetail(Int64 Id, HoursWorkedDetail _HoursWorkedDetail)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/HoursWorkedDetail/Delete", _HoursWorkedDetail);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _HoursWorkedDetail = JsonConvert.DeserializeObject<HoursWorkedDetail>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _HoursWorkedDetail }, Total = 1 });
        }
    }
}