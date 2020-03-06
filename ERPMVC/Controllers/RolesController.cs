using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class RolesController : Controller
    {
         private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public RolesController(ILogger<RolesController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Seguridad.Roles")]
        public IActionResult Roles()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Seguridad.Roles.Agregar Roles", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Seguridad.Roles.Editar Roles", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Seguridad.Roles.Exportar Roles", "true");
            return View();
        }

        [Authorize(Policy = "Seguridad.Permiso Roles")]
        public IActionResult PermisosRoles()
        {
            return View();
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> GetJsonRoles([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationRole> _users = new List<ApplicationRole>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Roles/GetJsonRoles");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationRole>>(valorrespuesta);

                }
                
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }



        [HttpGet("GetRoles")]
        public async Task<DataSourceResult> GetRoles([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationRole> _roles = new List<ApplicationRole>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                string token = "";
                token = HttpContext.Session.GetString("token");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var result = await _client.GetAsync(baseadress + "api/Roles/GetRoles");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _roles = JsonConvert.DeserializeObject<List<ApplicationRole>>(valorrespuesta);
                    _roles = _roles.OrderBy(q => q.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //return BadRequest($"Ocurrio un error{ex.Message}");
            }
           

            return _roles.ToDataSourceResult(request);
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetPolicyRoles([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationRole> _roles = new List<ApplicationRole>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                string token = "";
                token = HttpContext.Session.GetString("token");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var result = await _client.GetAsync(baseadress + "api/Roles/GetRoles");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _roles = JsonConvert.DeserializeObject<List<ApplicationRole>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //return BadRequest($"Ocurrio un error{ex.Message}");
            }


            return _roles.ToDataSourceResult(request);
        }

        [HttpGet]
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
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
           

            return View(_usuario);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ApplicationRole>> PostRol(ApplicationRole _roleS)
        {
            ApplicationRole _role = _roleS;
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.GetAsync(baseadress + "api/Roles/GetRoleByName/" + _role.Name);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _role = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                }
                if (_role == null) { _role = new Models.ApplicationRole(); _role = _roleS; }

                _role.UsuarioCreacion = HttpContext.Session.GetString("user");
                _role.UsuarioModificacion = HttpContext.Session.GetString("user");
                _role.FechaCreacion = DateTime.Now;
                _role.FechaModificacion = DateTime.Now;

                if (_role.Id != _roleS.Id)
                {
                    return await Task.Run(() => BadRequest($"Ya éxiste un rol registrado con este nombre."));
                }
                else
                {
                    result = await _client.PostAsJsonAsync(baseadress + "api/Roles/CreateRole", _role);
                    valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _role = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
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

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                _rol.UsuarioModificacion = HttpContext.Session.GetString("user");
                _rol.FechaModificacion = DateTime.Now;
                var result = await _client.PutAsJsonAsync(baseadress + "api/Roles/PutRol", _rol);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _rol = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                }
                else
                {
                    return BadRequest($"Ocurrio un error{result.ReasonPhrase}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
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

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Roles/Delete", _rol);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _rol = JsonConvert.DeserializeObject<ApplicationRole>(valorrespuesta);
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });
        }

        [Authorize(Policy = "Seguridad.Listar Permisos")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<string>>> ListarPermisos()
        {
            List<string> permisos = new List<string>();
            try
            {
                string baseDireccion = config.Value.urlbase;
                HttpClient _cliente = new HttpClient();

                _cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _cliente.GetAsync(baseDireccion + "api/Permisos/ListarPermisos");
                
                if (result.IsSuccessStatusCode)
                {
                    var respuesta = await (result.Content.ReadAsStringAsync());
                    permisos = JsonConvert.DeserializeObject<List<string>>(respuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return await Task.Run(() => Ok(permisos));
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<string>> GuardarAsignacionesPermisoRol([FromBody]PostAsignacionesPermisoRol asignaciones)
        {
            try
            {
                string urlBase = config.Value.urlbase;
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var resultado = await cliente.PostAsJsonAsync(urlBase + "api/Roles/GuardarPermisosAsignados", asignaciones);
                if (resultado.IsSuccessStatusCode)
                {
                    return await Task.Run(() => Ok(0));
                }
                else
                {
                    _logger.LogError($"Ocurrio un error y no se pudieron almacenar los permisos");
                    return BadRequest($"Ocurrio un error y no se pudieron almacenar los permisos");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
        }

        [Authorize(Policy = "Seguridad.Listar Permisos")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<RolPermisoAsignacion>>> AsignacionPermisosRol([FromQuery(Name="idRol")]string idRol)
        {
            List<string> permisosSistema = new List<string>();
            List<string> permisosRol = new List<string>();
            TreeDataSourceResult datos;
            if (idRol == null)
            {
                datos = new TreeDataSourceResult();

                return await Task.Run(() => Ok(datos));

            }
            try
            {
                string urlBase = config.Value.urlbase;
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization","Bearer " + HttpContext.Session.GetString("token"));

                var resPermisosSistema = await cliente.GetAsync(urlBase + "api/Permisos/ListarPermisos");
                if (resPermisosSistema.IsSuccessStatusCode)
                {
                    var contenidoRespuesta = await resPermisosSistema.Content.ReadAsStringAsync();
                    permisosSistema = JsonConvert.DeserializeObject<List<string>>(contenidoRespuesta);

                    var resPermisosRol = await cliente.GetAsync(urlBase + $"api/Roles/ListarPermisos/{idRol}");
                    if (resPermisosRol.IsSuccessStatusCode)
                    {
                        contenidoRespuesta = await resPermisosRol.Content.ReadAsStringAsync();
                        permisosRol = JsonConvert.DeserializeObject<List<string>>(contenidoRespuesta);
                        List<RolPermisoAsignacion> permisosAsignados = new List<RolPermisoAsignacion>();

                        foreach (string permiso in permisosSistema)
                        {
                            var campos = permiso.Split(".");
                            permisosAsignados.Add(new RolPermisoAsignacion()
                                                {
                                                    Id = permiso,
                                                    Asignado = false,
                                                    Categoria = campos.Length==1?campos[0]:"",
                                                    Nivel1 = campos.Length==2?campos[1]:"",
                                                    Nivel2 = campos.Length == 3 ? campos[2] : "",
                                                    Nivel3 = campos.Length == 4 ? campos[3] : "",
                                                    IdPadre = campos.Length<=1?null:
                                                        campos.Length==2?campos[0]:
                                                        campos.Length==3?campos[0]+"."+campos[1]:campos[0]+"."+campos[1]+"."+campos[2]
                                                });
                        }

                        foreach (string permiso in permisosRol)
                        {
                            var permisoAsignado = permisosAsignados.Find(p => p.Id.Equals(permiso));
                            if (permisoAsignado != null)
                            {
                                permisoAsignado.Asignado = true;
                            }
                        }

                        return await Task.Run(() => Ok(permisosAsignados));
                    }
                    else if (resPermisosRol.StatusCode == HttpStatusCode.Forbidden)
                    {
                        _logger.LogError($"Acceso denegado");
                        return BadRequest($"Acceso denegado");
                    }
                    else
                    {
                        _logger.LogError($"No se pudo cargar los permisos del rol {resPermisosSistema.ReasonPhrase}");
                        return BadRequest($"No se pudo cargar los permisos del rol {resPermisosSistema.ReasonPhrase}");
                    }
                }
                else if(resPermisosSistema.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogError($"Acceso denegado");
                    return BadRequest($"Acceso denegado");
                }
                else
                {
                    _logger.LogError($"No se pudo cargar los permisos del sistema {resPermisosSistema.ReasonPhrase}");
                    return BadRequest($"No se pudo cargar los permisos del sistema {resPermisosSistema.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
        }
    }
}