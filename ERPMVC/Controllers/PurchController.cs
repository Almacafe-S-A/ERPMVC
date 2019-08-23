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
    public class PurchController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PurchController(ILogger<PurchController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Purch
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult Proveedores()
        {
            return View();
        }*/
        // GET: Purch/Details/5
       // public ActionResult Details(int id)
        //{
          //  return View();
        //}

        // GET: Purch/Create
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Proveedores(Int64 PurchId)
        {
            Purch _customers = new Purch();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + PurchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<Purch>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_customers));
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> GetPurch([DataSourceRequest]DataSourceRequest request)
        {
            List<Purch> _customers = new List<Purch>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurch");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Purch>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_customers.ToDataSourceResult(request));

        }



        
        // POST: TypeAccount/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Purch _Purch)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Purch.CreatedUser = HttpContext.Session.GetString("user");
                _Purch.CreatedDate = DateTime.Now;
                _Purch.ModifiedUser = HttpContext.Session.GetString("user");
                _Purch.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Purch/Insert", _Purch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Purch = JsonConvert.DeserializeObject<Purch>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Purch }, Total = 1 });
        }


        [HttpPut("PurchId")]
        public async Task<IActionResult> Update(Int64 PurchId, Purch _Purch)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Purch/Update", _Purch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Purch = JsonConvert.DeserializeObject<Purch>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Purch }, Total = 1 });
        }
        // GET: Purch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Purch/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult<Purch>> SavePurch([FromBody]PurchDTO _PurchP)
        {
            Purch _Purch = _PurchP;
            try
            {
                Purch _listAccount = new Purch();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + _Purch.PurchId);
                string valorrespuesta = "";
                _Purch.ModifiedDate = DateTime.Now;
                _Purch.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Purch = JsonConvert.DeserializeObject<Purch>(valorrespuesta);
                }

                if (_Purch == null) { _Purch = new Models.Purch(); }

                if (_PurchP.PurchId == 0)
                {
                    _Purch.CreatedDate = DateTime.Now;
                    _Purch.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PurchP);
                }
                else
                {
                    _PurchP.CreatedUser = _Purch.CreatedUser;
                    _PurchP.CreatedDate = _Purch.CreatedDate;
                    var updateresult = await Update(_Purch.PurchId, _PurchP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PurchP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPurch([FromBody]PurchDTO _sarpara)
        {
            PurchDTO _Purch = new PurchDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + _sarpara.PurchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Purch = JsonConvert.DeserializeObject<PurchDTO>(valorrespuesta);

                }

                if (_Purch == null)
                {
                    _Purch = new PurchDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Purch);

        }
        // GET: Customer/Details/5

/*
        [HttpGet("[action]")]
        public async Task<ActionResult> Proveedores(Int64 PurchId)
        {
            Purch _customers = new Purch();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + PurchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<Purch>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_customers));
        }
  */      
        [HttpGet("[action]")]
        public async Task<ActionResult> GetPurchById(Int64 PurchId)
        {
            Purch _customers = new Purch();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + PurchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<Purch>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_customers));
        }


    }
}