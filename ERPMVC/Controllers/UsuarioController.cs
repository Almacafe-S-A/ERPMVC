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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UsuarioController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public UsuarioController(ILogger<UserRolController> logger,
            IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        [Authorize(Policy = "Seguridad.Usuarios")]
        public async Task<IActionResult> Usuarios()
        {
            ViewData["Branches"] = await ObtenerBranches();
            return View();
        }


        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityUsuario()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetQuantityUsuario");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetUsuariosJson([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetUsuarios()
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsuarios");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }

        [HttpGet("[controller]/GetUsers")]
        public async Task<DataSourceResult> GetUsers([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _users.ToDataSourceResult(request);
        }



        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> Details(Int64 UserId)
        {
            ApplicationUser _usuario = new ApplicationUser();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUserById/" + UserId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return View(_usuario);
        }

        [HttpPost("PostUsuario")]
        public async Task<ActionResult<ApplicationUser>> PostUsuario(ApplicationUserDTO _usuario)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _usuario.UsuarioCreacion = HttpContext.Session.GetString("token");
                _usuario.FechaCreacion = DateTime.Now;
                _usuario.UsuarioModificacion = HttpContext.Session.GetString("user");
                _usuario.UserName = _usuario.Email;
                _usuario.BranchId = _usuario.Branch.BranchId;
                _usuario.IsEnabled = true;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/PostUsuario", _usuario);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usuario = JsonConvert.DeserializeObject<ApplicationUserDTO>(valorrespuesta);

                    _usuario.PasswordHash = "**********************";
                }
                else
                {

                    _usuario.PasswordHash = await result.Content.ReadAsStringAsync() + " El password debe tener mayusculas y minusculas!";
                    string error =await result.Content.ReadAsStringAsync();
                    return this.Json(new DataSourceResult
                    {
                         //Data=  _usuario ,
                        Errors = $"Ocurrio un error:{error} El password debe tener mayusculas y minusculas!"

                    });

                    // return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });
                     //return await Task.Run(() => BadRequest($"Ocurrio un error:{result.Content.ReadAsStringAsync()} El password debe tener mayusculas y minusculas!"));
                    //  throw new Exception($"Ocurrio un error:{result.Content.ReadAsStringAsync()} El password debe tener mayusculas y minusculas!");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //return BadRequest($"Ocurrio un error{ex.Message}");
                throw ex;

            }

            _usuario.PasswordHash = "**********************";
            return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });
        }


        [HttpPost("[controller]/[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> ChangePassword([FromBody]CambiarPassDTO _cambio)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
             
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUserByEmail/"+ _cambio.Email);
                if (result.IsSuccessStatusCode)
                {
                    string password = _cambio.Password;
                    string datosUsuario = await (result.Content.ReadAsStringAsync());
                    /*if (!await IsPasswordHistory(JsonConvert.DeserializeObject<ApplicationUser>(datosUsuario).Id.ToString(),password))
                    {*/
                        result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/ChangePassword", _cambio);
                        if (result.IsSuccessStatusCode)
                        {
                            //return new ObjectResult(new DataSourceResult { Data = "", Total = 1 });
                            await HttpContext.SignOutAsync();
                            HttpContext.Session.Clear();
                            return await Task.Run(() => RedirectToAction(nameof(HomeController.Index), "Home"));
                        }
                        else
                        {   
                            string error = await result.Content.ReadAsStringAsync();
                            return await Task.Run(() => BadRequest($"{error}"));
                        }
                    /*}
                    else
                    {
                        return await Task.Run(() => BadRequest($"No puede utilizar las ultimas contraseñas "));
                    }*/
                }
                else
                {
                    return await Task.Run(() => BadRequest($"Usuario o contraseña incorrecta"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error no manejado en el sistema "));
            }
            
        }

        [HttpPut("PutUsuario")]
        public async Task<ActionResult<ApplicationUser>> PutUsuario(string Id, ApplicationUserDTO _usuario)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                _usuario.UserName = _usuario.Email;
                _usuario.BranchId = _usuario.BranchId;
                HttpClient _client = new HttpClient();
                _usuario.cambiarpassword = _usuario.cambiarpassword == null ? false : true;
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Usuario/PutUsuario", _usuario);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usuario = JsonConvert.DeserializeObject<ApplicationUserDTO>(valorrespuesta);
                }
                else
                {

                    _usuario.PasswordHash = await result.Content.ReadAsStringAsync() + " El password debe tener mayusculas, minusculas y caracteres especiales!";
                    string error = await result.Content.ReadAsStringAsync();
                    return this.Json(new DataSourceResult
                    {
                        Errors = $"Error: El password debe cumplir con los requisitos minimos (Longitud 8, Mayúsculas, minúsculas, 1 carater especial y caracteres númericos)!"
                    });
                    // return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });
                    //return await Task.Run(() => BadRequest($"Ocurrio un error{result.Content.ReadAsStringAsync()}"));
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            
            _usuario.PasswordHash = "**********************";
            return new ObjectResult(new DataSourceResult { Data = new[] { _usuario }, Total = 1 });

        }

        [HttpDelete("DeleteUsuario")]
        public async Task<ActionResult<ApplicationUser>> DeleteUsuario(ApplicationUser _user)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/DeleteUsuario", _user);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _user = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _user }, Total = 1 });
        }


        async Task<IEnumerable<Branch>> ObtenerBranches()
        {
            IEnumerable<Branch> branches = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Branch/GetBranch");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    branches = JsonConvert.DeserializeObject<IEnumerable<Branch>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["defaultbranch"] = branches.FirstOrDefault();
            return branches;

        }

    }
}