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

        //[HttpPost("[controller]/[action]")]
        //public async Task<ActionResult> pvwEndososDetailMant([FromBody]EndososCertificadosLine _EndososCertificadosLinep)
        //{
        //    EndososCertificadosLine _EndososCertificadosLine = new EndososCertificadosLine();
        //    try
        //    {
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/EndososCertificadosLine/GetEndososCertificadosLineById/" + _EndososCertificadosLinep.EndososCertificadosLineId);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _EndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);

        //        }

        //        if (_EndososCertificadosLine == null)
        //        {
        //            _EndososCertificadosLine = new EndososCertificadosLine();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }



        //    return PartialView("~/Views/EndososCertificados/pvwEndososDetailMant.cshtml", _EndososCertificadosLine);

        //}


        //[HttpGet]
        //public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        //{
        //    List<EndososCertificadosLine> _EndososCertificadosLine = new List<EndososCertificadosLine>();
        //    try
        //    {

        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/EndososCertificadosLine/GetEndososCertificadosLine");
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _EndososCertificadosLine = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(valorrespuesta);

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }


        //    return _EndososCertificadosLine.ToDataSourceResult(request);

        //}


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


        //[HttpPost("[action]")]
        //public async Task<ActionResult<HoursWorkedDetail>> SaveHoursWorkedDetail([FromBody]HoursWorkedDetail _HoursWorkedDetail)
        //{

        //    try
        //    {
        //        HoursWorkedDetail _listHoursWorkedDetail = new HoursWorkedDetail();
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/HoursWorkedDetail/GetHoursWorkedDetailById/" + _HoursWorkedDetail.IdDetallehorastrabajadas);
        //        string valorrespuesta = "";
        //        // _EndososCertificadosLine.FechaModificacion = DateTime.Now;
        //        // _EndososCertificadosLine.UsuarioModificacion = HttpContext.Session.GetString("user");
        //        if (result.IsSuccessStatusCode)
        //        {

        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _listHoursWorkedDetail = JsonConvert.DeserializeObject<HoursWorkedDetail>(valorrespuesta);
        //        }

        //        if (_listHoursWorkedDetail.IdDetallehorastrabajadas == 0)
        //        {
        //            //    _EndososCertificadosLine.FechaCreacion = DateTime.Now;
        //            // _EndososCertificadosLine.UsuarioCreacion = HttpContext.Session.GetString("user");
        //            var insertresult = await Insert(_HoursWorkedDetail);
        //        }
        //        else
        //        {
        //            var updateresult = await Update(_HoursWorkedDetail.IdDetallehorastrabajadas, _HoursWorkedDetail);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    return Json(_HoursWorkedDetail);
        //}

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
                // _EndososCertificadosLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_EndososCertificadosLine.UsuarioModificacion = HttpContext.Session.GetString("user");
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
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificadosLine }, Total = 1 });
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

        //[HttpPost("[action]")]
        //public async Task<ActionResult<EndososCertificadosLine>> Delete([FromBody]EndososCertificadosLine _EndososCertificadosLine)
        //{
        //    try
        //    {
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

        //        var result = await _client.PostAsJsonAsync(baseadress + "api/EndososCertificadosLine/Delete", _EndososCertificadosLine);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _EndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        return BadRequest($"Ocurrio un error: {ex.Message}");
        //    }



        //    return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificadosLine }, Total = 1 });
        //}



        [HttpPost("PostHoursWorkedDetail")]
        public async Task<DataSourceResult> PostHoursWorkedDetail([DataSourceRequest]DataSourceRequest request, HoursWorkedDetail _HoursWorkedDetailP)
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

        //[HttpPost("Post")]
        //public async Task<ActionResult<HoursWorkedDetail>> Post(HoursWorkedDetail _HoursWorkedDetailA)
        //{
        //    List<HoursWorkedDetail> _HoursWorkedDetail = new List<HoursWorkedDetail>();
        //    try
        //    {
        //        if (HttpContext.Session.Get("listadoHoursWorkedDetail") == null
        //           || HttpContext.Session.GetString("listadoHoursWorkedDetail") == "")
        //        {
        //            if (_HoursWorkedDetailA.IdHorasTrabajadas > 0)
        //            {
        //                string serialzado = JsonConvert.SerializeObject(_HoursWorkedDetailA).ToString();
        //                HttpContext.Session.SetString("listadoHoursWorkedDetail", serialzado);
        //            }
        //        }
        //        else
        //        {
        //            _HoursWorkedDetail = JsonConvert.DeserializeObject<List<HoursWorkedDetail>>(HttpContext.Session.GetString("listadoHoursWorkedDetail"));
        //        }


        //        if (_HoursWorkedDetailA.IdHorasTrabajadas > 0)
        //        {

        //            string baseadress = config.Value.urlbase;
        //            HttpClient _client = new HttpClient();

        //            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //            var result = await _client.GetAsync(baseadress + "api/HoursWorkedDetail/GetHoursWorkedDetailByIdHorasTrabajadas/" + _HoursWorkedDetailA.IdHorasTrabajadas);
        //            string valorrespuesta = "";

        //            if (result.IsSuccessStatusCode)
        //            {
        //                valorrespuesta = await (result.Content.ReadAsStringAsync());
        //                _HoursWorkedDetail = JsonConvert.DeserializeObject<List<HoursWorkedDetail>>(valorrespuesta);
        //                HttpContext.Session.SetString("listadoHoursWorkedDetail", JsonConvert.SerializeObject(_HoursWorkedDetail).ToString());
        //            }
        //        }
        //        else
        //        {
        //            if (_HoursWorkedDetailA.Multiplicahoras > 0)
        //            {
        //                _HoursWorkedDetail.Add(_HoursWorkedDetailA);
        //                HttpContext.Session.SetString("listadoHoursWorkedDetail", JsonConvert.SerializeObject(_HoursWorkedDetail).ToString());
        //                HttpContext.Session.SetString("listadoHoursWorkedDetail", "");
        //            }
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }

        //    //return await Task.Run(() => Ok(_customer));
        //    return await Task.Run(() => new ObjectResult(new DataSourceResult { Data = new[] { _HoursWorkedDetail }, Total = 1 }));
        //}

        [HttpPut("PutHoursWorkedDetail")]
        public async Task<IActionResult> PutHoursWorkedDetail(Int64 Id, HoursWorkedDetail _HoursWorkedDetail)
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
    }
}