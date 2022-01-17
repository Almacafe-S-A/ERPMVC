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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.Controllers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ERPMVC.Controllers
{
    public class PrecioCafesController : Controller
    {


        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        private IHostingEnvironment _hostingEnvironment;
        public PrecioCafesController    (ILogger<PrecioCafesController> logger, 
            IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
            this._hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Policy = "Administracion.PrecioCafe")]
        public async Task<IActionResult> PrecioCafe()
        {
            PrecioCafeDTO precio = new PrecioCafeDTO();
            precio = await ObtenerCoinfiguracionCafe();
            
            ViewData["Tasacambio"] = await Obtenertasadecambio();
            ViewData["Clientes"] = await ObtenerClientes();
            return View(precio);
        }


        public async Task<IActionResult> pvwAddImage([FromBody] PrecioCafe precioCafe)
        {
            PrecioCafe precio = new PrecioCafe();
            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + $"api/PrecioCafe/GetPrecioCafeById/{precioCafe.Id}");
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await (result.Content.ReadAsStringAsync());
                precio = JsonConvert.DeserializeObject<PrecioCafe>(valorrespuesta);
            }
            else
            {
                return BadRequest();
            }


            return View(precio);
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PrecioCafe> _PrecioCafe = new List<PrecioCafe>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PrecioCafe/GetPrecioCafe");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PrecioCafe = JsonConvert.DeserializeObject<List<PrecioCafe>>(valorrespuesta);
                    _PrecioCafe = _PrecioCafe.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PrecioCafe.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetPrecioCafeByCustomer([DataSourceRequest] DataSourceRequest request, int clienteid)
        {
            List<PrecioCafe> _PrecioCafe = new List<PrecioCafe>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress +$"api/PrecioCafe/GetPrecioCafeByCustomer/{clienteid}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PrecioCafe = JsonConvert.DeserializeObject<List<PrecioCafe>>(valorrespuesta);
                    _PrecioCafe = _PrecioCafe.OrderByDescending(q => q.Id).ToList();
                }

                _PrecioCafe = (from precio in _PrecioCafe
                               select new PrecioCafe()
                               {
                                   Id=precio.Id,
                                   Descripcion =  precio.Fecha.Date.ToString("dd/MM/yyyy") + " -> Valor Cafe Oro "+precio.PrecioQQOro.ToString("C2") +
                                   " Valor Cafe Pregamino " + precio.PercioQQPergamino.ToString("C2") + " Valor Precio Calidades Inferiores" + precio.PrecioQQOro.ToString("C2")
                               }
                                ).ToList();


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PrecioCafe.ToDataSourceResult(request);

        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CustomerDocument>> SaveImage(IEnumerable<IFormFile> files, PrecioCafe precioCafe)
        {
            PrecioCafe precio = new PrecioCafe();
            IFormFile Image = files.FirstOrDefault();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/PrecioCafe/GetPrecioCafeById/{precioCafe.Id}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    precio = JsonConvert.DeserializeObject<PrecioCafe>(valorrespuesta);
                }
                else
                {
                    return BadRequest();
                }

                if (Image == null)
                {
                    return BadRequest("No se Adjunto ningun archivo de Imagen");

                }
                FileInfo info = new FileInfo(Image.FileName);
                if (!(info.Extension.Equals(".pdf") || info.Extension.Equals(".jpeg")
                    || info.Extension.Equals(".png") || info.Extension.Equals(".txt")))
                {
                    return BadRequest("Formato de Imagen No Válido");                   
                }
                string filename = precioCafe.Id + "_PrecioCafe" + info.Extension;
                var filePath = _hostingEnvironment.WebRootPath + "/PrecioCafeImg/" +filename;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                precio.ImgPrecioCafe = filePath;
                precio.ImgName = filename;

                var udpate = await Update(precio);



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(precioCafe);
        }


        [HttpPost("savepreciocafes")]
        public async Task<ActionResult<PrecioCafe>> savepreciocafes(PrecioCafeDTO _Preciocafes)
        {
            try
            {
                if (_Preciocafes.CustomerId == 0)
                {
                    return await Task.Run(() => BadRequest($"Por favor seleccione el cliente"));
                }
                if (_Preciocafes.ExchangeRateId == 0)
                {
                    return await Task.Run(() => BadRequest($"Por favor seleccione la tasa de cambio."));
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Preciocafes.FechaCreacion = DateTime.Now;
                _Preciocafes.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Preciocafes.FechaModificacion = DateTime.Now;
                _Preciocafes.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/PrecioCafe/Insert", _Preciocafes);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Preciocafes = JsonConvert.DeserializeObject<PrecioCafeDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest("Registro Duplicado");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Preciocafes);
        }

        


        [AcceptVerbs("Post")]
        public async Task<ActionResult<PrecioCafe>> Update(/*Int64 Id,*/ PrecioCafe _Preciocafes)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _Preciocafes.FechaCreacion = DateTime.Now;
                _Preciocafes.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Preciocafes.FechaModificacion = DateTime.Now;
                _Preciocafes.UsuarioModificacion = HttpContext.Session.GetString("user");
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/PrecioCafe/Update", _Preciocafes);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Preciocafes = JsonConvert.DeserializeObject<PrecioCafeDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest("Registro Duplicado");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Preciocafes }, Total = 1 });

        }

        //[HttpPost("Delete")]
        [AcceptVerbs("Post")]
        public async Task<ActionResult<PrecioCafe>> Delete(PrecioCafe _PrecioCafe)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PrecioCafe/Delete", _PrecioCafe);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PrecioCafe = JsonConvert.DeserializeObject<PrecioCafe>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _PrecioCafe }, Total = 1 });
        }


        async Task<IEnumerable<ExchangeRate>> Obtenertasadecambio()
        {
            IEnumerable<ExchangeRate> tasacambio = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRate");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    tasacambio = JsonConvert.DeserializeObject<IEnumerable<ExchangeRate>>(valorrespuesta);
                    
                    DateTime tasacambioactual = DateTime.Now;
                    tasacambio = tasacambio.Where(x => x.DayofRate.Date > DateTime.Now.AddDays(-4));


                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            tasacambio = (IEnumerable<ExchangeRate>)ExchangeRate.PreprocesarTasasCambio(tasacambio.ToList());
            //ViewData["tasa"] = tasacambio.FirstOrDefault() == null ? 0 : tasacambio.FirstOrDefault().ExchangeRateValueCompra;
            
            //ViewData["defaultasadecambio"] = tasacambio;
            return tasacambio;

        }

        

        async Task<PrecioCafeDTO> ObtenerCoinfiguracionCafe()
        {
            IEnumerable<ElementoConfiguracion> configuracion = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementosConfiguracionByGrupoConfiguracion/141");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    configuracion = JsonConvert.DeserializeObject<IEnumerable<ElementoConfiguracion>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            PrecioCafeDTO precio = new PrecioCafeDTO();
            if (precio!=null)
            {
                precio.PermisoExportacionUSD = configuracion.Where(q => q.Id == 189).FirstOrDefault().Valordecimal;
                precio.UtilidadUSD = configuracion.Where(q => q.Id == 188).FirstOrDefault().Valordecimal;
                precio.FideicomisoUSD = configuracion.Where(q => q.Id == 187).FirstOrDefault().Valordecimal;
                precio.BeneficiadoUSD = configuracion.Where(q => q.Id == 186).FirstOrDefault().Valordecimal;
                precio.PorcentajeIngreso = configuracion.Where(q => q.Id == 185).FirstOrDefault().Valordecimal;
                precio.PorcentajeConsumoInterno =configuracion.Where(q => q.Id == 190).FirstOrDefault().Valordecimal;


            }
            return precio;

        }


        async Task<IEnumerable<Customer>> ObtenerClientes()
        {
            IEnumerable<Customer> clientes = null;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomer");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    clientes = JsonConvert.DeserializeObject<IEnumerable<Customer>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return clientes;

        }


        [HttpGet]
        public async Task<JsonResult> Tasadecambio()
        {
            Int32 Existe = 0;
            DateTime cd = DateTime.Now;

            List<ExchangeRate> _ExchangeRate = new List<ExchangeRate>();
            try
            {

                
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRate");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<List<ExchangeRate>>(valorrespuesta);
                    _ExchangeRate = _ExchangeRate.Where(x => x.DayofRate.Date == DateTime.Now.Date).ToList();

                }

                if(_ExchangeRate.Count == 0)
                {
                    Existe = 0;
                }
                else
                {
                    Existe = 1;
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