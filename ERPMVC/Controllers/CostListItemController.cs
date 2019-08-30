using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    public class CostListItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}