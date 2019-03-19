using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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

        public IActionResult Usuarios()
        {
            return View();
        }

        [HttpGet("GetUsers")]
        public async Task<DataSourceResult> GetUsers([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = "erp@bi-dss.com", Password = "Aa123456!" });

            if (resultlogin.IsSuccessStatusCode)
            {

                string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsuarios");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);

                }
            }


            return _users.ToDataSourceResult(request);
        }

        [HttpPost("PostUsuario")]
        public async Task<ActionResult<ApplicationUser>> PostUsuario(ApplicationUser _usuario)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = "erp@bi-dss.com", Password = "Aa123456!" });

                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/PostUsuario", _usuario);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });
        }

        [HttpPut("PutUsuario")]
        public async Task<ActionResult<ApplicationUser>> PutUsuario(Int64 Id, ApplicationUser _usuario)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = "erp@bi-dss.com", Password = "Aa123456!" });

                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/PutUsuario", _usuario);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                    }
                }

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


                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = "erp@bi-dss.com", Password = "Aa123456!" });

                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await(resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);

                    var result = await _client.PutAsJsonAsync(baseadress + "api/Usuario/Delete", _user);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await(result.Content.ReadAsStringAsync());
                        _user = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _user }, Total = 1 });
        }




    }
}