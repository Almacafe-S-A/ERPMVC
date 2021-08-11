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
            try
            {
                decimal totalciflpsitems = 0;
                foreach (var item in liquidacionLines)
                {
                    var totalCIF = total / item.TotalFOB * totalfob;
                    item.TotalCIB = totalCIF;
                    item.TotalCIFLPS = totalCIF * _Liquidacion.TasaCambio;
                    var totalciflps = totalCIF * _Liquidacion.TasaCambio;

                    item.ValorDerechosImportacion = item.TotalCIFLPS * _Liquidacion.DerechosImportacion;
                    item.TotalCIFDerechosImp = item.ValorDerechosImportacion + item.TotalCIFLPS;
                    item.ValorSelectivoConsumo = item.TotalCIFDerechosImp * _Liquidacion.SelectivoConsumo;
                    item.OtrosImpuestos = 0;
                    item.TotalImpuestoVentas = (item.TotalCIFDerechosImp+ item.ValorSelectivoConsumo) * _Liquidacion.ImpuestoSobreVentas;
                    item.TotalDerechosmasImpuestos = item.TotalCIFLPS + item.ValorDerechosImportacion+item.OtrosImpuestos+item.TotalImpuestoVentas + item.ValorSelectivoConsumo;
                    item.TotalFinal = item.TotalCIFLPS + item.TotalDerechosmasImpuestos;
                    totalciflpsitems += item.TotalCIFLPS;
                }
                    foreach (var item in liquidacionLines)
                {
                    item.OtrosImpuestos = (_Liquidacion.Otros / totalciflpsitems)* item.TotalCIFLPS ;
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
