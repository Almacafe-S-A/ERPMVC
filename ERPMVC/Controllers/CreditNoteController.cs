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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class CreditNoteController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CreditNoteController(ILogger<CreditNoteController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwCreditNote([FromBody]CreditNote _CreditNotep)
        {
            CreditNoteDTO _CreditNote = new CreditNoteDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNote/GetCreditNoteById/" + _CreditNotep.CreditNoteId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNote = JsonConvert.DeserializeObject<CreditNoteDTO>(valorrespuesta);

                }
                if (_CreditNote == null)
                {
                    _CreditNote = new CreditNoteDTO { OrderDate = DateTime.Now, DeliveryDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(30), editar = 1, BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")) };
                }
                else
                {
                    _CreditNote.NumeroDEIString = $"{_CreditNote.Sucursal}-{_CreditNote.Caja}-{_CreditNote.TipoDocumento}-{_CreditNote.NúmeroDEI.ToString().PadLeft(8, '0')} ";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CreditNote);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CreditNote> _CreditNote = new List<CreditNote>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNote/GetCreditNote");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNote = JsonConvert.DeserializeObject<List<CreditNote>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CreditNote.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CreditNote>> SaveCreditNote([FromBody]CreditNote _CreditNote)
        {

            try
            {
                CreditNote _listCreditNote = new CreditNote();
                List<CreditNote> _listCreditNoteRTN = new List<CreditNote>();
                List<CreditNote> _listCreditNoteValidation = new List<CreditNote>();
                List<CreditNote> _listCreditNoteNombre = new List<CreditNote>();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNote/GetCreditNoteById/" + _CreditNote.CreditNoteId);
                string valorrespuesta = "";
                _CreditNote.FechaModificacion = DateTime.Now;
                _CreditNote.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCreditNote = JsonConvert.DeserializeObject<CreditNote>(valorrespuesta);
                }

                if (_listCreditNote == null) {
                    _listCreditNote = new CreditNote();
                    
                    var resultRTN = await _client.GetAsync(baseadress + "api/CreditNote/GetCreditNote");
                    string valorrespuestaRTN = "";
                    if (resultRTN.IsSuccessStatusCode)
                    {
                        valorrespuestaRTN = await (resultRTN.Content.ReadAsStringAsync());
                        _listCreditNoteValidation = JsonConvert.DeserializeObject<List<CreditNote>>(valorrespuestaRTN);
                        _listCreditNoteRTN = _listCreditNoteValidation.Where(q => q.RTN == _CreditNote.RTN).ToList();
                        _listCreditNoteNombre = _listCreditNoteValidation.Where(q => q.CreditNoteName == _CreditNote.CreditNoteName).ToList();
                        if(_listCreditNoteRTN.Count > 0 && _listCreditNoteNombre.Count > 0)
                        {
                            return await Task.Run(() => BadRequest("Ya exísten Notas de Crédito creadas con el mismo Nombre y RTN"));
                        }
                        else if (_listCreditNoteRTN.Count > 0)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Nota de Crédito creada con el mismo RTN"));
                        }
                        else if (_listCreditNoteNombre.Count > 0)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Nota de Crédito creada con el mismo Nombre"));
                        }
                    }
                }

                if (_listCreditNote.CreditNoteId == 0)
                {
                    _CreditNote.TipoDocumento = "05";
                    _CreditNote.DeliveryDate = DateTime.Now;
                    _CreditNote.FechaCreacion = DateTime.Now;
                    _CreditNote.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CreditNote);
                    var value = (insertresult.Result as ObjectResult).Value;
                    CreditNote resultado = ((CreditNote)(value));
                    if (resultado.Fiscal is false)
                    {
                        if (resultado.CAI == null || resultado.CAI == "")
                        {
                            return await Task.Run(() => BadRequest("No existe un CAI activo para el punto de emisión"));
                        }
                    }
                }
                else
                {
                    var updateresult = await Update(_CreditNote.CreditNoteId, _CreditNote);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CreditNote);
        }

        // POST: CreditNote/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CreditNote>> Insert(CreditNote _CreditNote)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CreditNote.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CreditNote.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CreditNote/Insert", _CreditNote);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNote = JsonConvert.DeserializeObject<CreditNote>(valorrespuesta);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                  return await Task.Run(()=>BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_CreditNote);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CreditNote }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CreditNote>> Update(Int64 id, CreditNote _CreditNote)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CreditNote/Update", _CreditNote);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNote = JsonConvert.DeserializeObject<CreditNote>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                  return await Task.Run(()=>BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_CreditNote);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CreditNote }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CreditNote>> Delete([FromBody]CreditNote _CreditNote)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CreditNote/Delete", _CreditNote);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNote = JsonConvert.DeserializeObject<CreditNote>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CreditNote }, Total = 1 });
        }

        [HttpGet]
        public async Task<ActionResult> SFCreditNoteFiscal(Int32 id)
        {
            try
            {
                CreditNoteDTO _creditnotedto = new CreditNoteDTO { CreditNoteId = id, };
                return await Task.Run(() => View(_creditnotedto));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }
        [HttpGet]
        public async Task<ActionResult> SFCreditNoteInterna(Int32 id)
        {
            try
            {
                CreditNoteDTO _creditnotedto = new CreditNoteDTO { CreditNoteId = id, };
                return await Task.Run(() => View(_creditnotedto));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionRTN([FromBody]CreditNote creditNote)
        {
            List<CreditNote> _creditNote = new List<CreditNote>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNote/GetCreditNote");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _creditNote = JsonConvert.DeserializeObject<List<CreditNote>>(valorrespuesta);
                    _creditNote = _creditNote.Where(q => q.RTN == creditNote.RTN).ToList();
                    if (_creditNote.Count > 0)
                    {
                        return await Task.Run(() => BadRequest("Ya exíste una Nota de Crédito creada con el mismo RTN"));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_creditNote));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidacionCreditNoteName([FromBody]CreditNote creditNote)
        {
            List<CreditNote> _creditNote = new List<CreditNote>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNote/GetCreditNote");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _creditNote = JsonConvert.DeserializeObject<List<CreditNote>>(valorrespuesta);
                    _creditNote = _creditNote.Where(q => q.CreditNoteName == creditNote.CreditNoteName).ToList();
                    if (_creditNote.Count > 0)
                    {
                        return await Task.Run(() => BadRequest("Ya exíste una Nota de Crédito creada con el mismo Nombre"));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Ok(_creditNote));
        }
    }
}