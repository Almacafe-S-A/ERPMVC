using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult> SFDetalladoPorProducto()
        {

            return await Task.Run(()=> View());

        }


        public async Task<IActionResult> SFIngresos()
        {
            return await Task.Run(() => View());

        }

        public async Task<IActionResult> SFReciboMercaderia()
        {

            return await Task.Run(() => View());

        }


        public async Task<IActionResult> SFAreas()
        {

            return await Task.Run(() => View());

        }
        







    }
}