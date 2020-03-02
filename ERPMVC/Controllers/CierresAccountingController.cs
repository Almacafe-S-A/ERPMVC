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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CierresAccountingController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CierresAccountingController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public async Task<JsonResult> AccountingByTypeAccount(Int64 TypeAccountId)
        {
            CierresAccounting _customers = new CierresAccounting();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error
                var result = await _client.GetAsync(baseadress + "api/CierresAccounting/GetFatherAccountById/" + TypeAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<CierresAccounting>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_customers);

        }
        public async Task<JsonResult> GetAccountingByParentId(CierresAccounting Cuenta)
        {
            List<CierresAccounting> _childs = new List<CierresAccounting>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error             
                var result = await _client.GetAsync(baseadress + "api/CierresAccounting/GetFathersAccounting/" + Cuenta.AccountId);
                string valorrespuesta = "";// GetFathersAccounting
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _childs = JsonConvert.DeserializeObject<List<CierresAccounting>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_childs);

        }


        public async Task<JsonResult> GetAccountingById(Int64 CierreAccountingId)
        {
            CierresAccounting _CierresAccounting = new CierresAccounting();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CierresAccounting/GetAccountById/" + CierreAccountingId);
                string valorrespuesta = "";
                _CierresAccounting.FechaModificacion = DateTime.Now;
                _CierresAccounting.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CierresAccounting = JsonConvert.DeserializeObject<CierresAccounting>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CierresAccounting);
        }
        // [HttpPost("[action]")]

        public async Task<JsonResult> GetTreeAccounting(Int64 ParentAccountId, bool raiz)
        {
            List<NodeViewModel> items = new List<NodeViewModel>();

            try
            {

                //Es Padre Contable de todos los Padres e Hijos Contables
                if (raiz == true)
                {
                    //Accounting Padres = //JsonConvert.SerializeObject<>(
                    // );

                    var Padre = await AccountingByTypeAccount(ParentAccountId);
                    if (Padre.Value != null)
                    {
                        CierresAccounting resultado = ((CierresAccounting)Padre.Value);
                        var Arbol = await GetTreeAccounting(resultado.CierreAccountingId, false);
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

                    CierresAccounting _AccountingP = ((CierresAccounting)parametro.Value);
                    /*
                     *  var value = (resultsalesorder.Result as ObjectResult).Value;
                            SalesOrder resultado = ((SalesOrder)(value));

                     */
                    string condicion = "";
                    if (_AccountingP.IsCash == true)
                    {
                        condicion = _AccountingP.AccountBalance.ToString("N4");
                    }
                    var root = new NodeViewModel
                    {
                        Id = _AccountingP.AccountId,

                        Title = _AccountingP.AccountName + "  " + condicion
                        // Balance = _AccountingP.AccountBalance
                    };

                    if (_AccountingP != null)
                    {
                        items.Add(root);
                        var CuentasPadres = await GetAccountingByParentId(_AccountingP);
                        List<CierresAccounting> ListaPadres = ((List<CierresAccounting>)CuentasPadres.Value);
                        foreach (CierresAccounting Cuenta in ListaPadres)
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
                            foreach (NodeViewModel nodo in PadreconHijosNodo)
                            {
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
        [HttpGet]
        public async Task<ActionResult> CatalagueAccounting(Int64 TypeAccountId)
        {
            if (TypeAccountId == 0)
            {
                List<NodeViewModel> items = new List<NodeViewModel>();
                this.ViewBag.Tree2 = items;
                var TypesAccounting2 = await GetTypeAccount();
                List<TypeAccount> TiposCuentas2 = ((List<TypeAccount>)TypesAccounting2.Value);
                this.ViewBag.ListTypeAccount2 = TiposCuentas2;
            }
            else
            {
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
        public async Task<ActionResult> pvwTreeCierresAccounting(Int64 TypeAccountId)
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


        public async Task<JsonResult> CierresAccountingRes([DataSourceRequest]DataSourceRequest request, CierresAccountingFilter TypeAccount)
        {
            List<CierresAccountingDTO> _accounting = new List<CierresAccountingDTO>();
            //if (Estado == false)
            //{

            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromMinutes(15);
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CierresAccounting/GetAccountingType/", TypeAccount);//GetAccountingActive
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _accounting = JsonConvert.DeserializeObject<List<CierresAccountingDTO>>(valorrespuesta);
                }

                if (_accounting == null)
                {
                    _accounting = new List<CierresAccountingDTO>();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_accounting.ToDataSourceResult(request));

        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAccount([DataSourceRequest]DataSourceRequest request)
        {
            List<CierresAccounting> __customers = new List<CierresAccounting>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error
                var result = await _client.GetAsync(baseadress + "api/CierresAccounting/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    __customers = JsonConvert.DeserializeObject<List<CierresAccounting>>(valorrespuesta);
                    __customers = (from c in __customers.OrderBy(q => q.AccountCode)
                                   select new CierresAccounting
                                   {
                                       //CierreAccountingId = c.CierreAccountingId,
                                       AccountId = c.AccountId,
                                       AccountName = c.AccountCode + "--" + c.AccountName,
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
            List<CierresAccounting> _accounting = new List<CierresAccounting>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CierresAccounting/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _accounting = JsonConvert.DeserializeObject<List<CierresAccounting>>(valorrespuesta);
                    _accounting = (from c in _accounting
                                   select new CierresAccounting
                                   {
                                       //CierreAccountingId = c.CierreAccountingId,
                                       AccountId = c.AccountId,
                                       AccountName = c.AccountCode + "--" + c.AccountName,
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
            List<CierresAccounting> _customers = new List<CierresAccounting>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.GetAsync(baseadress + "api/Accounting/GetNoChildAccounts"); Cambio de metodo a configuracion de bloqueo para diarios
                var result = await _client.GetAsync(baseadress + "api/CierresAccounting/GetAccountDiary");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<CierresAccounting>>(valorrespuesta);
                    _customers = _customers.Select(c => new CierresAccounting
                    {
                        //CierreAccountingId = c.CierreAccountingId,
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
        public Int64 HierarchyAccountLevel(string Accountcode)
        {
            Int64 Level = 0;
            if (Accountcode.Length > 3)
            {
                Level = 3 + (Accountcode.Length - 3) / 2;
            }
            else
            {
                Level = Accountcode.Length;
            }
            return Level;
        }
        public Int64 CheckAccountCode(string Accountcode)
        {
            Int64 Level = 0;
            if (Accountcode.Length > 3)
            {
                Level = Accountcode.Length % 2;
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
        
    }
}