using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
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
    public class UsuarioController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public UsuarioController(ILogger<UserRolController> logger,
            IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }


        [Authorize(Policy = "Seguridad.Usuarios")]
        public async Task<IActionResult> Usuarios()
        {
            //ViewData["Branches"] = await ObtenerBranches();
            ViewData["permisoAgregar"] = _principal.HasClaim("Seguridad.Usuarios.Agregar Usuario", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Seguridad.Usuarios.Editar Usuario", "true");
            ViewData["permisoDesbloquear"] = _principal.HasClaim("Seguridad.Usuarios.Desbloquear Usuario", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Seguridad.Usuarios.Exportar Usuario", "true");
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
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta).Where(w => Convert.ToBoolean(w.IsEnabled) ).ToList() ;

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
            _users.Select(s => { s.PasswordHash = String.Empty; return s; }).ToList();
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
                if (_usuario.PasswordHash.Equals(_usuario.UserName))
                {
                    return this.Json(new DataSourceResult
                    {
                        Errors = $"Error: La contraseña no puede ser igual que el usuario!"
                    });
                }
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _usuario.UsuarioCreacion = HttpContext.Session.GetString("user");
                _usuario.FechaCreacion = DateTime.Now;
                _usuario.UsuarioModificacion = HttpContext.Session.GetString("user");
                _usuario.UserName = _usuario.Email;
                //_usuario.BranchId = _usuario.Branch.BranchId;
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
                        Errors = $"Ocurrio un error:{error}"

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
                        result = await _client.PostAsJsonAsync(baseadress + "api/Cuenta/CambiarPasswordPoliticas", _cambio);
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
        public async Task<ActionResult<ApplicationUser>> PutUsuario(string LockoutEnd, ApplicationUserDTO _usuario)
        {
            try
            {
                if (LockoutEnd != null&&LockoutEnd.Length<23)
                {
                    int x = 0;
                    int dia=1;
                    int mes=1;
                    int anio=1;
                    string digito = "";
                    foreach (char c in LockoutEnd)
                    {
                        if (c=='/'||c==' ')
                        {
                            switch (x)
                            {
                                case 0:
                                    mes = Convert.ToInt32(digito);
                                    x++;
                                    digito = "";
                                    break;
                                case 1:
                                     dia= Convert.ToInt32(digito);
                                    digito = "";
                                    x++;
                                    break;
                                case 2:
                                    anio = Convert.ToInt32(digito);
                                    digito = "";
                                    x ++;
                                    break;
                                default:
                                    break;
                            }
                            if (c == ' ')
                            {
                                break;
                            }

                        }
                        else
                        {

                        digito = digito + c;
                        }
                    }
                    DateTime fecha = new DateTime(anio,mes,dia);
                    _usuario.LockoutEnd = DateTimeOffset.Parse(fecha.ToString());
                }
                else
                {
                    _usuario.LockoutEnd = null;
                }
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                _usuario.UserName = _usuario.Email;
                _usuario.NormalizedEmail = Convert.ToString(_usuario.Email).ToUpper();
                _usuario.NormalizedUserName = Convert.ToString(_usuario.Email).ToUpper();
                //_usuario.BranchId = _usuario.BranchId;
                HttpClient _client = new HttpClient();
               // _usuario.cambiarpassword = _usuario.cambiarpassword == null ? false : true;
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Usuario/PutUsuario", _usuario);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode){
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usuario = JsonConvert.DeserializeObject<ApplicationUserDTO>(valorrespuesta);
                }
                else{
                    _usuario.PasswordHash = await result.Content.ReadAsStringAsync() + " El password debe tener mayusculas, minusculas y caracteres especiales!";
                    string error = await result.Content.ReadAsStringAsync();
                    return this.Json(new DataSourceResult
                    {
                        Errors = $"{error}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            
            //_usuario.PasswordHash = "**********************";
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

        [HttpPost("Desbloquear")]
        public async Task<ActionResult<ApplicationUser>> Desbloquear([FromBody]string Id)
        {
            ApplicationUser _usuario = new ApplicationUser();
            _usuario.Id = Guid.Parse(Id);
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));               
                var result = await _client.PostAsJsonAsync(baseadress + "api/Usuario/DesbloqueoUsuario", _usuario);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usuario = JsonConvert.DeserializeObject<ApplicationUser>(valorrespuesta);                    
                }
                else
                {                    
                    string error = await result.Content.ReadAsStringAsync();
                    return this.Json(new DataSourceResult
                    {
                        //Data=  _usuario ,
                        Errors = $"Ocurrio un error:{error} El password debe tener mayusculas y minusculas!"

                    });

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //return BadRequest($"Ocurrio un error{ex.Message}");
                throw ex;

            }
            return _usuario;
        }

    }
}