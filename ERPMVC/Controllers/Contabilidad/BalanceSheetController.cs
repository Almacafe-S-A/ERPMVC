using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Authorize]
    [CustomAuthorization]
    public class BalanceSheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Contabilidad.Estados Finacieros.Estado de Resultado")]
        public async Task<IActionResult> SFBalance()
        {
            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Contabilidad.Estados Finacieros.Balance de Saldos")]
        public async Task<IActionResult> BalanceSaldos()
        {
            return await Task.Run(() => View());

        }

        [Authorize(Policy = "Contabilidad.Reportes.Historico de movimientos por cuenta")]
        public async Task<IActionResult> SFAuxiliarMovimientos()
        {
            return await Task.Run(() => View());

        }



    }
}