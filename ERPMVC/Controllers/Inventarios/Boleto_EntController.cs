using System;
using System.Collections.Generic;
using System.Data.Odbc;
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
    public class Boleto_EntController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public Boleto_EntController(ILogger<Boleto_EntController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        //[HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("[controller]/[action]")]
        public async Task<ActionResult> pvwBoleto_Ent([FromBody]Boleto_Ent boleto_Ent )
        {
            Boleto_Ent _Boleto_Ent = new Boleto_Ent();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntById/" + boleto_Ent.clave_e);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);

                }

                if (_Boleto_Ent == null||_Boleto_Ent.clave_e == 0)
                {
                    _Boleto_Ent = new Boleto_Ent();
                    _Boleto_Ent.Boleto_Sal = new Boleto_Sal();
                    _Boleto_Ent.Boleto_Sal.peso_n = 0;
                    _Boleto_Ent.Boleto_Sal.peso_s = 0;
                    _Boleto_Ent.fecha_e = DateTime.Now;
                    _Boleto_Ent.clave_e = 0;
                    _Boleto_Ent.PesoLibrasEspañolas = true;
                    _Boleto_Ent.CapturaAutomatica = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Boleto_Ent);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest] DataSourceRequest request)
        {
            List<Boleto_Ent> _Boleto_Ent = new List<Boleto_Ent>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_Ent");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<List<Boleto_Ent>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Boleto_Ent.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetBoleto_EntById([DataSourceRequest] DataSourceRequest request, Boleto_EntDTO _Boleto_Entp)
        {
            Boleto_EntDTO _Boleto_Ent = new Boleto_EntDTO();
            try
            {

                string baseadress = config.Value.urlbase;
                SubProduct _subproduct = new SubProduct();
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntById/" + _Boleto_Entp.clave_e);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_EntDTO>(valorrespuesta);

                    if (_Boleto_Ent == null)
                    {
                        _Boleto_Ent = new Boleto_EntDTO();
                        return Json(_Boleto_Ent);
                    }

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/SubProduct/GetSubProductByCodigoBalanza/" + _Boleto_Ent.clave_p);
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _subproduct = JsonConvert.DeserializeObject<SubProduct>(valorrespuesta);




                        if (_subproduct != null)
                        {


                            int ProductAsignado = 0;
                            var ProductoAsignadoCliente = await GetProductbyCsutomer(_subproduct.SubproductId, _Boleto_Entp.Customer);
                            ProductAsignado = Convert.ToInt32(ProductoAsignadoCliente.Value);

                            if (ProductAsignado == 0)
                            {
                                return Json(ProductAsignado);
                            }
                            else
                            {
                                _Boleto_Ent.ProductId = _subproduct.SubproductId;
                                _Boleto_Ent.UnitOfMeasureId = _subproduct.UnitOfMeasureId == null ? 0 : _subproduct.UnitOfMeasureId.Value;
                            }

                        }

                        if (_subproduct == null)
                        {
                            var NoContieneProducto = "NoContienProducto";

                            return Json(NoContieneProducto);
                        }



                    }


                }

                if (_Boleto_Ent == null) { _Boleto_Ent = new Boleto_EntDTO(); }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Boleto_Ent); //_Pesaje.ToDataSourceResult(request);

        }


        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request, Customer _customerp, bool esIngreso)
        {
            //var res = await GetBoletaEntrada(_customerp);
            bool completo = true;
            List<Boleto_Ent> _Boleto_Ent = new List<Boleto_Ent>();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + $"api/Boleto_Ent/GetBoletasdePesoByCustomer/{_customerp.CustomerId}/{esIngreso}/{completo}");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                _Boleto_Ent = JsonConvert.DeserializeObject<List<Boleto_Ent>>(valorrespuesta);
                _Boleto_Ent = (from c in _Boleto_Ent
                                            //.Where(q => q.clave_C == _customer.CustomerRefNumber)
                                            // .Where(q => q.Boleto_Sal == null)
                                            //.Where(q => q.completo == false)
                                            .OrderByDescending(q => q.clave_e)
                               select new Boleto_Ent
                               {
                                   clave_e = c.clave_e,
                                   observa_e = "No.:" + c.clave_e
                                            + "|| Placas:" + c.placas
                                           + "  || Conductor:" + c.conductor
                                           + "|| Fecha:" + c.fecha_e.ToString("dd/MM/yyyy")
                                           //+ "|| ProductoCod:" + c.clave_p
                                           + "|| Producto: " + c.NombreProducto,
                                   Boleto_Sal = c.Boleto_Sal,
                                   peso_e = c.peso_e
                                   //CustomerId = c.CustomerId,
                               }).ToList();
            }
            return Json(_Boleto_Ent.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values, Customer _customerp)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetBoletaEntrada(_customerp))
                {
                    if (values.Contains(order.clave_e))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<Boleto_Ent>> GetBoletaEntrada(Customer _customerp)
        {
            List<Boleto_Ent> _Boleto_Ent = new List<Boleto_Ent>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_Ent");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<List<Boleto_Ent>>(valorrespuesta);

                    Customer _customer = new Customer();

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerById/" + _customerp.CustomerId);
                    valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _customer = JsonConvert.DeserializeObject<Customer>(valorrespuesta);
                    }

                    if (_customer != null)
                    {
                        if (_customerp.CustomerTypeId == 1)
                        {
                            //Boleto_Sal.peso_s > resultados.peso_e
                            _Boleto_Ent = (from c in _Boleto_Ent
                                           .Where(q => q.clave_C == _customer.CustomerRefNumber)
                                            // .Where(q => q.Boleto_Sal == null)
                                            .Where(q => q.completo == false)
                                            .OrderByDescending(q => q.clave_e)
                                           select new Boleto_Ent
                                           {
                                               clave_e = c.clave_e,
                                               observa_e = "Placas:" + c.placas + " || Boleta de peso No.:" + c.clave_e + "  || Conductor:" + c.conductor + "|| Fecha:" + c.fecha_e + "|| Hora:" + c.hora_e,
                                               Boleto_Sal = c.Boleto_Sal,
                                               peso_e = c.peso_e
                                               //CustomerId = c.CustomerId,
                                           }).ToList();

                            _client = new HttpClient();
                            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                            List<Int64> _boletapeso = new List<long>();
                            result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPallets/");
                            if (result.IsSuccessStatusCode)
                            {

                                valorrespuesta = await (result.Content.ReadAsStringAsync());
                                List<ControlPallets> _controlpallets = JsonConvert.DeserializeObject<List<ControlPallets>>(valorrespuesta);
                                _boletapeso = _controlpallets.Select(q => q.WeightBallot).ToList();
                                _Boleto_Ent = _Boleto_Ent.Where(q => !_boletapeso.Contains(q.clave_e)).ToList();

                            }

                            _Boleto_Ent = _Boleto_Ent.Where(q => q.Boleto_Sal.peso_s < q.peso_e).ToList();
                        }
                        else
                        {
                            _Boleto_Ent = (from c in _Boleto_Ent
                                            //.Where(q => q.Boleto_Sal != null)
                                            .Where(q => q.completo == false)
                                            .Where(q => q.clave_C == _customer.CustomerRefNumber)
                                             //.Where(q => q.Boleto_Sal.peso_s > q.peso_e)
                                             .OrderByDescending(q => q.clave_e)
                                           select new Boleto_Ent
                                           {
                                               clave_e = c.clave_e,
                                               observa_e = "Placas:" + c.placas + " || Boleta de peso No.:" + c.clave_e + "  || Conductor:" + c.conductor + "|| Fecha:" + c.fecha_e + "|| Hora:" + c.hora_e,
                                               Boleto_Sal = c.Boleto_Sal,
                                               peso_e = c.peso_e
                                               //CustomerId = c.CustomerId,
                                           }).ToList();

                            _Boleto_Ent = _Boleto_Ent.Where(q => q.Boleto_Sal.peso_s > q.peso_e).ToList();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return _Boleto_Ent;
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult<int>> GetPesoBascula() {
            string peso = "";
            Listener listener = new Listener();
            int pesoobtenido = 0;
            if (config.Value.SimuladorBascula ==1)
            {
                peso = SimuladorBascula();
                pesoobtenido = Convert.ToInt32(peso);
            }
            else
            {
                try
                {
                    peso = await listener.ClienteTcpLectura(config.Value.IpBascula, config.Value.PuertoBascula);
                    peso = peso.Trim();
                    peso = peso.Split('+', StringSplitOptions.None)[1];
                    pesoobtenido = Convert.ToInt32(peso);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error:valor: [{peso}]+ { ex.ToString() }");
                    return await Task.Run(() => BadRequest("Error al Conectar con Bascula"));
                }
                
            }

             

            return pesoobtenido;
        }

        private string SimuladorBascula() {
            Random rd = new Random();
            int rand_num = rd.Next(100, 800);
            string peso = "";
            peso = "ST,GS,+" + rand_num.ToString().PadLeft(7, '0');
            peso = peso.Split('+', StringSplitOptions.None)[1];
            return peso;

        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> SFBoletaPeso(int id)
         {
            Boleto_Ent boleto_Ent = new Boleto_Ent();
            try
            {

                boleto_Ent.clave_e = id;


                return View(boleto_Ent);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Boleto_Ent>> SaveBoleto_Ent([FromBody]Boleto_Ent _Boleto_Ent)
        {

            try
            {
                Boleto_Ent _listBoleto_Ent = new Boleto_Ent();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntById/" + _Boleto_Ent.clave_e);
                string valorrespuesta = "";
               // _Boleto_Ent.FechaModificacion = DateTime.Now;
                //_Boleto_Ent.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listBoleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

                if(_listBoleto_Ent==null)
                {
                    _listBoleto_Ent = new Boleto_Ent();
                }

                if (_listBoleto_Ent.clave_e == 0)
                {
                    //_Boleto_Ent.FechaCreacion = DateTime.Now;
                   // _Boleto_Ent.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Boleto_Ent);
                }
                else
                {
                    var updateresult = await Update(_Boleto_Ent.clave_e, _Boleto_Ent);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Boleto_Ent);
        }

        // POST: Boleto_Ent/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Boleto_Ent>> Insert(Boleto_Ent _Boleto_Ent)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _Boleto_Ent.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_Boleto_Ent.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Ent/Insert", _Boleto_Ent);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Boleto_Ent);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Boleto_Ent }, Total = 1 });
        }

        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<Boleto_Ent>> Update(Int64 id, Boleto_Ent _Boleto_Ent)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Boleto_Ent/Update", _Boleto_Ent);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Boleto_Ent }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Boleto_Ent>> Delete([FromBody]Boleto_Ent _Boleto_Ent)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Boleto_Ent/Delete", _Boleto_Ent);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Boleto_Ent }, Total = 1 });
        }


        [HttpGet]
        public async Task<JsonResult> GetProductbyCsutomer( Int64 Subprodut, Int64 Customer)
        {
            Int32 Existe = 0;

            CustomerProduct Contiene = new CustomerProduct();            

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
               
                Contiene.CustomerId = Customer;
                Contiene.SubProductId = Subprodut;
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerProduct/GetProductbyCsutomer", Contiene);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    Existe = JsonConvert.DeserializeObject<Int32>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(Existe);
        }


    }
}