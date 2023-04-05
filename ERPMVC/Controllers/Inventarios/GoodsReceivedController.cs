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
    public class GoodsReceivedController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public GoodsReceivedController(ILogger<GoodsReceivedController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Recibo de Mercaderia")]
        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwGoodsReceived([FromBody]GoodsReceived _GoodsReceived)
        {
            //GoodsReceived _GoodsReceived = new GoodsReceived();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceived.GoodsReceivedId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);

                }

                if (_GoodsReceived == null)
                {
                    _GoodsReceived = new GoodsReceived { DocumentDate = DateTime.Now, OrderDate = DateTime.Now 
                        ,BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsReceived);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsReceived> _GoodsReceived = new List<GoodsReceived>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceived");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _GoodsReceived = _GoodsReceived.OrderByDescending(q => q.GoodsReceivedId).ToList();
                    _GoodsReceived = (from c in _GoodsReceived
                                      select new GoodsReceived
                                      {
                                          GoodsReceivedId= c.GoodsReceivedId,
                                          BranchId = c.BranchId,
                                          BranchName = c.BranchName,
                                          ControlId = c.ControlId,
                                          CustomerId = c.CustomerId,
                                          CustomerName = c.CustomerName,
                                          Motorista = c.Motorista,
                                          DocumentDate = Convert.ToDateTime(c.DocumentDate.ToString("dd/MM/yyyy")),
                                          Comments = c.Comments,
                                          BoletaSalidaId = c.BoletaSalidaId,
                                          FechaCreacion = c.FechaCreacion,
                                          FechaModificacion = c.FechaModificacion,
                                          OrderDate = Convert.ToDateTime(c.OrderDate.ToString("dd/MM/yyyy")),
                                          CountryName = c.CountryName,
                                          Marca = c.Marca,
                                          VigilanteName = c.VigilanteName,
                                          WeightBallot = c.WeightBallot,
                                          TaraUnidadMedida = c.TaraCamion,
                                          Placa = c.Placa,
                                          ProductId = c.ProductId,
                                          ProductName = c.ProductName,
                                          TaraCamion = c.TaraCamion,
                                          PesoNeto = c.PesoNeto,
                                          CustomerUnitOfMeasure = c.CustomerUnitOfMeasure,
                                          PesoNeto2= c.PesoNeto2,

                                          //QQPesoNeto = c.QQPesoNeto,
                                          //TotalSacos = c.TotalSacos,
                                          //UnitOfMeasureName = c.UnitOfMeasureName,
                                          UsuarioCreacion = c.UsuarioCreacion,
                                          WarehouseName = c.WarehouseName,
                                          SubProductName = c.SubProductName,
                                          UsuarioModificacion = c.UsuarioModificacion,






                                      }).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceived.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> RecibosPendientesCertificar([DataSourceRequest] DataSourceRequest request, 
            [FromQuery(Name = "clienteid")] int clienteid, 
            [FromQuery(Name = "servicioid")] int servicioid,
            [FromQuery(Name = "escafe")] int escafe, 
            [FromQuery(Name = "sucursal")] int sucursal)
        {
            List<GoodsReceived> _GoodsReceived = new List<GoodsReceived>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/GoodsReceived/RecibosPendientesCertificar/{clienteid}/{servicioid}/{escafe}/{sucursal}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _GoodsReceived = _GoodsReceived.OrderByDescending(q => q.GoodsReceivedId).ToList();
                    _GoodsReceived = (from recibos in _GoodsReceived
                                      select new GoodsReceived() {
                                          GoodsReceivedId = recibos.GoodsReceivedId,
                                          Comments = recibos.Comments,
                                      }).ToList();

                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceived.ToDataSourceResult(request);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> RecibosPendientesLiquidar([DataSourceRequest] DataSourceRequest request,
            [FromQuery(Name = "clienteid")] int clienteid,
            [FromQuery(Name = "servicioid")] int servicioid)
        {
            List<GoodsReceived> _GoodsReceived = new List<GoodsReceived>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/GoodsReceived/RecibosPendientesLiquidar/{clienteid}/{servicioid}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _GoodsReceived = _GoodsReceived.OrderByDescending(q => q.GoodsReceivedId).ToList();
                    _GoodsReceived = (from recibos in _GoodsReceived
                                      select new GoodsReceived()
                                      {
                                          GoodsReceivedId = recibos.GoodsReceivedId,
                                          Comments = recibos.GoodsReceivedId + " - " + recibos.Comments,
                                      }).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceived.ToDataSourceResult(request);

        }

        public ActionResult SFGoodsReceived(Int32 id)
        {

            GoodsReceived _GoodsReceived = new GoodsReceived { GoodsReceivedId = id, }; //token = HttpContext.Session.GetString("token") };

            return View(_GoodsReceived);
        }



        public ActionResult SFPorCertificar()
        {

            return View();
        }

        public ActionResult SFEmitidosDetalle()
        {

            return View();
        }


        /// <summary>
        /// Posible
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>


        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request,GoodsReceived _gr)
        {
           
            var res = await GetGoodsReceived(_gr);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values, GoodsReceived _gr)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetGoodsReceived(_gr))
                {
                    if (values.Contains(order.GoodsReceivedId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        [HttpGet("[controller]/[action]")]
        private async Task<List<GoodsReceived>> GetGoodsReceived( GoodsReceived _gr)
        {
            List<GoodsReceived> _GoodsReceiveds = new List<GoodsReceived>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedNoSelected");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceiveds = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _GoodsReceiveds = (from c in _GoodsReceiveds
                                       .Where(q => q.CustomerId == _gr.CustomerId)
                                        .Where(q => q.ProductId == _gr.ProductId)
                                       select new GoodsReceived
                                       {
                                           GoodsReceivedId = c.GoodsReceivedId,
                                           ProductName = " No. Documento:" + c.GoodsReceivedId + " || Cliente:" + c.CustomerName+ " || Tipo de servicio:" + c.ProductName +" || Fecha: " + c.DocumentDate ,
                                           //DocumentDate = c.DocumentDate,
                                           CustomerId = c.CustomerId,
                                       }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _GoodsReceiveds;
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<List<GoodsReceivedLine>>> AgruparRecibos([FromBody]GoodsReceivedParams _params)
        {
            List<GoodsReceivedLine> _GoodsReceived = new List<GoodsReceivedLine>();
            if(_params!=null)
            if (_params.RecibosSeleccionados != null)
            {
            
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/AgruparRecibos", _params.RecibosSeleccionados);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(valorrespuesta);
                      
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            else
            {

            }

            return Json(_GoodsReceived);
        }


      




        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetGoodsReceivedById([DataSourceRequest]DataSourceRequest request,[FromBody] GoodsReceived _GoodsReceivedId)
        {
            GoodsReceived _GoodsReceived = new GoodsReceived();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceivedId.GoodsReceivedId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_GoodsReceived);

        }

        GoodsReceived ToGoodsReceived(dynamic dto) {
            try
            {
                GoodsReceived goodsReceived = new GoodsReceived
                {
                    GoodsReceivedId = dto.GoodsReceivedId,
                    CustomerId = dto.CustomerId,
                    CustomerName = dto.CustomerName,
                    ControlId = dto.ControlId,
                    CountryId = dto.CountryId,
                    VigilanteId = dto.VigilanteId,
                    VigilanteName = dto.VigilanteName,
                    CountryName = dto.CountryName,
                    BranchId = dto.BranchId,
                    BranchName = dto.BranchName,
                    WarehouseId = dto.WarehouseId,
                    WarehouseName = dto.WarehouseName,
                    ProductId = dto.ProductId,
                    ProductName = dto.ProductName,
                    SubProductId = dto.SubProductId,
                    SubProductName = dto.SubProductName,
                    OrderDate = dto.OrderDate,
                    DocumentDate = dto.DocumentDate,
                    Motorista = dto.Motorista,
                    BoletaSalidaId = dto.BoletaSalidaId,
                    Placa = dto.Placa,
                    Marca = dto.Marca,
                    WeightBallot = dto.WeightBallot,
                    PesoBruto = dto.PesoBruto,
                    TaraTransporte = dto.TaraTransporte,
                    TaraCamion = dto.TaraCamion,
                    PesoNeto = dto.PesoNeto,
                    TaraUnidadMedida = dto.TaraUnidadMedida,
                    PesoNeto2 = dto.PesoNeto2,
                    Comments = dto.Comments,
                    FechaCreacion = dto.FechaCreacion,
                    FechaModificacion = dto.FechaModificacion,
                    UsuarioCreacion = dto.UsuarioCreacion,
                    UsuarioModificacion = dto.UsuarioModificacion,
                    _GoodsReceivedLine = dto._GoodsReceivedLine,




                };

                return goodsReceived;
            }
            catch (Exception ex )
            {

                throw;
            }
           




            
        
        
        }


        [HttpPost("[controller]/[action]")]
       public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody] GoodsReceived dto)
        {
            try
            {

                //GoodsReceived _GoodsReceived = ToGoodsReceived(dto);
                GoodsReceived _GoodsReceived = dto;

                if (_GoodsReceived == null)
                {
                    return BadRequest("No llego correctamente el modelo!");                   
                }

                GoodsReceived _listGoodsReceived = new GoodsReceived();
                // _listGoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(_GoodsReceived.ToString());
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceived.GoodsReceivedId);
                string valorrespuesta = "";
                _GoodsReceived.FechaModificacion = DateTime.Now;
                _GoodsReceived.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

                if (_listGoodsReceived == null)
                {
                    _listGoodsReceived = new GoodsReceived();
                }

                if (_listGoodsReceived.GoodsReceivedId == 0)
                {
                    _GoodsReceived.FechaCreacion = DateTime.Now;
                    _GoodsReceived.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GoodsReceived);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _GoodsReceived = ((GoodsReceived)(value));
                    if (_GoodsReceived.GoodsReceivedId == 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero el documento!"));
                    }

                    return Ok(_GoodsReceived);
                }
                else
                {
                    //var updateresult = await Update(_GoodsReceived.GoodsReceivedId, _GoodsReceived);
                }

                return Ok(_GoodsReceived);



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //throw ex;
                return BadRequest("Error al Guardar");
            }

        }

        // POST: GoodsReceived/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsReceived>> Insert(GoodsReceived _GoodsReceived)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsReceived.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsReceived.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/Insert", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_GoodsReceived);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsReceived>> Update(Int64 id, GoodsReceived _GoodsReceived)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsReceived/Update", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsReceived>> Delete([FromBody]GoodsReceived _GoodsReceived)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/Delete", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }





    }

    public class GoodsReceivedParams
    {
        public Int64[] values { get; set; }

        public Int64 CustomerId { get; set; } 

        public List<Int64> RecibosSeleccionados { get; set; }

    }

    


}