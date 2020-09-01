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
    public class CostListItemController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CostListItemController(ILogger<CostListItemController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        // GET: CostListItem
        public ActionResult CostListItem()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetCostListItem([DataSourceRequest]DataSourceRequest request)
        {
            List<CostListItem> _CostListItem = new List<CostListItem>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostListItem/GetCostListItem");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostListItem = JsonConvert.DeserializeObject<List<CostListItem>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_CostListItem.ToDataSourceResult(request));

        }
        public async Task<ActionResult<CostListItem>> SaveCostListItem([FromBody]CostListItemDTO _CostListItemP)
        {
            CostListItem _CostListItem = _CostListItemP;
            try
            {
                CostListItem _listAccount = new CostListItem();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostListItem/GetCostListItemById/" + _CostListItem.CostListItemId);
                string valorrespuesta = "";
                _CostListItem.ModifiedDate = DateTime.Now;
                _CostListItem.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostListItem = JsonConvert.DeserializeObject<CostListItem>(valorrespuesta);
                }

                if (_CostListItem == null) { _CostListItem = new Models.CostListItem(); }

                if (_CostListItemP.CostListItemId == 0)
                {
                    _CostListItem.CreatedDate = DateTime.Now;
                    _CostListItem.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CostListItemP);
                }
                else
                {
                    _CostListItemP.CreatedUser = _CostListItem.CreatedUser;
                    _CostListItemP.CreatedDate = _CostListItem.CreatedDate;
                    var updateresult = await Update(_CostListItem.CostListItemId, _CostListItemP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CostListItemP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCostListItem([FromBody]CostListItemDTO _sarpara)
        {
            CostListItemDTO _CostListItem = new CostListItemDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostListItem/GetCostListItemById/" + _sarpara.CostListItemId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostListItem = JsonConvert.DeserializeObject<CostListItemDTO>(valorrespuesta);

                }

                if (_CostListItem == null)
                {
                    _CostListItem = new CostListItemDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CostListItem);

        }

        // POST: CostListItem/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(CostListItem _CostListItem)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CostListItem.CreatedUser = HttpContext.Session.GetString("user");
                _CostListItem.CreatedDate = DateTime.Now;
                _CostListItem.ModifiedUser = HttpContext.Session.GetString("user");
                _CostListItem.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CostListItem/Insert", _CostListItem);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostListItem = JsonConvert.DeserializeObject<CostListItem>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CostListItem }, Total = 1 });
        }

        [HttpPut("CostListItemId")]
        public async Task<IActionResult> Update(Int64 CostListItemId, CostListItem _CostListItem)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/CostListItem/Update", _CostListItem);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostListItem = JsonConvert.DeserializeObject<CostListItem>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CostListItem }, Total = 1 });
        }

    }
}