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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class InventarioFisicoLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public InventarioFisicoLineController(ILogger<InventarioFisicoLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<DataSourceResult> GetInventarioFisicoLines([DataSourceRequest] DataSourceRequest request
            , [FromQuery(Name = "Id")] int InventarioId
            , [FromQuery(Name = "BranchId")] int branchid
            , [FromQuery(Name = "CustomerId")] int CustomerId
            , [FromQuery(Name = "ProductId")] int ProductId)
        {
            try
            {
                if (InventarioId == 0)
                {
                    return await  GetSaldoLibros(request,branchid, CustomerId, ProductId);
                }
                else
                {
                    return await GetInventarioLinesByInventarioId(request,InventarioId);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            

        }


        public async Task<DataSourceResult> GetInventarioBodegaHabilitadaLines([DataSourceRequest] DataSourceRequest request
           , [FromQuery(Name = "Id")] int InventarioId)
        {

            List<InventarioBodegaHabilitada> _InventarioFisicoLine = new List<InventarioBodegaHabilitada>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisicoLine/GetInventarioBodegaHabilitadaLines/" + InventarioId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisicoLine = JsonConvert.DeserializeObject<List<InventarioBodegaHabilitada>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InventarioFisicoLine.ToDataSourceResult(request);



        }





        public async Task<DataSourceResult> GetSaldoLibros([DataSourceRequest] DataSourceRequest request
            ,  int branchid
            ,  int CustomerId
            , int ProductId)
        {
            List<InventarioFisicoLine> InventarioFisicoLines = new List<InventarioFisicoLine>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                string requestURl;
                requestURl = $"api/InventarioFisico/GetSaldoLibros/{branchid}/{CustomerId}/{ProductId}";
                var result = await _client.GetAsync(baseadress + requestURl);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    InventarioFisicoLines = JsonConvert.DeserializeObject<List<InventarioFisicoLine>>(valorrespuesta);
                    InventarioFisicoLines = InventarioFisicoLines.OrderByDescending(e => e.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return InventarioFisicoLines.ToDataSourceResult(request);



        }

        public async Task<ActionResult> pvwInventarioFisicoDetailMant(Int64 InventarioFisicoLineId = 0)
        {
            InventarioFisicoLine _InventarioFisicoLine = new InventarioFisicoLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisicoLine/GetInventarioFisicoLineById/" + InventarioFisicoLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisicoLine = JsonConvert.DeserializeObject<InventarioFisicoLine>(valorrespuesta);

                }

                if (_InventarioFisicoLine == null)
                {
                    _InventarioFisicoLine = new InventarioFisicoLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView("~/Views/InventarioFisico/pvwInventarioFisicoDetailMant.cshtml", _InventarioFisicoLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InventarioFisicoLine> _InventarioFisicoLine = new List<InventarioFisicoLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisicoLine/GetInventarioFisicoLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisicoLine = JsonConvert.DeserializeObject<List<InventarioFisicoLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InventarioFisicoLine.ToDataSourceResult(request);

        }

        [HttpGet]
        public async Task<DataSourceResult> GetInventarioLinesByInventarioId(DataSourceRequest request, Int64 InventarioId)
        {
            List<InventarioFisicoLine> _InventarioFisicoLine = new List<InventarioFisicoLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InventarioFisicoLine/GetInventarioLinesByInventarioId/" + InventarioId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisicoLine = JsonConvert.DeserializeObject<List<InventarioFisicoLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InventarioFisicoLine.ToDataSourceResult(request);

        }





    

        // POST: InventarioFisicoLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InventarioFisicoLine>> Insert(InventarioFisicoLine _InventarioFisicoLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                // _InventarioFisicoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_InventarioFisicoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InventarioFisicoLine/Insert", _InventarioFisicoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisicoLine = JsonConvert.DeserializeObject<InventarioFisicoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_InventarioFisicoLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InventarioFisicoLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InventarioFisicoLine>> Update(Int64 id, InventarioFisicoLine _InventarioFisicoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InventarioFisicoLine/Update", _InventarioFisicoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InventarioFisicoLine = JsonConvert.DeserializeObject<InventarioFisicoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InventarioFisicoLine }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InventarioFisicoLine>> Delete([FromBody]InventarioFisicoLine _InventarioFisicoLine)
        {
            List<InventarioFisicoLine> _salesorderLIST = new List<InventarioFisicoLine>();
            try
            {

                 _salesorderLIST =
                JsonConvert.DeserializeObject<List<InventarioFisicoLine>>(HttpContext.Session.GetString("listadoproductosInventarioFisico"));

                if (_salesorderLIST != null)
                {
                    

                    HttpContext.Session.SetString("listadoproductosInventarioFisico", JsonConvert.SerializeObject(_salesorderLIST));
                }


                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                //var result = await _client.PostAsJsonAsync(baseadress + "api/InventarioFisicoLine/Delete", _InventarioFisicoLine);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _InventarioFisicoLine = JsonConvert.DeserializeObject<InventarioFisicoLine>(valorrespuesta);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


            return await Task.Run(() => Ok(_salesorderLIST));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _InventarioFisicoLine }, Total = 1 });
        }





    }
}