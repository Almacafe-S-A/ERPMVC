using System;
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
using NPOI.OpenXmlFormats.Dml.Chart;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class BankAccountTransfersController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public BankAccountTransfersController(ILogger<BankAccountTransfersController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Bancos.Transferencias")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwBankAccountTransfers([FromBody] BankAccountTransfers _sarpara)
         {
            BankAccountTransfers _BankAccountTransfers = new BankAccountTransfers();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BankAccountTransfers/GetBankAccountTransfersById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BankAccountTransfers = JsonConvert.DeserializeObject<BankAccountTransfers>(valorrespuesta);

                }

                if (_BankAccountTransfers == null || _BankAccountTransfers.Id == 0)
                {
                    _BankAccountTransfers = new BankAccountTransfers();
                    _BankAccountTransfers.DestinationAmount = 0;
                    _BankAccountTransfers.SourceAmount = 0;
                    _BankAccountTransfers.Rate = 0;
                    _BankAccountTransfers.FechaCreacion = DateTime.Now;
                    _BankAccountTransfers.TransactionDate = DateTime.Now;
                    _BankAccountTransfers.EstadoId = 5;
                    _BankAccountTransfers.DestinationAccountManagement = new AccountManagement();
                    _BankAccountTransfers.SourceAccountManagement = new AccountManagement();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //throw ex;
                return BadRequest("Error al cargar el formulario");

            }



            return PartialView(_BankAccountTransfers);

        }


        public async Task<ActionResult> Aprobar([FromBody] BankAccountTransfers _transfer, bool aprobacion)
        {

            if (_transfer.EstadoId == 6)
            {
                aprobacion = true;
            }
            else
            {
                aprobacion = false;
            }
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/BankAccountTransfers/Aprobar/{_transfer.Id}/{aprobacion}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _transfer = JsonConvert.DeserializeObject<BankAccountTransfers>(valorrespuesta);
                }
                else
                {
                    return BadRequest($"Error este cheque ya habia sido Aprobado o Rechazado.");
                }
            }


            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: { ex.ToString() }");
            }



            return Ok(aprobacion);

        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetBankAccountTransfers([DataSourceRequest] DataSourceRequest request)
        {
            List<BankAccountTransfers> _BankAccountTransfers = new List<BankAccountTransfers>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BankAccountTransfers/Get");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BankAccountTransfers = JsonConvert.DeserializeObject<List<BankAccountTransfers>>(valorrespuesta);
                    _BankAccountTransfers = _BankAccountTransfers.OrderByDescending(e => e.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _BankAccountTransfers.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetJson([DataSourceRequest] DataSourceRequest request)
        {
            List<BankAccountTransfers> _BankAccountTransfers = new List<BankAccountTransfers>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BankAccountTransfers/GetBankAccountTransfers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BankAccountTransfers = JsonConvert.DeserializeObject<List<BankAccountTransfers>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_BankAccountTransfers.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<BankAccountTransfers>> SaveBankAccountTransfers([FromBody] BankAccountTransfers _BankAccountTransfersS)
        {
            BankAccountTransfers _BankAccountTransfers = _BankAccountTransfersS;
            try
            {
                BankAccountTransfers _listBankAccountTransfers = new BankAccountTransfers();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                if (_BankAccountTransfers.Id != 0)
                {
                    var result = await _client.GetAsync(baseadress + "api/BankAccountTransfers/GetBankAccountTransfersById/" + _BankAccountTransfers.Id);
                    string valorrespuesta = "";
                    _BankAccountTransfers.FechaModificacion = DateTime.Now;
                    _BankAccountTransfers.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _BankAccountTransfers = JsonConvert.DeserializeObject<BankAccountTransfers>(valorrespuesta);
                    }

                    if (_BankAccountTransfers == null) { _BankAccountTransfers = new Models.BankAccountTransfers(); }

                    if (_BankAccountTransfers.Id > 0)
                    {
                        
                        var updateresult = await Update(_BankAccountTransfersS);
                        if (updateresult.Result is BadRequestObjectResult)
                        {
                            return BadRequest(updateresult.Result.ToString());
                        }
                    }
                }else 
                {
                    _BankAccountTransfersS.FechaCreacion = DateTime.Now;
                    _BankAccountTransfersS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BankAccountTransfersS);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(insertresult.Result.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }

            return Json(_BankAccountTransfersS);
        }





        // POST: BankAccountTransfers/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<BankAccountTransfers>> Insert(BankAccountTransfers _BankAccountTransfers)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _BankAccountTransfers.UsuarioCreacion = HttpContext.Session.GetString("user");
                _BankAccountTransfers.UsuarioModificacion = HttpContext.Session.GetString("user");
                _BankAccountTransfers.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/BankAccountTransfers/Insert", _BankAccountTransfers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BankAccountTransfers = JsonConvert.DeserializeObject<BankAccountTransfers>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    return BadRequest($"Ocurrio un error: {error}");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_BankAccountTransfers);
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BankAccountTransfers>> Update(BankAccountTransfers _BankAccountTransfers)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/BankAccountTransfers/Update", _BankAccountTransfers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BankAccountTransfers = JsonConvert.DeserializeObject<BankAccountTransfers>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    return BadRequest($"Ocurrio un error: {error}");

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _BankAccountTransfers }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<BankAccountTransfers>> Delete(Int64 Id, BankAccountTransfers _BankAccountTransfers)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/BankAccountTransfers/Delete", _BankAccountTransfers);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BankAccountTransfers = JsonConvert.DeserializeObject<BankAccountTransfers>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _BankAccountTransfers }, Total = 1 });
        }





    }
}