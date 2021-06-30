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
        private readonly InvoiceHandler _invoiceHandler;
        private readonly StoreHandler _storeHandler;
        private readonly ProductHandler _productHandler;

        public AccountController(CustomerHandler customerHandler, InvoiceHandler invoiceHandler, StoreHandler storeHandler, ProductHandler productHandler, ILogger<AccountController> logger)
        {
            this._logger = logger;
            this._customerHandler = customerHandler;
            this._invoiceHandler = invoiceHandler;
            this._storeHandler = storeHandler;
            this._productHandler = productHandler;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("customerSession") == null || HttpContext.Session.GetString("customerSession") == "{}")
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
            return View("CreateCustomer");
        }

        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
                RedirectToAction("Create");
            bool success = _customerHandler.Add(customer);
            if (success)
            {
                var tables = from c in _customerHandler.CustomerList()
                             select new CustomerViewModel
                             {
                                 customerVm = c
                             };
                var cust = tables.Where(x => x.customerVm.Id == customer.Id).FirstOrDefault();
                HttpContext.Session.SetString("customerSession", JsonConvert.SerializeObject(cust));
                return View("Details");
            }
            else
            {
                ViewData["Message"] = "Something went wrong.";
                return View("CustomError");
            }
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Details()
        {
            if (HttpContext.Session.GetString("customerSession") == null || HttpContext.Session.GetString("customerSession") == "{}")
                return View("Index");
            var customer = JsonConvert.DeserializeObject<CustomerViewModel>(HttpContext.Session.GetString("customerSession"));
            try
            {
                var history = from c in _customerHandler.CustomerList()
                              join i in _invoiceHandler.SearchInvoicesByCustomer((int)customer.customerVm.Id) on c.Id equals i.CustomerId
                              join s in _storeHandler.StoreList() on i.StoreId equals s.Id
                              join o in _invoiceHandler.OrderLineList() on i.Id equals o.InvoiceId
                              join p in _productHandler.ProductList() on o.ProductId equals p.Id
                              select new CustomerHistoryViewModel
                              {
                                  invoiceVm = i,
                                  orderLineVm = o,
                                  storeVm = s,
                                  productVm = p,
                                  customerVm = c
                              };

                return View(history);
            }
            catch (Exception)
            {
                ViewData["Message"] = "Something went wrong";
                return View("CustomError");
            }
            
        }

        public IActionResult History()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer objUser)
        {
            string result = _customerHandler.LoginCustomer(objUser.Username, objUser.Password);
            ViewData["Message"] = result;
            if (result == "") // login successful
            {
                var tables = from c in _customerHandler.CustomerList()
                             select new CustomerViewModel
                             {
                                 customerVm = c
                             };
                var customer = tables.Where(x => x.customerVm.Username == objUser.Username).First();
                Dictionary<int, int> cart = new Dictionary<int, int>();
                // serialize customer into a string
                HttpContext.Session.SetString("customerSession", JsonConvert.SerializeObject(customer));
                // create cart if it doesnt already exist
                if (HttpContext.Session.GetString("cart") == null || HttpContext.Session.GetString("cart") == "{}")
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                return RedirectToAction("Details", "Account");
            }
            else // login not successful, return to login screen
                return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
