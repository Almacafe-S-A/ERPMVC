using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
using System.Security.Claims;

namespace ERPMVC.Controllers.Inventarios
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class LiquidacionDetalleController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public LiquidacionDetalleController(ILogger<LiquidacionController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
        }
        public async Task<DataSourceResult> LiquidacionesPendientes([DataSourceRequest] DataSourceRequest request, [FromQuery(Name = "Recibos")] int[] recibos, [FromQuery(Name = "Id")] int id)
        {
            List<LiquidacionLine> liquidacionLines = new List<LiquidacionLine>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                string requestURl;
                if (id == 0)
                {
                    string strrecibos = "?";
                    foreach (var item in recibos)
                    {
                        strrecibos += $"Recibos={item}";
                        if (item != recibos.ElementAt(recibos.Count() - 1))
                        {
                            strrecibos += "&&";
                        }
                    }
                    requestURl = $"api/Liquidaciones/GetLiquidacionesPendientesporCliente{strrecibos}";
                }
                else
                {
                    requestURl = $"api/Liquidaciones/LiquidacionDetalle/{id}";
                }



                var result = await _client.GetAsync(baseadress + requestURl);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    liquidacionLines = JsonConvert.DeserializeObject<List<LiquidacionLine>>(valorrespuesta);
                    liquidacionLines = liquidacionLines.OrderByDescending(e => e.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return liquidacionLines.ToDataSourceResult(request);



        }

        public async Task<List<LiquidacionLine>> Update([FromBody] Liquidacion _Liquidacion)
        {
            List<LiquidacionLine> liquidacionLines = _Liquidacion.detalleliquidacion;
            decimal totalfob = liquidacionLines.Sum(s => s.TotalFOB);
            decimal total = totalfob + _Liquidacion.Seguro + _Liquidacion.Otros + _Liquidacion.Flete;
            liquidacionLines = (from liquidacions in liquidacionLines
                                select new LiquidacionLine() {
                                    Id = liquidacions.Id,
                                    GoodsReceivedLine = liquidacions.GoodsReceivedLine,
                                    SubProductId = liquidacions.SubProductId,
                                    SubProductName = liquidacions.SubProductName,
                                    Cantidad = liquidacions.Cantidad,
                                    TotalFOB = liquidacions.TotalFOB,
                                    TotalCIB =  total / (totalfob * liquidacions.TotalFOB) ,
                                    TotalCIFLPS = total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio,
                                    ValorDerechosImportacion = total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio * _Liquidacion.DerechosImportacion,
                                    TotalCIFDerechosImp = (total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio * _Liquidacion.DerechosImportacion) +
                                        total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio,
                                    ValorSelectivoConsumo = ((total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio * _Liquidacion.DerechosImportacion) +
                                        total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio) * _Liquidacion.SelectivoConsumo,
                                    OtrosImpuestos = 0,
                                    TotalImpuestoVentas = total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio * _Liquidacion.DerechosImportacion,
                                    TotalDerechosmasImpuestos = (total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio * _Liquidacion.DerechosImportacion) +
                                        total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio + (((total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio * _Liquidacion.DerechosImportacion) +
                                        total / (totalfob * liquidacions.TotalFOB) * _Liquidacion.TasaCambio) * _Liquidacion.SelectivoConsumo),
                                    //TotalFinal = 



            return liquidacionLines;



        }
    }
}
