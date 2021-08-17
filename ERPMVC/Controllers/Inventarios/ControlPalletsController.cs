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
    public class ControlPalletsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public ControlPalletsController(ILogger<ControlPalletsController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Control de Ingresos")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [Authorize(Policy = "Inventarios.Control de Salida")]
        public IActionResult IndexSalida()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<ControlPallets>> SFControlPallets(Int32 id)
        {
            ControlPallets _ControlPallets = new ControlPallets();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPallets/GetControlPalletsById/" + id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<ControlPallets>(valorrespuesta);

                }
                

                return View(_ControlPallets);

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
                                           CustomerName ="Control Ingreso No.:"+c.ControlPalletsId 
                                           //+" || Control de ingresos:"+c.PalletId 
                                              + " || Cliente:" + c.CustomerName +" || Placa:"+c.Placa + " || Motorista:"+c.Motorista + " || Fecha: " + c.DocumentDate.ToString("dd/MM/yyyy") + " || Total Sacos:" + c.TotalSacos,
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

                if (_Boleto_Ent != null )
                {
                    if (_Boleto_Ent.Boleto_Sal == null)
                    {
                        return await Task.Run(() => BadRequest("No se ha completado esta boleta!, cierre el proceso"));
                    }
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
                    return Json(_ControlPallets);
                    // return await Task.Run(() => BadRequest("No se encontro la boleta de peso, cierre el proceso"));
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
                        EsIngreso = _ControlPalletsId.EsIngreso,
                        EsSalida = _ControlPalletsId.EsSalida,
                        ProductoPesado = true,
                   
                        BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                else
                {
                    //_ControlPallets.editar = 0; _ControlPallets.EsIngreso = _ControlPalletsId.EsIngreso;
                    if (_ControlPalletsId.EsSalida == 1)
                    {
                        _ControlPallets.editar = 0; _ControlPallets.EsSalida = _ControlPalletsId.EsSalida;
                    }
                    else
                    {
                        _ControlPallets.editar = 0; _ControlPallets.EsIngreso = _ControlPalletsId.EsIngreso;
                    }
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ControlPallets);

        }

        private ControlPalletsDTO ToControlPallets(dynamic dto) {
                ControlPalletsDTO controlPallets = new ControlPalletsDTO {
                ControlPalletsId = dto.ControlPalletsId,
                BranchId = dto.BranchId,
                BranchName = dto.BranchName,
                WeightBallot = dto.WeightBallot,
                QQPesoBruto = dto.QQPesoBruto,
                QQPesoFinal = dto.QQPesoFinal==null ? 0 : dto.QQPesoFinal,
                QQPesoNeto = dto.QQPesoNeto,
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                SubProductName = dto.SubProductId == null ? "": dto.SubProductName,
                SubProductId = dto.SubProductId == null? 0 : dto.SubProductId,
                DescriptionProduct = dto.DescriptionProduct,
                DocumentDate = dto.DocumentDate,
                EsIngreso = dto.EsIngreso,
                EsSalida = dto.EsSalida,
                PalletId = dto.PalletId,
                Placa = dto.Placa,
                Marca = dto.Marca,
                //SacosDevueltos = dto.SacosDevueltos,
                SubTotal = dto.SubTotal==null ? 0 : dto.SubTotal,
                TotalSacos = dto.TotalSacos,
                Motorista = dto.Motorista,
                Tara = dto.Tara,
                TotalSacosPolietileno = dto.TotalSacosPolietileno,
                TotalSacosYute = dto.TotalSacosYute,
                UnitOfMeasureId = dto.UnitOfMeasureId == null? 0 : dto.UnitOfMeasureId,
                UnitOfMeasureName = dto.UnitOfMeasureName,
                WarehouseId = dto.WarehouseId,
                WarehouseName = dto.WarehouseName,
                Observaciones = dto.Observaciones,
                ProductoPesado = dto.ProductoPesado,
                _Boleto_Ent = dto._Boleto_Ent
            };
            controlPallets._ControlPalletsLine = new List<ControlPalletsLine>();
            foreach (var item in dto._ControlPalletsLine)
            {
                controlPallets._ControlPalletsLine.Add(new ControlPalletsLine
                {
                    ControlPalletsId = item.ControlPalletsId,
                    SubProductId = item.SubProductId,
                    SubProductName = item.SubProductName,
                    WarehouseName = item.WarehouseName,
                    WarehouseId= item.WarehouseId,
                    Alto = item.Alto,
                    Ancho = item.Ancho,
                    cantidadPoliEtileno = item.cantidadPoliEtileno,
                    cantidadYute = item.cantidadYute,
                    ControlPalletsLineId = item.ControlPalletsLineId,
                    Observacion = item.Observacion,
                    Otros = item.Otros,
                    Qty = item.Qty,
                    UnitofMeasureId = item.UnitofMeasureId,
                    UnitofMeasureName = item.UnitofMeasureName,
                    Totallinea = item.Totallinea,
                    

                    

                }) ;
            }
            
            //controlPallets._ControlPalletsLine = dto._ControlPalletsLine;
            return controlPallets;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<ControlPallets>> SaveControlPallets([FromBody]dynamic dto)
        {
            ControlPalletsDTO _ControlPalletsDTO = new ControlPalletsDTO();
            try
            {
                
                _ControlPalletsDTO = ToControlPallets(dto);
                if (_ControlPalletsDTO != null)
                {
                    ControlPallets _listControlPallets = new ControlPallets();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    if (_ControlPalletsDTO.ControlPalletsId == 0)
                    {
                        _ControlPalletsDTO.FechaCreacion = DateTime.Now;
                        _ControlPalletsDTO.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_ControlPalletsDTO);
                        //_ControlPalletsDTO = ((ControlPalletsDTO)(value));
                        if (insertresult.Result is BadRequestResult)
                        {
                            return await Task.Run(() => BadRequest(insertresult.Result.ToString()));
                        }
                    }
                    else
                    {
                        _ControlPalletsDTO.FechaModificacion = DateTime.Now;
                        _ControlPalletsDTO.UsuarioModificacion = HttpContext.Session.GetString("user");
                        var insertresult = await Update(_ControlPalletsDTO);
                        //_ControlPalletsDTO = ((ControlPalletsDTO)(value));
                        if (insertresult.Result is BadRequestResult)
                        {
                            return await Task.Run(() => BadRequest(insertresult.Result.ToString()));
                        }
                    }             
                }
                else
                {
                    return await Task.Run(() => BadRequest("Datos no guardados!"));
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
                if (!result.IsSuccessStatusCode)
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_ControlPallets);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ControlPallets>> Update( ControlPallets _ControlPallets)
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



        [HttpGet]
        private async Task<List<ControlPallets>> GetControlPalletbyIdandCustomerid(int BoletaId, int CustomerId)
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
                                       .Where(q => q.EsIngreso == 1)
                                       select new ControlPallets
                                       {
                                           ControlPalletsId = c.ControlPalletsId,
                                           CustomerName = "Id:" + c.ControlPalletsId + " || Control de ingresos:" + c.PalletId
                                              + " || Nombre:" + c.CustomerName + " || Placa:" + c.Placa + " || Motorista:" + c.Motorista + " || Fecha: " + c.DocumentDate + " || Total:" + c.TotalSacos,
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



    }
}