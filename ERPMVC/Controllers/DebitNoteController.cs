﻿using System;
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
    public class DebitNoteController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public DebitNoteController(ILogger<DebitNoteController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<ActionResult> pvwDebitNote(Int64 Id = 0)
        //{
        //    DebitNote _DebitNote = new DebitNote();
        //    try
        //    {
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/DebitNote/GetDebitNoteById/" + Id);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _DebitNote = JsonConvert.DeserializeObject<DebitNote>(valorrespuesta);

        //        }

        //        if (_DebitNote == null)
        //        {
        //            _DebitNote = new DebitNote();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }



        //    return PartialView(_DebitNote);

        //}
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

                }
                if (_DebitNote == null)
                {
                    _DebitNote = new DebitNoteDTO { OrderDate = DateTime.Now, DeliveryDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(30), editar = 1, BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")) };
                }
                else
                {
                    Branch _branch = new Branch();
                    string valorrespuestaBranch = "";
                    var resultBranch = await _client.GetAsync(baseadress + "api/Branch/GetBranchById/" + _DebitNote.BranchId);
                    valorrespuestaBranch = await (resultBranch.Content.ReadAsStringAsync());
                    _branch = JsonConvert.DeserializeObject<Branch>(valorrespuestaBranch);

                    _DebitNote.NumeroDEIString = $"{_branch.BranchCode}-{_DebitNote.Caja}-05-{_DebitNote.NúmeroDEI.ToString().PadLeft(8, '0')} ";
                    //var resultado = new BranchController().FileUploadMsgView(_DebitNote.BranchId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return PartialView(_DebitNote);

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

                if (_listDebitNote == null) { _listDebitNote = new DebitNote(); }

                if (_listDebitNote.DebitNoteId == 0)
                {
                    _DebitNote.FechaCreacion = DateTime.Now;
                    _DebitNote.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DebitNote);
                    var value = (insertresult.Result as ObjectResult).Value;

                    DebitNote resultado = ((DebitNote)(value));
                    if (resultado.Fiscal is false)
                    {
                        if (resultado.CAI == "" || resultado.CAI == null)
                        {
                            string error = await result.Content.ReadAsStringAsync();
                            return await Task.Run(() => BadRequest($" No existe un CAI activo para el punto de emisión."));
                        }
                    }
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