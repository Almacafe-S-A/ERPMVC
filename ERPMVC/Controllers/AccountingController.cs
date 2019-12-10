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
        //public async Task<IActionResult> SFAuxiliarMovimientos()
        //{
        //    return await Task.Run(() => View());

        //}
        [HttpGet]
        public ActionResult SFAuxiliarMovimientos(Int32 id)
        {

            AccountingDTO _accountingdto = new AccountingDTO { AccountId = id, }; //token = HttpContext.Session.GetString("token") };

            return View(_accountingdto);
        }
        public async Task<IActionResult> SFAuxiliarMovimientosPorDia()
        {
            return await Task.Run(() => View());

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
        async Task<IEnumerable<TypeAccount>> ObtenerTypeAccount()
        {
            IEnumerable<TypeAccount> typesAccount = null;
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
                    typesAccount = JsonConvert.DeserializeObject<IEnumerable<TypeAccount>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["defaultAccount"] = typesAccount.FirstOrDefault();
            return typesAccount;

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


        //[Authorize(Policy = "Admin")]
        public async Task<ActionResult> pvwTreeAccounting(Int64 TypeAccountId)
        {
            try
            {
                ViewData["TypeAccounts"] = await ObtenerTypeAccount();

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


        public async Task<JsonResult> AccountingRes([DataSourceRequest]DataSourceRequest request,AccountingFilter TypeAccount)
        {
            List<AccountingDTO> _accounting = new List<AccountingDTO>();
            //if (Estado == false)
            //{

                try
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.Timeout = TimeSpan.FromMinutes(15);
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/Accounting/GetAccountingType/", TypeAccount);//GetAccountingActive
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
                    __customers = (from c in __customers.OrderBy(q => q.AccountCode)
                                  select new Accounting
                                  {
                                      AccountId = c.AccountId,
                                      AccountName = c.AccountCode +"--" + c.AccountName,
                                      AccountCode = c.AccountCode,
                                      ParentAccountId = c.ParentAccountId,
                                      IsCash = c.IsCash,
                                      TypeAccountId = c.TypeAccountId,
                                      HierarchyAccount = c.HierarchyAccount,
                                      Description = c.Description,
                                      AccountBalance = c.AccountBalance,
                                      CompanyInfoId = c.CompanyInfoId,
                                      ChildAccounts = c.ChildAccounts,

                                  }
                                  ).ToList();

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
            List<Accounting> _accounting = new List<Accounting>();

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
                    _accounting = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);
                    _accounting = (from c in _accounting
                                   select new Accounting
                                   {
                                        AccountId = c.AccountId,
                                        AccountName = c.AccountCode + "--" +c.AccountName,
                                        AccountCode = c.AccountCode,
                                        Description = c.Description,
                                        Estado = c.Estado,
                                        IdEstado = c.IdEstado,
                                   }
                                   ).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_accounting.ToDataSourceResult(request));

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
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetNoChildAccounts");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);
                    _customers = _customers.Select(c => new Accounting
                                                        {
                                                            AccountId = c.AccountId,
                                                            AccountName = c.AccountCode + "--" + c.AccountName,
                                                            AccountCode = c.AccountCode,
                                                            Description = c.Description,
                                                            Estado = c.Estado,
                                                            IdEstado = c.IdEstado,
                                                        }).ToList();
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
            if (_AccountingP != null)
            {
                Accounting _Account = _AccountingP;
                _Account.CompanyInfoId = 1;

                try
                {

                    Accounting _listAccount = new Accounting();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _Account.ParentAccountId);
                    string valorrespuesta = "";
                    Accounting _acount = new Accounting();
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _acount = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);
                        if (_acount.AccountCode.Length >= _Account.AccountCode.Length)
                        {
                            return await Task.Run(() => BadRequest($"El código de la cuenta hija no puede ser inferior a su jerarquía(padre)! "));
                        }

                        for (int i = 0; i <= _acount.AccountCode.Length; i++)
                        {
                            if (_acount.AccountCode.Substring(0, i) != _Account.AccountCode.Substring(0, i))
                            {
                                return await Task.Run(() => BadRequest($"El código de la cuenta no es coincidente con su jerarquía(padre)! "));
                            }
                        }                      

                    }


                    _Account.FechaModificacion = DateTime.Now;
                    _Account.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _Account.AccountId);
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Account = JsonConvert.DeserializeObject<Accounting>(valorrespuesta);
                    }

                    if (_Account == null) { _Account = new Models.Accounting(); }

                    if (_AccountingP.AccountId == 0)
                    {
                        _Account.HierarchyAccount = HierarchyAccountLevel(_AccountingP.AccountCode);
                        if (CheckAccountCode(_AccountingP.AccountCode) == -1)
                        {
                            string error = await result.Content.ReadAsStringAsync();
                            return await Task.Run(() => BadRequest($"El número de caracteres del código de cuenta no es valido."));
                            /* return this.Json(new DataSourceResult
                             {
                                 Errors = $"Ocurrio un error: {error} El numero de caracteres del codigo de cuenta no es valido."
                             });*/
                        }
                        AccountingDTO _AccountDuplicated = new AccountingDTO();
                        // string baseadress = config.Value.urlbase;
                        HttpClient _client2 = new HttpClient();
                        _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var resultado = await _client.GetAsync(baseadress + "api/Accounting/GetAccountingByAccountCode/" + _AccountingP.AccountCode);
                        string valorrespuesta2 = "";

                        if (resultado.IsSuccessStatusCode)
                        {
                            valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                            _AccountDuplicated = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta2);

                        }
                        if (_AccountDuplicated != null)
                        {
                            if (_AccountDuplicated.AccountId != _AccountingP.AccountId)
                            {
                                string error = await result.Content.ReadAsStringAsync();
                                return await Task.Run(() => BadRequest($"El código de cuenta ya esta ingresado..."));
                            }
                            /* return this.Json(new DataSourceResult
                             {
                                 Errors = $"Ocurrio un error:{error} El codigo de cuenta ya esta ingresado."

                             });*/
                        }

                        _AccountingP.FechaCreacion = DateTime.Now;
                        _AccountingP.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_AccountingP);
                        var value = (insertresult.Result as ObjectResult).Value;
                        _AccountingP = ((AccountingDTO)(value));
                    }
                    else
                    {
                        AccountingDTO _AccountDuplicated = new AccountingDTO();
                        // string baseadress = config.Value.urlbase;
                        HttpClient _client2 = new HttpClient();
                        _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var resultado = await _client.GetAsync(baseadress + "api/Accounting/GetAccountingByAccountCode/" + _AccountingP.AccountCode);
                        string valorrespuesta2 = "";

                        if (resultado.IsSuccessStatusCode)
                        {
                            valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                            _AccountDuplicated = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta2);

                        }
                        if (_AccountDuplicated != null && _AccountingP.AccountId != _AccountDuplicated.AccountId)
                        {
                            string error = await result.Content.ReadAsStringAsync();

                            return await Task.Run(() => BadRequest($"El código de cuenta ya esta ingresado..."));

                            /*    return this.Json(new DataSourceResult
                                {
                                    Errors = $"Ocurrio un error:{error} El codigo de cuenta ya esta ingresado."

                                });*/
                        }

                        _AccountingP.UsuarioCreacion = _Account.UsuarioCreacion;
                        _AccountingP.FechaCreacion = _Account.FechaCreacion;
                        var updateresult = await Update(_Account.AccountId, _AccountingP);
                        var value = (updateresult.Result as ObjectResult).Value;
                        _AccountingP = ((AccountingDTO)(value));
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    return await Task.Run(() => BadRequest($"Ocurrio un error{ex.ToString()}"));
                    // throw ex;
                }
            }
            else
            {
                return await Task.Run(() => BadRequest($"No llego correctamente el modelo"));
            }

          
            return Ok(_AccountingP);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _AccountingP }, Total = 1 });
            //return Json(_Accounting);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddAccountingFather([FromBody]AccountingFatherDTO _sarpara)
        {
            AccountingFatherDTO _Account = new AccountingFatherDTO();
            AccountingDTO _Accountingfalse = new AccountingDTO();

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
                    _Accountingfalse = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta);

                }

                if (_Account == null)
                {
                    // AccountingDTO _AccountPadre = new AccountingDTO();
                    AccountingDTO _AccountParentId = new AccountingDTO();
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado2 = await _client2.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _sarpara.ParentAccountId);
                    var valorrespuesta2 = "";
                    if (resultado2.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado2.Content.ReadAsStringAsync());
                        _AccountParentId = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta2);

                    }
                    _Accountingfalse = new AccountingDTO();
                    _Account.ParentAccountId = _sarpara.ParentAccountId;
                    if (_AccountParentId == null) { }
                    else
                    {
                        _Account.TypeAccountIdPadre = _AccountParentId.TypeAccountId !=null?_AccountParentId.TypeAccountId.Value:0;
                    }
                    _Account = new AccountingFatherDTO();
                   
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
                     // AccountingDTO _AccountPadre = new AccountingDTO();
                    AccountingDTO _AccountParentId = new AccountingDTO();
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado2 = await _client2.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _sarpara.ParentAccountId);
                    var valorrespuesta2 = "";
                    if (resultado2.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado2.Content.ReadAsStringAsync());
                        _AccountParentId = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta2);

                    }
                    _Account = new AccountingDTO();
                    _Account.ParentAccountId = _sarpara.ParentAccountId;
                    if (_AccountParentId == null) { }
                    else { 
                    _Account.TypeAccountId = _AccountParentId.TypeAccountId;
                    }

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
        public async Task<ActionResult<AccountingDTO>> Insert(Accounting _Account)
        {
            AccountingDTO _accdto = new AccountingDTO();
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
                    _accdto = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta);
                }      
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    throw new Exception($"Occurio un error:{valorrespuesta}");
                }
            
               

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
                // return BadRequest($"Ocurrio un Error{ex.Message}");
            }

            return Ok(_accdto);
           // return new ObjectResult(new DataSourceResult { Data = new[] { _Account }, Total = 1 });
        }
      
        [HttpPut("AccountId")]
        public async Task<ActionResult<Accounting>> Update(Int64 AccountId, Accounting _Account)
        {
            AccountingDTO _accdto = new AccountingDTO();
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
                    _accdto = JsonConvert.DeserializeObject<AccountingDTO>(valorrespuesta);
                }
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    throw new Exception($"Occurio un error:{valorrespuesta}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
                // return BadRequest($"Ocurrio un Error{ex.Message}");
            }

            return Ok(_accdto);
           // return new ObjectResult(new DataSourceResult { Data = new[] { _Account }, Total = 1 });
        }
       
    }
}