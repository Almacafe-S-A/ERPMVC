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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class DebitNoteController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public DebitNoteController(ILogger<DebitNoteController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Cuentas por Cobrar.Nota de Debito")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwDebitNote([FromBody]DebitNote _DebitNotep)
        {
            DebitNoteDTO _DebitNote = new DebitNoteDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNote/GetDebitNoteById/" + _DebitNotep.DebitNoteId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNote = JsonConvert.DeserializeObject<DebitNoteDTO>(valorrespuesta);

                }if(_DebitNote == null)
                {
                    _DebitNote = new DebitNoteDTO { 
                        DebitNoteDate = DateTime.Now, 
                        DebitNoteDueDate= null,
                        editar = 1, 
                        BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")) ,
                        Amount = 0,

                    };
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return PartialView(_DebitNote);

        }

        public async Task<ActionResult> Anular([FromBody] DebitNote invoice)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            DebitNote _Invoice = new DebitNote();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/DebitNote/Anular/{invoice.DebitNoteId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);

                }
                else
                {
                    throw new Exception(await (result.Content.ReadAsStringAsync()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest(ex.Message);
            }

            return Json(_Invoice);
        }

        public ActionResult SFReporteND()
        {
            return View();
        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<DebitNote> _DebitNote = new List<DebitNote>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNote/GetDebitNote");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNote = JsonConvert.DeserializeObject<List<DebitNote>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _DebitNote.ToDataSourceResult(request);

        }



        public async Task<ActionResult> GenerarNotaDebito([FromBody] DebitNote debitnote)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            DebitNote debitNote = new DebitNote();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/DebitNote/GenerarNotaDebito/{debitnote.DebitNoteId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    debitNote = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);

                }
                else
                {
                    throw new Exception(await (result.Content.ReadAsStringAsync()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest(ex.Message);
            }

            return Json(debitNote);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<DebitNote>> SaveDebitNote([FromBody]DebitNote _DebitNote)
        {

            try
            {
                DebitNote _listDebitNote = new DebitNote();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNote/GetDebitNoteById/" + _DebitNote.DebitNoteId);
                string valorrespuesta = "";
                _DebitNote.FechaModificacion = DateTime.Now;
                _DebitNote.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listDebitNote = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);
                }
                if(_listDebitNote == null)
                {
                    _listDebitNote = _DebitNote;
                }
                if (_listDebitNote.DebitNoteId == 0)
                {
                    _DebitNote.TipoDocumento = "07";
                    _DebitNote.FechaCreacion = DateTime.Now;
                    _DebitNote.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DebitNote);
                    var value = (insertresult.Result as ObjectResult).Value;

                    DebitNote resultado = ((DebitNote)(value));
                    
                }
                else
                {
                    var updateresult = await Update(_DebitNote.DebitNoteId, _DebitNote);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_DebitNote);
        }

        // POST: DebitNote/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<DebitNote>> Insert(DebitNote _DebitNote)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _DebitNote.UsuarioCreacion = HttpContext.Session.GetString("user");
                _DebitNote.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/DebitNote/Insert", _DebitNote);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNote = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_DebitNote);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNote }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DebitNote>> Update(Int64 id, DebitNote _DebitNote)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/DebitNote/Update", _DebitNote);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNote = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_DebitNote);
            //  return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNote }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DebitNote>> Delete([FromBody]DebitNote _DebitNote)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/DebitNote/Delete", _DebitNote);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNote = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNote }, Total = 1 });
        }

        [HttpGet]
        public async Task<ActionResult> SFDebitNote(Int32 id)
        {
            try
            {
                DebitNoteDTO _debitdto = new DebitNoteDTO { DebitNoteId = id, };
                return await Task.Run(() => View(_debitdto));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionCAI([FromBody]DebitNote debitNote)
        {
            List<NumeracionSAR> _NumeracionSAR = new List<NumeracionSAR>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/NumeracionSAR/GetNumeracion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<List<NumeracionSAR>>(valorrespuesta);
                    _NumeracionSAR = _NumeracionSAR.Where(q => q.BranchId == debitNote.BranchId)
                                                   .Where(q => q.IdPuntoEmision == debitNote.IdPuntoEmision)
                                                   .Where(q => q.Estado == "Activo").ToList();

                    if (_NumeracionSAR.Count == 0)
                    {
                        return BadRequest("No exíste un CAI activo para el punto de emisión");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_NumeracionSAR));
        }
    }
}