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

        [Authorize(Policy = "Inventarios.Reportes.Movimientos Boletas de Peso")]
        public async Task<IActionResult> SFMovimientosBoletasPeso()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Movimientos Detalle Boletas de Peso")]
        public async Task<IActionResult> SFMovimientosDetalleBoletasPeso()
        {

            return await Task.Run(() => View());

        }
        
        [Authorize(Policy = "Inventarios.Reportes.Recibos de Mercadería Emitidos Detallado")]
        public async Task<IActionResult> SFRecibosMercaderiaDetallado()
        {

            return await Task.Run(() => View());

        }
        
        [Authorize(Policy = "Inventarios.Reportes.Recibos de Mercadería por Certificar")]
        public async Task<IActionResult> SFRecibosMercaderiaCertificar()
        {

            return await Task.Run(() => View());

        }
        
        [Authorize(Policy = "Inventarios.Reportes.Solicitudes de Deposito Emitidas Detallado")]
        public async Task<IActionResult> SFSolicitudesDepositoDetallado()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Certificados de Deposito Emitidos Detallado")]
        public async Task<IActionResult> SFCertificadosDepositoDetallado()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Endosos Registrados por Cliente")]
        public async Task<IActionResult> SFEndososRegistradosxCliente()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Endosos Registrados por Banco")]
        public async Task<IActionResult> SFEndososRegistradosxBanco()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Autorizaciones de Retiro Emitidas Detallado")]
        public async Task<IActionResult> SFAutorizacionRetiroDetallado()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Comprobante de Entrega Emitidos Detallado")]
        public async Task<IActionResult> SFComprobanteEntregaDetallado()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Boletas de Salida Emitidas Detallado")]
        public async Task<IActionResult> SFBoletasSalidaDetallado()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Saldos de Certificados de Deposito")]
        public async Task<IActionResult> SFSaldosCD()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Saldos de Autorizaciones de Retiro")]
        public async Task<IActionResult> SFSaldosAR()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Kardex de Producto x Bodega")]
        public async Task<IActionResult> SFKardexProductoxBodega()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Kardex de Producto x Estiba")]
        public async Task<IActionResult> SFKardexBodegaxEstiba()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Kardex Detallado x Producto")]
        public async Task<IActionResult> SFKardexDetalladoxProducto()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Kardex x CD")]
        public async Task<IActionResult> SFKardexCD()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Kardex x AR")]
        public async Task<IActionResult> SFKardexAR()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Inventarios.Reportes.Movimientos Diarios")]
        public async Task<IActionResult> SFMovimientosDiarios()
        {

            return await Task.Run(() => View());

        }
        [Authorize(Policy = "Monitoreo.Reportes.Cumplimiento")]
        public async Task<IActionResult> SFReporteTransacciones()
        {

            return await Task.Run(() => View());

        }


        [Authorize(Policy = "Inventarios.Reportes.Servicios de Bodega")]
        public async Task<IActionResult> SFServicioBodega()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Monitoreo.Reportes")]
        public async Task<IActionResult> SFClientes()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Monitoreo.Reportes")]
        public async Task<IActionResult> SFClientesTransacciones()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Monitoreo.Reportes")]
        public async Task<IActionResult> SFClientesRelacion()
        {

            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Monitoreo.Reportes")]
        public async Task<IActionResult> SFClientesAltasyBajas()
        {

            return await Task.Run(() => View());

        }



        

        //[Authorize(Policy = "Ventas.SFClienteEstadoCuenta")]
        public async Task<IActionResult> SFClienteEstadoCuenta()
        {

            return await Task.Run(() => View());

        }


    }
}