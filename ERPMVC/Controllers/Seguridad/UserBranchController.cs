using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class UserBranchController : Controller
    {

        private readonly IOptions<MyConfig> _config;
        private readonly ILogger _logger;

        public UserBranchController(ILogger<EstadosController> logger, IOptions<MyConfig> config)
        {
            this._config = config;
            this._logger = logger;

        }

        [Authorize(Policy = "Seguridad.Sucursales por Usuario")]
        public IActionResult UserBranch()
        {
            return View();
        }


        [HttpGet("GetUsuarios")]
        public async Task<DataSourceResult> GetUsuarios([DataSourceRequest]DataSourceRequest request)
        {
            List<ApplicationUser> _users = new List<ApplicationUser>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Usuario/GetUsers");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<List<ApplicationUser>>(valorrespuesta);
                    _users = _users.OrderBy(q => q.UserName).ToList();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            //return Json(_users);
            return _users.ToDataSourceResult(request);
        }

        [HttpPost]
        public async Task<JsonResult> SucursalesPorUsuario(UserBranch Sucursal, Guid Userselect)
        {

            UserBranch _usersbra = new UserBranch();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                Sucursal.UserId = Userselect;

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/UserBranch/ObtenerUserBranch", Sucursal);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usersbra = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }            
            return Json(_usersbra);
        }

        [HttpGet]
        public async Task<JsonResult> Sucursales(string UserParametrp)
        {
            List<UserBranch> _FormarSucursales = new List<UserBranch>();           
            List<Branch> _branchs = new List<Branch>();

            if(UserParametrp == null)
            {
                List<UserBranch> Branch = _FormarSucursales;
                _FormarSucursales = Branch;
            }
            else
            {
                Guid Userselect = new Guid("" + UserParametrp + "");
                try
                {
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    string apidir = "api/Branch/GetBranch";
                    var result = await _client.GetAsync(baseadress + apidir);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _branchs = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta);
                    }

                    foreach (var Brach in _branchs)
                    {
                        UserBranch NuevaBrach = new UserBranch();
                        NuevaBrach.BranchId = Brach.BranchId;

                        var SucursalesAsignadas = await SucursalesPorUsuario(NuevaBrach, Userselect);
                        var ASucursal = ((UserBranch)SucursalesAsignadas.Value);

                        if (ASucursal == null)
                        {
                        }
                        else
                        {
                            NuevaBrach.Id = ASucursal.Id;
                            NuevaBrach.BranchId = ASucursal.BranchId;
                            NuevaBrach.BranchName = ASucursal.BranchName;
                            NuevaBrach.UserId = Userselect;                           
                            _FormarSucursales.Add(NuevaBrach);
                        }


                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

            }
            return Json(_FormarSucursales);          
        }
                          
        [HttpGet]
        public async Task<JsonResult> SucursalesNo(string UserParametrp)
        {
            List<UserBranch> _FormaSucursales = new List<UserBranch>();
            List<Branch> _branchs = new List<Branch>();

            if (UserParametrp == null)
            {
                List<UserBranch> Sucursales = _FormaSucursales;
                _FormaSucursales = Sucursales;
            }
            else
            {
                Guid Userselect = new Guid("" + UserParametrp + "");
                try
                {
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    string apidir = "api/Branch/GetBranchUserAssignement";
                    var result = await _client.GetAsync(baseadress + apidir);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _branchs = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta);
                    }
                    foreach (var Brach in _branchs)
                    {
                        UserBranch NuevaBrach = new UserBranch();
                        NuevaBrach.BranchId = Brach.BranchId;

                        var SucursalesAsignadas = await SucursalesPorUsuario(NuevaBrach, Userselect);
                        var ASucursal = ((UserBranch)SucursalesAsignadas.Value);
                        
                        if (ASucursal == null)
                        {
                            NuevaBrach.BranchId = Brach.BranchId;
                            NuevaBrach.BranchName = Brach.BranchName;
                            NuevaBrach.UserId = new Guid("00000000-0000-0000-0000-000000000000");
                            NuevaBrach.Id = 0;
                            _FormaSucursales.Add(NuevaBrach);
                        }
                        else
                        {
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            return Json(_FormaSucursales);           
        }


        [HttpGet]
        public async Task<JsonResult> InsertAndDelete(string UsuarioId, Int32 BranchId, string BranchName, bool Accion, Int32 Id)
        {
            UserBranch _usersbra = new UserBranch();
            try
            {
                Guid UserSucursal = new Guid("" + UsuarioId + "");

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _usersbra.UserId = UserSucursal;
                _usersbra.BranchId = BranchId;

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/UserBranch/ObtenerUserBranch", _usersbra);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _usersbra = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);
                }

                if(_usersbra == null && Accion == true) {

                    UserBranch _UserBranch = new UserBranch();                  

                    string baseadres = _config.Value.urlbase;
                    HttpClient _clients = new HttpClient();
                    _clients.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    _UserBranch.UserId = UserSucursal;
                    _UserBranch.BranchId = BranchId;
                    _UserBranch.BranchName = BranchName;
                    _UserBranch.CreatedUser = HttpContext.Session.GetString("user");
                    _UserBranch.CreatedDate = DateTime.Now;
                    var resultsave = await _client.PostAsJsonAsync(baseadress + "api/UserBranch/Insert", _UserBranch);
                    string valorrespuestasave = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuestasave = await (result.Content.ReadAsStringAsync());
                        _UserBranch = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);
                    }
                }

                if (_usersbra != null && Accion == false)
                {

                    UserBranch _UserBranch = new UserBranch();

                    _UserBranch.UserId = UserSucursal;
                    _UserBranch.BranchId = BranchId;
                    _UserBranch.BranchName = BranchName;
                    _UserBranch.Id = Id;

                    string baseadresdelete = _config.Value.urlbase;
                    HttpClient _clientdelete = new HttpClient();
                    _clientdelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                    var resultdelete = await _clientdelete.PostAsJsonAsync(baseadress + "api/UserBranch/Delete", _UserBranch);
                    string valorrespuestadelete = "";
                    if (resultdelete.IsSuccessStatusCode)
                    {
                        valorrespuestadelete = await (resultdelete.Content.ReadAsStringAsync());
                        _UserBranch = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            
            return Json(_usersbra);

        }


    }

}