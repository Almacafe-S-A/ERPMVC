using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class UsuarioController : Controller
    {

         private readonly IOptions<MyConfig> config;

        public UsuarioController(IOptions<MyConfig> config)
        {
            this.config = config;
        }

        [Authorize(Policy ="Admin")]
        public IActionResult Usuarios()
        {
            return View();
        }


        
        [HttpGet("[action]")]
        public async Task<JsonResult> GetUsuarios()
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });

                //if (resultlogin.IsSuccessStatusCode)
                //{

                //    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " +  HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsuarios");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);

                    }
                //}
            }
            catch (System.Exception myExc)
            {
                throw (new Exception(myExc.Message));
            }
            return Json(_users);
        }

        [HttpGet("GetUsers")]
        public async Task<DataSourceResult> GetUsers([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            //var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.Value.UserEmail, Password = config.Value.UserPassword });

            //if (resultlogin.IsSuccessStatusCode)
            //{

            //    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
            //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);

                }
           // }


            return _users.ToDataSourceResult(request);
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

            //    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
            //    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

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

        [HttpPost("PostUsuario")]
        public async Task<ActionResult<ApplicationUser>> PostUsuario(ApplicationUser _usuario)
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
                    var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/PostUsuario", _usuario);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                    }
               // }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });
        }

        [HttpPut("PutUsuario")]
        public async Task<ActionResult<ApplicationUser>> PutUsuario(string Id, ApplicationUser _usuario)
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

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PutAsJsonAsync(baseadress + "api/Usuario/PutUsuario", _usuario);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                    }
                //}

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });

        }

        [HttpDelete("DeleteUsuario")]
        public async Task<ActionResult<ApplicationUser>> DeleteUsuario(ApplicationUser _user)
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

                    var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/DeleteUsuario", _user);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _user = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                    }
               // }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _user }, Total = 1 });
        }




    }
}