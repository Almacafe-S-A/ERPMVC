using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CustomerOfCustomerController : Controller
    {
        // GET: CustomerOfCustomer
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CustomersOfCustomer()
        {

            return await Task.Run(() => View());
        }

        // GET: CustomerOfCustomer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerOfCustomer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerOfCustomer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOfCustomer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerOfCustomer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOfCustomer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerOfCustomer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}