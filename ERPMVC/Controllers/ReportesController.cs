using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Reportes.Kardex detallado por producto")]
        public async  Task<IActionResult> SFDetalladoPorProducto()
        {

            return await Task.Run(()=> View());

        }

        [Authorize(Policy = "Reportes.Ingresos")]
        public async Task<IActionResult> SFIngresos()
        {
            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Reportes.Recibo de mercaderia")]
        public async Task<IActionResult> SFReciboMercaderia()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Reportes.Areas")]
        public async Task<IActionResult> SFAreas()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Reportes.Tabla de Conversion de Peso")]
        public async Task<IActionResult> SFTabladeConversionPeso()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Contabilidad.Seguros.Reporte Valores Fisicos")]
        public async Task<IActionResult> SFDetalleValorSeguros()
        {

            return await Task.Run(() => View());

        }


        [Authorize(Policy = "Inventarios.Reportes.Guias Emision Emitidas")]
        public async Task<IActionResult> SFGuiasRemisionEmitidas()
        {

            return await Task.Run(() => View());

        }


        [Authorize(Policy = "Inventarios.Reportes.Movimientos Diarios")]
        public async Task<IActionResult> SFMovimientosDiarios()
        {

            return await Task.Run(() => View());

        }






    }
}