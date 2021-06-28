using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Project1DbContext;
using Domain;
using Project1.Models;
using Newtonsoft.Json;

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
            if (HttpContext.Session.GetString("customerSession") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Details", "Account");
            }            
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
            var customer = JsonConvert.DeserializeObject<CustomerViewModel>(HttpContext.Session.GetString("customerSession"));
            return View(customer);
        }

        public IActionResult History()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer objUser)
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
                    //serialize customer into a string
                    HttpContext.Session.SetString("customerSession", JsonConvert.SerializeObject(customer));                    
                    return RedirectToAction("Details", "Account");
                }
                else
                    return View("Index");
            }
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
