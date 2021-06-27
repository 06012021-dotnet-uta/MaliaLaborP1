using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project1DbContext;
using Domain;
using Project1.Models;

namespace Project1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly CustomerHandler _customerHandler;

        public AccountController(CustomerHandler customerHandler, ILogger<AccountController> logger)
        {
            this._logger = logger;
            this._customerHandler = customerHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer objUser) // need to do something with sessions here?
        {
            if (ModelState.IsValid)
            {
                bool success = _customerHandler.LoginCustomer(objUser.Username, objUser.Password);
                if (success)
                {
                    var tables = from c in _customerHandler.CustomerList()
                                 select new CustomerViewModel
                                 {
                                     customerVm = c
                                 };
                    var customer = tables.Where(x => x.customerVm.Username == objUser.Username && x.customerVm.Password == objUser.Password).First();
                    return View(customer);
                }
                else
                    return NotFound();
            }
            return NotFound();
        }
    }
}
