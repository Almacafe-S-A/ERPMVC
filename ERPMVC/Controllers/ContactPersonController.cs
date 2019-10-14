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
    public class ContactPersonController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ContactPersonController(ILogger<ContactPersonController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult ContactPerson()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<JsonResult> GetContactPerson([DataSourceRequest]DataSourceRequest request, Int64 VendorId)
        {
            List<ContactPerson> _customers = new List<ContactPerson>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContactPerson/GetContactPerson/" + VendorId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<ContactPerson>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult> Insert(ContactPerson _ContactPerson)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ContactPerson.CreatedUser = HttpContext.Session.GetString("user");
                _ContactPerson.CreatedDate = DateTime.Now;
                _ContactPerson.ModifiedUser = HttpContext.Session.GetString("user");
                _ContactPerson.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/ContactPerson/Insert", _ContactPerson);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContactPerson = JsonConvert.DeserializeObject<ContactPerson>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ContactPerson }, Total = 1 });
        }


        [HttpPut("ContactPersonId")]
        public async Task<IActionResult> Update(Int64 ContactPersonId, ContactPerson _ContactPerson)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ContactPerson/Update", _ContactPerson);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContactPerson = JsonConvert.DeserializeObject<ContactPerson>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ContactPerson }, Total = 1 });
        }

        public async Task<ActionResult<ContactPerson>> SaveContactPerson([FromBody]ContactPersonDTO _ContactPersonP)
        {
            ContactPerson _ContactPerson = _ContactPersonP;
            try
            {
                //JournalEntry _listItems = new JournalEntry();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContactPerson/GetContactPersonById/" + _ContactPerson.ContactPersonId);
                string valorrespuesta = "";
                _ContactPerson.ModifiedDate = DateTime.Now;
                _ContactPerson.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContactPerson = JsonConvert.DeserializeObject<ContactPerson>(valorrespuesta);
                }

                if (_ContactPerson == null) { _ContactPerson = new Models.ContactPerson(); }

                if (_ContactPersonP.ContactPersonId == 0)
                {
                    _ContactPersonP.CreatedDate = DateTime.Now;
                    _ContactPersonP.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ContactPersonP);
                }
                else
                {
                    _ContactPersonP.CreatedUser = _ContactPerson.CreatedUser;
                    _ContactPersonP.CreatedDate = _ContactPerson.CreatedDate;
                    var updateresult = await Update(_ContactPerson.ContactPersonId, _ContactPersonP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ContactPersonP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddContactPerson([FromBody]ContactPersonDTO _sarpara)
        {
            ContactPersonDTO _ContactPerson = new ContactPersonDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContactPerson/GetContactPersonById/" + _sarpara.ContactPersonId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContactPerson = JsonConvert.DeserializeObject<ContactPersonDTO>(valorrespuesta);

                }

                if (_ContactPerson == null)
                {
                    _ContactPerson = new ContactPersonDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ContactPerson);

        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetContactPersonById(Int64 ContactPersonId)
        {
            ContactPerson _customers = new ContactPerson();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContactPerson/GetContactPersonById/" + ContactPersonId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<ContactPerson>(valorrespuesta);

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