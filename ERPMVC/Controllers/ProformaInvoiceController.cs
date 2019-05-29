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
    public class ProformaInvoiceController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ProformaInvoiceController(ILogger<ProformaInvoiceController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwProformaInvoice(Int64 Id = 0)
        {
            ProformaInvoiceDTO _ProformaInvoice = new ProformaInvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoiceById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoiceDTO>(valorrespuesta);

                }

                if (_ProformaInvoice == null)
                {
                    _ProformaInvoice = new ProformaInvoiceDTO { ExpirationDate = DateTime.Now.AddDays(30), OrderDate = DateTime.Now, editar = 1 };
                }
                else
                {
                    _ProformaInvoice.editar = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ProformaInvoice);

        }


        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ProformaInvoice> _ProformaInvoice = new List<ProformaInvoice>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoice");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<List<ProformaInvoice>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProformaInvoice.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> SaveProformaInvoice([FromBody]ProformaInvoice _ProformaInvoice)
        {

            try
            {
                ProformaInvoice _listProformaInvoice = new ProformaInvoice();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetProformaInvoiceById/" + _ProformaInvoice.ProformaId);
                string valorrespuesta = "";
                _ProformaInvoice.FechaModificacion = DateTime.Now;
                _ProformaInvoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

                if (_listProformaInvoice.ProformaId == 0)
                {
                    _ProformaInvoice.FechaCreacion = DateTime.Now;
                    _ProformaInvoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProformaInvoice);
                }
                else
                {
                    var updateresult = await Update(_ProformaInvoice.ProformaId, _ProformaInvoice);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProformaInvoice);
        }

        // POST: ProformaInvoice/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Insert(ProformaInvoice _ProformaInvoice)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ProformaInvoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ProformaInvoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoice/Insert", _ProformaInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_ProformaInvoice);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Update(Int64 id, ProformaInvoice _ProformaInvoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ProformaInvoice/Update", _ProformaInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoice>> Delete([FromBody]ProformaInvoice _ProformaInvoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoice/Delete", _ProformaInvoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoice = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoice }, Total = 1 });
        }





    }
}