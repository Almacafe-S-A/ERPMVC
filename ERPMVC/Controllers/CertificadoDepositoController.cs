using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CertificadoDepositoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CertificadoDepositoController(ILogger<CertificadoDepositoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwCertificadoDeposito([FromBody]CertificadoDepositoDTO _CertificadoDepositoDTO)
        {
            CertificadoDepositoDTO _CertificadoDeposito = new CertificadoDepositoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _CertificadoDepositoDTO.IdCD);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(valorrespuesta);

                }

                if (_CertificadoDeposito == null)
                {
                    _CertificadoDeposito = new CertificadoDepositoDTO {
                        IdCD = 0,
                        FechaCertificado = DateTime.Now,
                        FechaVencimiento = DateTime.Now.AddDays(60),
                        FechaVencimientoDeposito = DateTime.Now.AddDays(30),
                        FechaFirma = DateTime.Now,
                        FechaInicioComputo = DateTime.Now,
                        FechaPagoBanco = DateTime.Now,
                        
                    };
                }
                else
                {
                    _CertificadoDeposito.editar = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CertificadoDeposito);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCertificadoDeposito([DataSourceRequest]DataSourceRequest request)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDeposito");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CertificadoDeposito.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetCertificadoDepositoById([DataSourceRequest]DataSourceRequest request, [FromBody] CertificadoDeposito _Certificado)
        {
            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _Certificado.IdCD);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);

                }

                if (_CertificadoDeposito == null)
                {
                    _CertificadoDeposito = new CertificadoDeposito();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CertificadoDeposito);
        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<List<CertificadoDeposito>>> AgruparCertificados([FromBody]GoodsDeliveryAuthorizationParams _params)
        {
            CertificadoDeposito _GoodsReceived = new CertificadoDeposito();
            if (_params != null)
                if (_params.CertificadosSeleccionados != null)
                {

                    try
                    {
                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/AgruparCertificados", _params.CertificadosSeleccionados);
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _GoodsReceived = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                        throw ex;
                    }
                }
                else
                {

                }

            return Json(_GoodsReceived);
        }




        [HttpPost("[action]")]
             public async Task<ActionResult<CertificadoDeposito>> SaveCertificadoDeposito([FromBody]CertificadoDepositoDTO _CertificadoDeposito)
          //  public async Task<ActionResult<CertificadoDeposito>> SaveCertificadoDeposito([FromBody]dynamic dto)
        {
           // CertificadoDepositoDTO _CertificadoDeposito = new CertificadoDepositoDTO(); 
            try
            {
                // _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(dto.ToString());
                if (_CertificadoDeposito != null)
                {
                    CertificadoDeposito _listCertificadoDeposito = new CertificadoDeposito();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _CertificadoDeposito.IdCD);
                    string valorrespuesta = "";
                    _CertificadoDeposito.FechaModificacion = DateTime.Now;
                    _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listCertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                    }

                    if (_listCertificadoDeposito == null) { _listCertificadoDeposito = new CertificadoDeposito(); }
                    if (_listCertificadoDeposito.IdCD == 0)
                    {
                        _CertificadoDeposito.FechaCreacion = DateTime.Now;
                        _CertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_CertificadoDeposito);
                    }
                    else
                    {
                        var updateresult = await Update(_CertificadoDeposito.IdCD, _CertificadoDeposito);
                    }
                }
                else
                {
                    return BadRequest("No llego correctamente el modelo!");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CertificadoDeposito);
        }

        // POST: CertificadoDeposito/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CertificadoDepositoDTO>> Insert(CertificadoDepositoDTO _CertificadoDeposito)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Insert", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDepositoDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CertificadoDeposito);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CertificadoDeposito>> Update(Int64 id, CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CertificadoDeposito/Update", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CertificadoDeposito>> Delete([FromBody]CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Delete", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }


        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request)
        {
            var res = await GetCertificados();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetCertificados())
                {
                    if (values.Contains(order.IdCD))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<CertificadoDeposito>> GetCertificados()
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDeposito");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);
                    _CertificadoDeposito = (from c in _CertificadoDeposito
                                            select new CertificadoDeposito
                                   {                                       
                                        IdCD = c.IdCD,
                                        CustomerName = "Número de certificado:" + c.NoCD + "|| Nombre:" + c.CustomerName + "|| Fecha:" + c.FechaCertificado + "|| Total:" + c.Total,
                                                CustomerId = c.CustomerId,
                                 }).ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _CertificadoDeposito;
        }




    }
}