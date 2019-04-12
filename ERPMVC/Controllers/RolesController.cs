using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
     [Authorize]
      [CustomAuthorization]
    public class RolesController : Controller
    {
         private readonly IOptions<MyConfig> config;

        public RolesController(IOptions<MyConfig> config)
        {
            this.config = config;
        }

        public IActionResult Roles()
        {
            return View();
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> GetJsonRoles()
        {
            List<ApplicationRole> _users = new List<ApplicationRole>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });

                //if (resultlogin.IsSuccessStatusCode)
                //{

                   // string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                  //  UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/Roles/GetJsonRoles");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _users = JsonConvert.DeserializeObject<List<ApplicationRole>>(valorrespuesta);
                        
                    }
                //}
            }
            catch (System.Exception myExc)
            {
                throw (new Exception(myExc.Message));
            }
            return Json(_users);
        }



        [HttpGet("GetRoles")]
        public async Task<DataSourceResult> GetRoles([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationRole> _roles = new List<ApplicationRole>();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
          //  var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });

            //if (resultlogin.IsSuccessStatusCode)
            //{

            // string webtoken = await(resultlogin.Content.ReadAsStringAsync());
            // UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
            string token = "";
            token = HttpContext.Session.GetString("token");
           
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
          // _client.DefaultRequestHeaders.Add("Authorization",  token);
           var result = await _client.GetAsync(baseadress + "api/Roles/GetRoles");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                _roles = JsonConvert.DeserializeObject<List<ApplicationRole>>(valorrespuesta);

            }
            // }


            return _roles.ToDataSourceResult(request);
        }

        [HttpGet]
        public async Task<ActionResult> Details(Int64 UserId)
        {
            ApplicationUser _usuario = new ApplicationUser();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });
            //if (resultlogin.IsSuccessStatusCode)
            //{

              //  string webtoken = await (resultlogin.Content.ReadAsStringAsync());
               // UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUserById/" + UserId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);

                }
           // }


            return View(_usuario);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ApplicationRole>> PostRol(ApplicationRole _role)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });

                //if (resultlogin.IsSuccessStatusCode)
                //{
                //    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/Roles/CreateRole", _role);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _role = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                    }
                //}

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _role }, Total = 1 });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ApplicationRole>> PutRol(string Id, ApplicationRole _rol)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });

                //if (resultlogin.IsSuccessStatusCode)
                //{
                //    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                    var result = await _client.PutAsJsonAsync(baseadress + "api/Roles/PutRol", _rol);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _rol = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                    }
                //}

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });

        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<ApplicationRole>> DeleteRole(ApplicationRole _rol)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();


                //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword});

                //if (resultlogin.IsSuccessStatusCode)
                //{
                //    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));

                    var result = await _client.PostAsJsonAsync(baseadress + "api/Roles/Delete", _rol);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _rol = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                    }
                //}
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });
        }




    }
}