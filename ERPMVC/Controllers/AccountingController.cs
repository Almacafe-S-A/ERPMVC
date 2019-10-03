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
    public class AccountingController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public AccountingController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        public async Task<JsonResult> AccountingByTypeAccount(Int64 TypeAccountId)
        {
            Accounting _customers = new Accounting();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetFatherAccountById/"+TypeAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_customers);

        }
        public async Task<JsonResult> GetAccountingByParentId(Accounting Cuenta)
        {
            List<Accounting> _childs = new List<Accounting>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error             
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetFathersAccounting/" + Cuenta.AccountId);
                string valorrespuesta = "";// GetFathersAccounting
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _childs = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_childs);

        }

        
        public async Task<JsonResult> GetAccountingById(Int64 AccountId)
        {
            Accounting _Accounting  = new Accounting();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountById/" + AccountId);
                string valorrespuesta = "";
                _Accounting.FechaModificacion = DateTime.Now;
                _Accounting.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Accounting = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);
                }

                

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Accounting);
        }
        // [HttpPost("[action]")]

        public async Task<JsonResult> GetTreeAccounting(Int64 ParentAccountId,bool raiz) {
            List<NodeViewModel> items = new List<NodeViewModel>();

            try { 

                //Es Padre Contable de todos los Padres e Hijos Contables
           if (raiz == true)
                {
                //Accounting Padres = //JsonConvert.SerializeObject<>(
                // );

                var Padre = await AccountingByTypeAccount(ParentAccountId);
                    if (Padre.Value != null)
                    {
                        Accounting resultado = ((Accounting)Padre.Value);
                        var Arbol = await GetTreeAccounting(resultado.AccountId, false);
                        List<NodeViewModel> ArbolNodo = ((List<NodeViewModel>)Arbol.Value);
                        items = ArbolNodo;
                    }
               /* foreach (NodeViewModel nodo in ArbolNodo)
                {
                    items.Add(nodo);
                }*/



            }
            else
                {
                var parametro = await GetAccountingById(ParentAccountId);

                Accounting _AccountingP = ((Accounting)parametro.Value);
                    /*
                     *  var value = (resultsalesorder.Result as ObjectResult).Value;
                            SalesOrder resultado = ((SalesOrder)(value));

                     */
                    string condicion="";
                    if (_AccountingP.IsCash == true)
                    {
                       condicion = _AccountingP.AccountBalance.ToString("N4");
                    }
             var root = new NodeViewModel
                {
                    Id = _AccountingP.AccountId,
                    
                    Title = _AccountingP.AccountName +"  "+condicion
                    // Balance = _AccountingP.AccountBalance
                };
                
                if (_AccountingP != null)
                {
                    items.Add(root);
                    var CuentasPadres = await GetAccountingByParentId(_AccountingP);
                    List<Accounting> ListaPadres = ((List<Accounting>)CuentasPadres.Value);
                    foreach (Accounting Cuenta in ListaPadres)
                    {
                        var PadresConHijos = await GetTreeAccounting(Cuenta.AccountId, false);
                        var PadreconHijosNodo = ((List<NodeViewModel>)PadresConHijos.Value);
                        /*var Hijos = await GetAccountingChildsByParentId(Cuenta);
                        List<AccountingChilds> ListaHijos = ((List<AccountingChilds>)Hijos.Value);
                            foreach (AccountingChilds Childs in ListaHijos) {
                        PadreconHijosNodo.Children.Add(new NodeViewModel
                        {
                            Id = Convert.ToInt32(Childs.AccountingChildsId),
                            Title = _AccountingP.AccountName
                        });
                        }*/
                        foreach (NodeViewModel nodo in PadreconHijosNodo) {
                            root.Children.Add(nodo);

                            //  root.Children.Add
                            //      (nodo);
                        }
                    }
                   
                  }
                

            
            }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(items);

        }


        



        public async Task<JsonResult> GetTypeAccount()
        {
            List<TypeAccount> _TypeAccount = new List<TypeAccount>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeAccount/GetTypeAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeAccount = JsonConvert.DeserializeObject<List<TypeAccount>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TypeAccount);

        }
        /*
        
            // return Json(__customers.ToDataSourceResult(request));


            var items = new List<NodeViewModel>();
            foreach (Accounting Cuenta in __customers) {
                var root = new NodeViewModel { Id = Convert.ToInt32(Cuenta.AccountId),
                                                Title = Cuenta.AccountName };
                items.Add(root);
                foreach (AccountingChilds CuentaChield in __childs)
                {

                    root.Children.Add(new NodeViewModel { Id = Convert.ToInt32(CuentaChield.AccountingChildsId), Title = CuentaChield.AccountName});
                }
            }


            //foreach (Accounting in )

            // root.Children.Add(new NodeViewModel { Id = 2, Title = "One" });
            // root.Children.Add(new NodeViewModel { Id = 3, Title = "Two" });

            this.ViewBag.Tree = items;

             */
        // GET: Accounting
        /*[HttpGet]
        public async Task<ActionResult> Index()
        {
          
                this.ViewBag.Tree = new List<NodeViewModel>();
                var TypesAccounting = await GetTypeAccount();
                List<TypeAccount> TiposCuentas = ((List<TypeAccount>)TypesAccounting.Value);
                this.ViewBag.ListTypeAccount = TiposCuentas;
                return await Task.Run(() => View());
            
            
        }*/
        [HttpGet]
        public async Task<ActionResult> CatalagueAccounting(Int64 TypeAccountId)
        {
            if (TypeAccountId == 0) {
                List<NodeViewModel> items = new List<NodeViewModel>();
                this.ViewBag.Tree2 = items;
                var TypesAccounting2 = await GetTypeAccount();
                    List<TypeAccount> TiposCuentas2 = ((List<TypeAccount>)TypesAccounting2.Value);
                    this.ViewBag.ListTypeAccount2 = TiposCuentas2;
            }
            else {
                var TypesAccounting2 = await GetTypeAccount();
                List<TypeAccount> TiposCuentas2 = ((List<TypeAccount>)TypesAccounting2.Value);
                this.ViewBag.ListTypeAccount2 = TiposCuentas2;
                var arbol = await GetTreeAccounting(TypeAccountId, true);
                List<NodeViewModel> items = ((List<NodeViewModel>)arbol.Value);
                this.ViewBag.Tree2 = items;

            }

            return await Task.Run(() => View());

        }


        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var TypesAccounting = await GetTypeAccount();
            List<TypeAccount> TiposCuentas = ((List<TypeAccount>)TypesAccounting.Value);
            this.ViewBag.ListTypeAccount = TiposCuentas;

           
            return await Task.Run(() => View());

        }


        
        public async Task<ActionResult> pvwTreeAccounting(Int64 TypeAccountId)
        {
            try
            {
                ViewData["TypeAccounts"] = await GetTypeAccount();

                var arbol = await GetTreeAccounting(TypeAccountId, true);
                List<NodeViewModel> items = ((List<NodeViewModel>)arbol.Value);
                this.ViewBag.Tree = items;
                var TypesAccounting = await GetTypeAccount();
                List<TypeAccount> TiposCuentas = ((List<TypeAccount>)TypesAccounting.Value);
                this.ViewBag.ListTypeAccount = TiposCuentas;
                this.ViewBag.AccountingParameter = TypeAccountId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => PartialView());
            //return PartialView(_Vendor);

        }


        public async Task<JsonResult> AccountingRes([DataSourceRequest]DataSourceRequest request, TypeAccount TypeAccount)
        {
            List<AccountingDTO> _accounting = new List<AccountingDTO>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromMinutes(15);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));              
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountingType/" + TypeAccount.TypeAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _accounting = JsonConvert.DeserializeObject<List<AccountingDTO>>(valorrespuesta);

                }

                if (_accounting == null)
                {
                    _accounting = new List<AccountingDTO>();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            // return Json(_accounting, JsonRequestBehavior.AllowGet);
            return Json(_accounting.ToTreeDataSourceResult(request));

        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAccount([DataSourceRequest]DataSourceRequest request)
        {
            List<Accounting> __customers = new List<Accounting>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    __customers = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(__customers.ToDataSourceResult(request));

        }
        [HttpGet]
        public async Task<JsonResult> GetAccounting([DataSourceRequest]DataSourceRequest request)
        {
            List<Accounting> _customers = new List<Accounting>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }
        [HttpGet]
        public async Task<JsonResult> GetAccountingDiary([DataSourceRequest]DataSourceRequest request)
        {
            List<Accounting> _customers = new List<Accounting>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountDiary");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }
        public Int64 HierarchyAccountLevel(string Accountcode) {
            Int64 Level = 0;
            if (Accountcode.Length > 3)
            {
                Level = 3 + (Accountcode.Length - 3) / 2;
            }
            else {
                Level = Accountcode.Length;
            }
            return Level;
        }
        public Int64 CheckAccountCode(string Accountcode)
        {
            Int64 Level = 0;
            if (Accountcode.Length > 3)
            {
                Level = Accountcode.Length %  2;
                if (Level == 0)
                {
                    Level = -1;
                }
            }
            else
            {
                Level = Accountcode.Length;
            }
            return Level;
        }


        [HttpPost]
        public async Task<ActionResult> SaveAccounting([FromBody]AccountingDTO _AccountingP)
        {
            Accounting _Account = _AccountingP;
            _Account.CompanyInfoId = 1;

             
            try
            {
                Accounting _listAccount = new Accounting();
                string baseadress = config.Value.urlbase;
                 HttpClient _client = new HttpClient();
                 _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _Account.AccountId);
                string valorrespuesta = "";
                _Account.FechaModificacion = DateTime.Now;
                _Account.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    _Account.HierarchyAccount = HierarchyAccountLevel(_AccountingP.AccountCode);
                    if (CheckAccountCode(_AccountingP.AccountCode) == -1)
                    {
                        string error = await result.Content.ReadAsStringAsync();
                         return  this.Json(new DataSourceResult
                             {
                             Errors = $"Ocurrio un error: : El numero de caracteres del codigo de cuenta no es valido."
                          } );
                    }



                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);
                }

                if (_Account == null) { _Account = new Models.Accounting(); }

                if (_AccountingP.AccountId == 0)
                {
                    _AccountingP.FechaCreacion = DateTime.Now;
                    _AccountingP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_AccountingP);
                }
                else
                {
                    _AccountingP.UsuarioCreacion = _Account.UsuarioCreacion;
                    _AccountingP.FechaCreacion = _Account.FechaCreacion;
                    var updateresult = await Update(_Account.AccountId, _AccountingP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return new ObjectResult(new DataSourceResult { Data = new[] { _AccountingP }, Total = 1 });
           
            //return Json(_Accounting);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddAccounting([FromBody]AccountingDTO _sarpara)
        {
            AccountingDTO _Account = new AccountingDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _sarpara.AccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta);

                }

                if (_Account == null)
                {
                    _Account = new AccountingDTO();
                    _Account.ParentAccountId = _sarpara.ParentAccountId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => PartialView(_Account));

           // return PartialView(_Account);

        }

        // POST: Account/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Accounting _Account)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Account.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Account.FechaCreacion = DateTime.Now;
                _Account.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Accounting/Insert", _Account);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Account }, Total = 1 });
        }
      
        [HttpPut("AccountId")]
        public async Task<IActionResult> Update(Int64 AccountId, Accounting _Account)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Accounting/Update", _Account);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Account }, Total = 1 });
        }
       
    }
}