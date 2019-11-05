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
    public class ControlPalletsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ControlPalletsController(ILogger<ControlPalletsController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexSalida()
        {

            return View();
        }

        [HttpGet]
        public ActionResult SFControlPallets(Int32 id)
        {
            try
            {
                ControlPalletsDTO _ControlPalletsDTO = new ControlPalletsDTO { ControlPalletsId = id, }; //token = HttpContext.Session.GetString("token") };

                return View(_ControlPalletsDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

        }




        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request,int ingreso=1)
        {
            var res = await GetControlPallets(ingreso);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values, int ingreso = 1)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetControlPallets(ingreso))
                {
                    if (values.Contains(order.ControlPalletsId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<ControlPallets>> GetControlPallets(int ingreso=1)
        {
            List<ControlPallets> _ControlPallets = new List<ControlPallets>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsNoSelected");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<List<ControlPallets>>(valorrespuesta);
                    
                    _ControlPallets = (from c in _ControlPallets
                                       .Where(q=>q.EsIngreso==ingreso)
                                       select new ControlPallets
                                       {
                                           ControlPalletsId = c.ControlPalletsId,
                                           CustomerName ="Id:"+c.ControlPalletsId +" || Control de ingresos:"+c.PalletId 
                                              + " || Nombre:" + c.CustomerName +" || Placa:"+c.Placa + " || Motorista:"+c.Motorista + " || Fecha: " + c.DocumentDate + " || Total:" + c.TotalSacos,
                                           DocumentDate = c.DocumentDate,

                                       }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _ControlPallets;
        }

        public async Task<ActionResult> GetControlPalletsById(Int64 Id)
        {
            ControlPallets _ControlPallets = new ControlPallets();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);

                }

                if (_ControlPallets == null)
                {
                    _ControlPallets = new ControlPallets();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ControlPallets);
        }


        public async Task<ActionResult> GetControlPalletsByIdCalculo(Int64 Id)
        {
            ControlPalletsDTO _ControlPallets = new ControlPalletsDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPalletsDTO>(valorrespuesta);
                }


                if (_ControlPallets == null)
                {
                    _ControlPallets = new ControlPalletsDTO();
                }

                _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                result = await _client.GetAsync(baseadress + "api/Boleto_Ent/GetBoleto_EntById/"+ _ControlPallets.WeightBallot);
                valorrespuesta = "";
                Boleto_Ent _Boleto_Ent = new Boleto_Ent();
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Boleto_Ent = JsonConvert.DeserializeObject<Boleto_Ent>(valorrespuesta);
                }

                if (_Boleto_Ent.Boleto_Sal != null)
                {
                    if (_Boleto_Ent.peso_e > _Boleto_Ent.Boleto_Sal.peso_n)
                    {
                        _ControlPallets.taracamion = Convert.ToDouble((_Boleto_Ent.peso_e - _Boleto_Ent.Boleto_Sal.peso_n) / Convert.ToDouble(100));
                    }
                    else if (_Boleto_Ent.peso_e < _Boleto_Ent.Boleto_Sal.peso_n)
                    {
                        _ControlPallets.taracamion = Convert.ToDouble((_Boleto_Ent.peso_e)) / Convert.ToDouble(100);
                    }

                    _ControlPallets.pesobruto = Math.Round(Convert.ToDouble(_Boleto_Ent.peso_e) / Convert.ToDouble(100),2, MidpointRounding.AwayFromZero);
                    _ControlPallets.pesoneto = Math.Round(Convert.ToDouble(_ControlPallets.pesobruto) - Convert.ToDouble(_ControlPallets.taracamion),2, MidpointRounding.AwayFromZero);
                    _ControlPallets._Boleto_Ent = _Boleto_Ent;
                    
                    double yute = Math.Round(Convert.ToDouble(_ControlPallets.TotalSacosYute * 1) / Convert.ToDouble(100), 2, MidpointRounding.AwayFromZero);
                    double polietileno = Math.Round(Convert.ToDouble((_ControlPallets.TotalSacosPolietileno * 0.5)) / Convert.ToDouble(100), 2, MidpointRounding.AwayFromZero);
                    double tarasaco = Math.Round(Math.Round(yute, 2) + Math.Round(polietileno, 2), 2, MidpointRounding.AwayFromZero);
                    _ControlPallets.Tara = tarasaco;
                    _ControlPallets.pesoneto2 = Convert.ToDouble(_ControlPallets.pesoneto) - Convert.ToDouble(tarasaco);
                }
                else
                {
                    return await Task.Run(() => BadRequest("No se ha completado esta boleta!, cierre el proceso"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ControlPallets);
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ControlPallets> _ControlPallets = new List<ControlPallets>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPallets");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<List<ControlPallets>>(valorrespuesta);
                    _ControlPallets = _ControlPallets.OrderByDescending(q => q.ControlPalletsId).ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ControlPallets.ToDataSourceResult(request);

        }

        [HttpGet]
        public async Task<DataSourceResult> GetSalidas([DataSourceRequest]DataSourceRequest request)
        {
            List<ControlPallets> _ControlPallets = new List<ControlPallets>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsSalida");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<List<ControlPallets>>(valorrespuesta);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ControlPallets.ToDataSourceResult(request);

        }




        [HttpPost("[action]")]
        public async Task<ActionResult> pvwControlPallets([FromBody]ControlPalletsDTO _ControlPalletsId)
        {
            ControlPalletsDTO _ControlPallets = new ControlPalletsDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + _ControlPalletsId.ControlPalletsId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPalletsDTO>(valorrespuesta);

                }

                if (_ControlPallets == null)
                {
                    _ControlPallets = new ControlPalletsDTO
                    {
                        DocumentDate = DateTime.Now,
                        editar = 1,
                        EsIngreso = _ControlPalletsId.EsIngreso
                   ,
                        BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                else
                {
                    _ControlPallets.editar = 0; _ControlPallets.EsIngreso = _ControlPalletsId.EsIngreso;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ControlPallets);

        }



        [HttpPost("[action]")]
        public async Task<ActionResult<ControlPallets>> SaveControlPallets([FromBody]dynamic dto)
       // public async Task<ActionResult<ControlPallets>> SaveControlPallets([FromBody]ControlPalletsDTO _ControlPalletsDTO)
        {
            ControlPalletsDTO _ControlPalletsDTO = new ControlPalletsDTO();
            try
            {
             
                _ControlPalletsDTO = JsonConvert.DeserializeObject<ControlPalletsDTO>(dto.ToString());
                if (_ControlPalletsDTO != null)
                {
                    ControlPallets _listControlPallets = new ControlPallets();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + _ControlPalletsDTO.ControlPalletsId);
                    string valorrespuesta = "";
                    _ControlPalletsDTO.FechaModificacion = DateTime.Now;
                    _ControlPalletsDTO.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);
                    }

                    if (_listControlPallets == null) { _listControlPallets = new ControlPallets(); }
                    if (_listControlPallets.ControlPalletsId == 0)
                    {
                        _ControlPalletsDTO.FechaCreacion = DateTime.Now;
                        _ControlPalletsDTO.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_ControlPalletsDTO);

                        var value = (insertresult.Result as ObjectResult).Value;
                        _ControlPalletsDTO = ((ControlPalletsDTO)(value));
                        if (_ControlPalletsDTO.ControlPalletsId > 0)
                        {

                        }
                        else
                        {
                            return await Task.Run(() => BadRequest("No se genero correctamente el control!"));
                        }

                    }
                    else
                    {
                        //var updateresult = await Update(_ControlPalletsDTO.ControlPalletsId, _ControlPalletsDTO);
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ControlPalletsDTO);
        }

        // POST: ControlPallets/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ControlPalletsDTO>> Insert(ControlPalletsDTO _ControlPallets)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ControlPallets.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ControlPallets.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlPallets/Insert", _ControlPallets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPalletsDTO>(valorrespuesta);
                }
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    throw new Exception($"Ocurrio un error: {valorrespuesta}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
                //return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_ControlPallets);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPallets }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ControlPallets>> Update(Int64 id, ControlPallets _ControlPallets)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ControlPallets/Update", _ControlPallets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_ControlPallets);
            //  return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPallets }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ControlPallets>> Delete([FromBody]ControlPallets _ControlPallets)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlPallets/Delete", _ControlPallets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPallets }, Total = 1 });
        }





    }
}