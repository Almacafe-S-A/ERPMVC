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
    public class RecibosCertificadoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }



}