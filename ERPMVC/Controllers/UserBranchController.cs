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


                List<Branch> _usersbra = new List<Branch>();

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
        public async Task<JsonResult> SucursalesPorUsuario(UserBranch rama, Guid Userselect)
        {

            UserBranch _usersbra = new UserBranch();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                                               
                rama.UserId = Userselect;

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/UserBranch/ObtenerUserBranch", rama);
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
            //return Json(_users);
            return Json(_usersbra);
        }

        [HttpGet]
        public async Task<DataSourceResult> GetBranch([DataSourceRequest]DataSourceRequest request)
        {
            List<Branch> _branchs = new List<Branch>();

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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return _branchs.ToDataSourceResult(request);

        }


        [HttpGet/*("[action]")*/]
        public async Task<JsonResult> Sucursales(/*[FromBody]*/ string UserParametrp)
        {

            //var a = "fc405b7d-9fe3-43c9-97b5-d87a174cab8a";

            //Guid Userselect = new Guid (a);
            List<UserBranchDTO> _FormarRamas = new List<UserBranchDTO>();
            

            List<Branch> _branchs = new List<Branch>();

            if(UserParametrp == null)
            {

                List<UserBranchDTO> ccc  = _FormarRamas;

                _FormarRamas = ccc;

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

                    foreach (var Rama in _branchs)
                    {
                        UserBranchDTO NuevaRama = new UserBranchDTO();
                        NuevaRama.BranchId = Rama.BranchId;

                        var SucursalesAsignadas = await SucursalesPorUsuario(NuevaRama, Userselect);
                        var ASucursal = ((UserBranch)SucursalesAsignadas.Value);


                        if (ASucursal == null)
                        {
                            //NuevaRama.BranchId = Rama.BranchId;
                            //NuevaRama.BranchName = Rama.BranchName;
                            //NuevaRama.UserId = new Guid("00000000-0000-0000-0000-000000000000");
                            //NuevaRama.regitrado = false;

                            //_FormarRamas.Add(NuevaRama);
                        }
                        else
                        {

                            NuevaRama.BranchId = ASucursal.BranchId;
                            NuevaRama.BranchName = ASucursal.BranchName;
                            NuevaRama.UserId = Userselect;
                            NuevaRama.regitrado = true;


                            _FormarRamas.Add(NuevaRama);
                        }


                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

            }


            return Json(_FormarRamas);
          //  return _FormarRamas.ToDataSourceResult(request);
        }





        [HttpGet/*("[action]")*/]
        public async Task<JsonResult> SucursalesNo(/*[FromBody]*/ string UserParametrp)
        {

            //var a = "fc405b7d-9fe3-43c9-97b5-d87a174cab8a";

            //Guid Userselect = new Guid (a);
            List<UserBranchDTO> _FormarRamas = new List<UserBranchDTO>();


            List<Branch> _branchs = new List<Branch>();

            if (UserParametrp == null)
            {

                List<UserBranchDTO> ccc = _FormarRamas;

                _FormarRamas = ccc;

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

                    foreach (var Rama in _branchs)
                    {
                        UserBranchDTO NuevaRama = new UserBranchDTO();
                        NuevaRama.BranchId = Rama.BranchId;

                        var SucursalesAsignadas = await SucursalesPorUsuario(NuevaRama, Userselect);
                        var ASucursal = ((UserBranch)SucursalesAsignadas.Value);


                        if (ASucursal == null)
                        {
                            NuevaRama.BranchId = Rama.BranchId;
                            NuevaRama.BranchName = Rama.BranchName;
                            NuevaRama.UserId = new Guid("00000000-0000-0000-0000-000000000000");
                            NuevaRama.regitrado = false;

                            _FormarRamas.Add(NuevaRama);
                        }
                        else
                        {

                            //NuevaRama.BranchId = ASucursal.BranchId;
                            //NuevaRama.BranchName = ASucursal.BranchName;
                            //NuevaRama.UserId = Userselect;
                            //NuevaRama.regitrado = true;


                            //_FormarRamas.Add(NuevaRama);
                        }


                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }

            }


            return Json(_FormarRamas);
            //  return _FormarRamas.ToDataSourceResult(request);
        }


        [HttpPost]
        public async Task<ActionResult> SaveUserBranch( string[] ArrayNoAsignadas, string[] ArrayAsignadas)
        {

            UserBranch cc = new UserBranch();


            return Json(cc);
        }



        [HttpPost]
        public JsonResult UserBranch(string procesoData, int cabo)
        {


            UserBranch cc = new UserBranch();


            return Json(cc);

        }




        [HttpPost]
        public async Task<ActionResult<UserBranch>> Insert(UserBranch _UserBranch)
        {
            try
            {                
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _UserBranch.CreatedUser = HttpContext.Session.GetString("user");
                _UserBranch.CreatedDate = DateTime.Now;                
                var result = await _client.PostAsJsonAsync(baseadress + "api/UserBranch/Insert", _UserBranch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserBranch = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_UserBranch);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _ExchangeRate }, Total = 1 });
        }


        [HttpPut("Id")]
        public async Task<IActionResult> Update(Int64 UserBranchId, UserBranch _UserBranch)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _UserBranch.ModifiedUser = HttpContext.Session.GetString("user");
                _UserBranch.ModifiedDate = DateTime.Now;
                var result = await _client.PutAsJsonAsync(baseadress + "api/UserBranch/Update", _UserBranch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserBranch = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);
                }
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    throw new Exception($"Ocurrio un error:{valorrespuesta}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_UserBranch);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _UserBranch }, Total = 1 });
        }


        [HttpDelete("Id")]
        public async Task<ActionResult<UserBranch>> Delete(UserBranch _UserBranch)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/UserBranch/Delete", _UserBranch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UserBranch = JsonConvert.DeserializeObject<UserBranch>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _UserBranch }, Total = 1 });
        }


















    }

}