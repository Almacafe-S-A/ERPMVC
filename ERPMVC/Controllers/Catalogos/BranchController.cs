using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    public class BranchController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public BranchController(ILogger<HomeController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }


        // GET: Branch
        [Authorize(Policy = "Administracion.Sucursales")]
        public async Task<ActionResult> Brach()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(() => View());
        }

        public async Task<ActionResult> BranchCustomer()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(()=> PartialView());
        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetBranchByCustomer([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<Branch> _Branch = new List<Branch>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Branch/GetBranchByCustomer/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta);
                }
                //else if(result.StatusCode== 401)
                //{
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }


            return _Branch.ToDataSourceResult(request);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetBranch([DataSourceRequest]DataSourceRequest request,BranchDTO _Branchp)
        {
            List<Branch> _branchs = new List<Branch>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                string apidir = "api/Branch/GetBranch";              
                var result = await _client.GetAsync(baseadress + apidir);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _branchs = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta);
             

                    //if(_Branchp.ServicioId==2 && _Branchp.CustomerId>0)
                    //{
                    //    _branchs = _branchs.Where(q => q.CustomerId == _Branchp.CustomerId).ToList();
                    //}
                    //else
                    //{
                    //    _branchs = _branchs.Where(q => q.CustomerId == null).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_branchs.ToDataSourceResult(request));

        }


        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetBranchActivos([DataSourceRequest] DataSourceRequest request, BranchDTO _Branchp)
        {
            List<Branch> _branchs = new List<Branch>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                string apidir = "api/Branch/GetBranch";
                var result = await _client.GetAsync(baseadress + apidir);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _branchs = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta).Where(w => w.IdEstado == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_branchs.ToDataSourceResult(request));
        }




        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Branch> _customers = new List<Branch>();

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
                    _customers = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta);
                    _customers = _customers.OrderByDescending(q => q.BranchId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
         


               return Json(_customers.ToDataSourceResult(request));

        }


        public async Task<ActionResult<Branch>> SaveBranch([FromBody]BranchDTO _BranchP)
        {
            List<Branch> Branch = new List<Branch>();

            Branch _Branch = _BranchP;
            try
            {
                Branch _listBranch = new Branch();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Branch/GetBranchByName/" + _Branch.BranchName);
                string valorrespuesta = "";
                if (_Branch.BranchId == 0)
                {
                    
                    _Branch.FechaModificacion = DateTime.Now;
                    _Branch.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        var result1 = await _client.GetAsync(baseadress + "api/Branch/GetBranch/");
                        string valorrespuesta1 = "";
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        Branch = JsonConvert.DeserializeObject<List<Branch>>(valorrespuesta1);

                        Branch = Branch.Where(q => q.BranchCode == _BranchP.BranchCode).ToList();
                        if (Branch.Count > 0)
                        {
                            return await Task.Run(() => BadRequest("Ya exíste una Sucursal con este código de sucursal"));
                        }


                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Branch = JsonConvert.DeserializeObject<Branch>(valorrespuesta);
                        if (_Branch == null)
                        {
                            _Branch = new Models.Branch();
                        }
                        if (_Branch.BranchId > 0)
                        {
                            return await Task.Run(() => BadRequest($"Ya existe una sucursal registrada para con ese nombre."));
                        }
                    }                 
                }


                 result = await _client.GetAsync(baseadress + "api/Branch/GetBranchById/" + _Branch.BranchId);
                 valorrespuesta = "";

                Branch _bc = new Branch();
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _bc = JsonConvert.DeserializeObject<Branch>(valorrespuesta);
                    if (_bc == null)
                    {
                        _bc = new Branch { CustomerId = 0 };
                    }
                    if(_bc.CustomerId==0 || _bc.CustomerId ==null)
                    {

                    }
                    else if(_bc.CustomerId!=_BranchP.CustomerId)
                    {
                        return await Task.Run(() => BadRequest($"Ya tiene asignado un cliente dicha sucursal, no se puede reasignar."));
                    }
                }


                if (_BranchP.BranchId == 0)
                {
                    //_Branch.FechaCreacion = DateTime.Now;
                    //_Branch.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _Branch.FechaCreacion = DateTime.Now;
                    _Branch.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BranchP);
                    var value = (insertresult.Result as ObjectResult).Value;
                    Branch resultado = ((Branch)(value));
                    if (resultado.BranchId <= 0)
                    {
                        return await Task.Run(() => BadRequest($"No se guardo la sucursal."));
                    }
                }
                else
                {

                    result = await _client.GetAsync(baseadress + "api/Branch/GetBranchById/" + _Branch.BranchId);
                    valorrespuesta = "";

                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Branch = JsonConvert.DeserializeObject<Branch>(valorrespuesta);

                        if (_Branch == null)
                        {
                            _Branch = new Models.Branch();
                        }
                    }
                    _BranchP.UsuarioCreacion = _Branch.UsuarioCreacion;
                    _BranchP.FechaCreacion = _Branch.FechaCreacion;
                    _BranchP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _BranchP.FechaModificacion = DateTime.Now;
                    var updateresult = await Update(_Branch.BranchId, _BranchP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_BranchP);
        }


        public async Task<ActionResult<Branch>> SaveBodegaHabilitada([FromBody] BranchDTO _BranchP)
        {
            List<Branch> Branch = new List<Branch>();

            Branch _Branch = _BranchP;
            try
            {
                Branch _listBranch = new Branch();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Branch/GetBranchByName/" + _Branch.BranchName);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<Branch>(valorrespuesta);
                    if (_Branch == null)
                    {
                        _Branch = new Branch();
                    }
                }
                if (_Branch.BranchId == 0)
                {
                    _Branch.FechaCreacion = DateTime.Now;
                    _Branch.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BranchP);
                    var value = (insertresult.Result as ObjectResult).Value;
                    Branch resultado = ((Branch)(value));
                    if (resultado.BranchId <= 0)
                    {
                        return await Task.Run(() => BadRequest($"No se guardo la sucursal."));
                    }
                }
                else
                {                    
                    _BranchP.UsuarioCreacion = _Branch.UsuarioCreacion;
                    _BranchP.FechaCreacion = _Branch.FechaCreacion;
                    _BranchP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _BranchP.FechaModificacion = DateTime.Now;
                    var updateresult = await Update(_Branch.BranchId, _BranchP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_BranchP);
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddBranch([FromBody]BranchDTO _sarpara)
        {
            BranchDTO _Branch = new BranchDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Branch/GetBranchById/" + _sarpara.BranchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<BranchDTO>(valorrespuesta);
                }
                if (_Branch == null)
                {
                    _Branch = new BranchDTO();
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_Branch);
        }


        
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddBranchCustomer([FromBody]BranchDTO _branchpara)
        {
            BranchDTO _Branch = new BranchDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Branch/GetBranchById/" + _branchpara.BranchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<BranchDTO>(valorrespuesta);
                }

                if (_Branch == null)
                {
                    _Branch = new BranchDTO { CustomerId = _branchpara.CustomerId };
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_Branch);
        }

        // POST: Branch/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Insert(Branch _Branch)
        public async Task<ActionResult<Branch>> Insert(Branch _Branch)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Branch.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Branch.FechaCreacion = DateTime.Now;
                _Branch.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Branch/Insert", _Branch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<Branch>(valorrespuesta);
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

            return Ok(_Branch);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Branch }, Total = 1 });
        }

        [HttpPut("BranchId")]
        public async Task<IActionResult> Update(Int64 BranchId, Branch _Branch)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Branch.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Branch.FechaModificacion = DateTime.Now;
                var result = await _client.PutAsJsonAsync(baseadress + "api/Branch/Update", _Branch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<Branch>(valorrespuesta);
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
            return Ok(_Branch);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _Branch }, Total = 1 });
        }

        [HttpDelete("BranchId")]
        public async Task<ActionResult<Branch>> Delete(Int64 BranchId, Branch _Branch)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Branch/Delete", _Branch);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Branch = JsonConvert.DeserializeObject<Branch>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Branch }, Total = 1 });
        }





    }
}