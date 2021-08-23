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

        public ActionResult<Liquidacion> ValidarDetalle([FromBody] Liquidacion _Liquidacion)
        {
            List<LiquidacionLine> liquidacionLines = _Liquidacion.detalleliquidacion;
            decimal totalfob = _Liquidacion.detalleliquidacion.Sum(s => s.TotalFOB);
            decimal total = totalfob + _Liquidacion.Seguro + _Liquidacion.Otros + _Liquidacion.Flete;
            decimal isv = _Liquidacion.ImpuestoSobreVentas/100;
            decimal derecchosImportacion = _Liquidacion.DerechosImportacion/100;
            decimal selectivoConsumo =  _Liquidacion.SelectivoConsumo/100;


            try
            {
                decimal totalciflpsitems = 0;
                foreach (var item in liquidacionLines)
                {
                    if (item.Cantidad ==0 )
                    {
                        return BadRequest($"La Cantidad en factura del item {item.SubProductName} no puede ser cero");
                    }
                    var totalCIF = +total / totalfob * item.TotalFOB;
                    item.TotalCIB = totalCIF;
                    item.TotalCIFLPS = totalCIF * _Liquidacion.TasaCambio;
                    var totalciflps = totalCIF * _Liquidacion.TasaCambio;

                    //item.ValorDerechosImportacion = item.TotalCIFLPS * derecchosImportacion;
                    //item.TotalCIFDerechosImp = item.ValorDerechosImportacion + item.TotalCIFLPS;
                    //item.ValorSelectivoConsumo = item.TotalCIFDerechosImp * selectivoConsumo;
                    //item.OtrosImpuestos = 0;
                    //item.TotalImpuestoVentas = (item.TotalCIFDerechosImp+ item.ValorSelectivoConsumo) * isv;
                    //item.TotalDerechosmasImpuestos = item.ValorDerechosImportacion+item.OtrosImpuestos+item.TotalImpuestoVentas + item.ValorSelectivoConsumo;
                    //item.TotalDerechos = item.TotalCIFLPS + item.TotalDerechosmasImpuestos;
                    //if (_Liquidacion.ProductId == 2)
                    //{
                    //    item.PrecioUnitarioCIF = item.TotalCIFLPS / item.Cantidad;
                    //}
                    //else
                    //{
                    //    item.PrecioUnitarioCIF = item.TotalFinal / item.Cantidad;
                    //}
                    
                    //item.ValorUnitarioDerechos = item.TotalDerechosmasImpuestos / item.Cantidad;
                    
                    
                    //item.ValorTotalCIF = (decimal)item.PrecioUnitarioCIF * item.CantidadRecibida;
                    //item.ValorTotalDerechos = item.ValorUnitarioDerechos * item.CantidadRecibida;
                    //item.TotalFinal = (decimal)item.ValorTotalCIF +  (decimal)item.ValorTotalDerechos;
                    //totalciflpsitems += item.TotalCIFLPS;
                }
                totalciflpsitems = _Liquidacion.detalleliquidacion.Sum(s => s.TotalCIFLPS);
                    foreach (var item in liquidacionLines)
                {
                    item.ValorDerechosImportacion = item.TotalCIFLPS * derecchosImportacion;
                    item.TotalCIFDerechosImp = item.ValorDerechosImportacion + item.TotalCIFLPS;
                    item.ValorSelectivoConsumo = item.TotalCIFDerechosImp * selectivoConsumo;
                    item.OtrosImpuestos = ((decimal)_Liquidacion.TotalOtrosImpuestos / totalciflpsitems)* item.TotalCIFLPS ;
                    item.TotalImpuestoVentas = (item.TotalCIFDerechosImp + item.ValorSelectivoConsumo) * isv;
                    item.TotalDerechosmasImpuestos = item.ValorDerechosImportacion + item.OtrosImpuestos + item.TotalImpuestoVentas + item.ValorSelectivoConsumo;
                    item.TotalDerechos = item.TotalCIFLPS + item.TotalDerechosmasImpuestos;
                    decimal valortotalderechos = item.ValorTotalDerechos == null ? 0 : (decimal)item.ValorTotalDerechos;
                    if (_Liquidacion.ProductId == 2)
                    {
                        item.ValorUnitarioDerechos = item.TotalDerechosmasImpuestos / item.Cantidad;
                        item.ValorTotalDerechos = item.ValorUnitarioDerechos * item.CantidadRecibida;
                        item.PrecioUnitarioCIF = item.TotalCIFLPS / item.Cantidad;
                        item.ValorTotalCIF = (decimal)item.PrecioUnitarioCIF * item.CantidadRecibida; ;
                        item.TotalFinal = (decimal)item.ValorTotalDerechos + (decimal)item.ValorTotalCIF;
                    }
                    else
                    {
                        item.PrecioUnitarioCIF = item.TotalDerechos / item.Cantidad;
                        item.TotalFinal = (decimal)item.PrecioUnitarioCIF * item.CantidadRecibida;
                        item.ValorTotalCIF = (decimal)item.TotalCIFLPS + (decimal)item.TotalDerechosmasImpuestos;
                    }

                    item.ValorUnitarioDerechos = item.TotalDerechosmasImpuestos / item.Cantidad;


                    
                    item.ValorTotalDerechos = item.ValorUnitarioDerechos * item.CantidadRecibida;
                    
                    //totalciflpsitems += item.TotalCIFLPS;
                }
            }
            catch (DivideByZeroException)
            {

                return BadRequest("No se permiten Total FOB con valor 0");
            } catch (Exception ex) {

                return BadRequest(ex.ToString());
            }
            
            _Liquidacion.detalleliquidacion = liquidacionLines;
            _Liquidacion.Total = total;

            return _Liquidacion;



        }
    }
}
