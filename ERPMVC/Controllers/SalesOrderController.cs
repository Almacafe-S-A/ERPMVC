using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    public class SalesOrderController : Controller
    {
        //private readonly ApplicationDbContext _context;

        public SalesOrderController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwSalesOrder()
        {
           // SalesOrder _salesorder = 

            return View();
        }


    }
}