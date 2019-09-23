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
        public async Task<ActionResult> GetAccountingByParentId(Accounting Cuenta)
        {
            List<Accounting> _childs = new List<Accounting>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error             
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetFathers/"+Cuenta.AccountId);
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

        public async Task<ActionResult> GetAccountingChildsByParentId(Accounting Cuenta)
        {
            List<AccountingChilds> _childs = new List<AccountingChilds>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //Error
                var result = await _client.GetAsync(baseadress + "api/AccountingChilds/GetAccountingChildsByParentId/" + Cuenta.AccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _childs = JsonConvert.DeserializeObject<List<AccountingChilds>>(valorrespuesta);

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
        public async Task<ActionResult> GetTreeAccounting(Int64 ParentAccountId,bool raiz) {
            List<NodeViewModel> items = new List<NodeViewModel>();

          

                //Es Padre Contable de todos los Padres e Hijos Contables
           if (raiz == true)
                {
                //Accounting Padres = //JsonConvert.SerializeObject<>(
                // );

                var Padre = await AccountingByTypeAccount(ParentAccountId);
                Accounting resultado = ((Accounting)Padre.Value);
               var Arbol= await GetTreeAccounting(resultado.AccountId, false);


             
                
            }
            else
                {
                var parametro = await GetAccountingById(ParentAccountId);

                Accounting _AccountingP = ((Accounting)parametro.Value);
                /*
                 *  var value = (resultsalesorder.Result as ObjectResult).Value;
                        SalesOrder resultado = ((SalesOrder)(value));
                       
                 */
                var root = new NodeViewModel
                {
                    Id = Convert.ToInt32(_AccountingP.AccountId),
                    Title = _AccountingP.AccountName
                };

                if (_AccountingP != null)
                {
                    items.Add(root);
                    var Padres = await GetAccountingByParentId(_AccountingP);


                    //var PadresConHijos=await GetTreeAccounting(_AccountingP.AccountId, false);
                    // 

                }
                else {

                    var Hijos = await GetAccountingChildsByParentId(_AccountingP);
                    root.Children.Add(new NodeViewModel {
                        Id = Convert.ToInt32(_AccountingP.AccountId),
                        Title = _AccountingP.AccountName });

                    //

                }

                // foreach (Accounting Cuenta in Padres) 

                //  Accounting Cuenta = JsonConvert.DeserializeObject<Accounting>(valorrespue);
                /* var root = new NodeViewModel
                 {
                     Id = Convert.ToInt32(Padres.a),
                     Title = Cuenta.AccountName
                 };
                 items.Add(root);
                 */

            }

            return Json(items);

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
        public async Task<ActionResult> Index()
        {
            var items = new List<NodeViewModel>();
            await GetTreeAccounting(1, true);
            this.ViewBag.Tree = items;

            // return View();
            return await Task.Run(() => View());
        }
        
        [HttpGet("[action]")]
        public async Task<JsonResult> GetAccount([DataSourceRequest]DataSourceRequest request)
        {
            List<Account> __customers = new List<Account>();
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
                    __customers = JsonConvert.DeserializeObject<List<Account>>(valorrespuesta);

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
            List<Account> _customers = new List<Account>();

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
                    _customers = JsonConvert.DeserializeObject<List<Account>>(valorrespuesta);

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
            List<Account> _customers = new List<Account>();

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
                    _customers = JsonConvert.DeserializeObject<List<Account>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }


        public async Task<ActionResult<Account>> SaveAccounting([FromBody]AccountDTO _Accounting)
        {
            Account _Account = _Accounting;
            try
            {
                Account _listAccount = new Account();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Accounting/GetAccountById/" + _Account.AccountId);
                string valorrespuesta = "";
                _Account.FechaModificacion = DateTime.Now;
                _Account.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Account>(valorrespuesta);
                }

                if (_Account == null) { _Account = new Models.Account(); }

                if (_Accounting.AccountId == 0)
                {
                    _Account.FechaCreacion = DateTime.Now;
                    _Account.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Accounting);
                }
                else
                {
                    _Accounting.UsuarioCreacion = _Account.UsuarioCreacion;
                    _Accounting.FechaCreacion = _Account.FechaCreacion;
                    var updateresult = await Update(_Account.AccountId, _Accounting);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Accounting);
        }
        

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddAccounting([FromBody]AccountDTO _sarpara)
        {
            AccountDTO _Account = new AccountDTO();
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
                    _Account = JsonConvert.DeserializeObject<AccountDTO>(valorrespuesta);

                }

                if (_Account == null)
                {
                    _Account = new AccountDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Account);

        }

        // POST: Account/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Account _Account)
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
                    _Account = JsonConvert.DeserializeObject<Account>(valorrespuesta);
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
        public async Task<IActionResult> Update(Int64 AccountId, Account _Account)
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
                    _Account = JsonConvert.DeserializeObject<Account>(valorrespuesta);
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