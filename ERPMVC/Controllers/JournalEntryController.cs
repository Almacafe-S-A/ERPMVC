using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    public class JournalEntryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}