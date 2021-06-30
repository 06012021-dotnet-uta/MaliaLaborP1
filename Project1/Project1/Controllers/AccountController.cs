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
        private readonly ICustomerHandler _customerHandler;
        private readonly IInvoiceHandler _invoiceHandler;
        private readonly IStoreHandler _storeHandler;
        private readonly IProductHandler _productHandler;

        public AccountController(ICustomerHandler customerHandler, IInvoiceHandler invoiceHandler, IStoreHandler storeHandler, IProductHandler productHandler, ILogger<AccountController> logger)
        {
            this._logger = logger;
            this._customerHandler = customerHandler;
            this._invoiceHandler = invoiceHandler;
            this._storeHandler = storeHandler;
            this._productHandler = productHandler;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("customerSession") == null || HttpContext.Session.GetString("customerSession") == "{}" || !ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Details", "Account");
            }
        }

        public IActionResult PreferredStore()
        {
            var stores = _storeHandler.StoreList();
            return View(stores);
        }

        public IActionResult SetPreferredStore(int storeId)
        {
            int customerId = JsonConvert.DeserializeObject<CustomerViewModel>(HttpContext.Session.GetString("customerSession")).customerVm.Id;
            bool success = _storeHandler.SetPreferredStore(storeId, customerId);
            HttpContext.Session.SetInt32("prefStore", storeId);
            if (success)
                return View();
            else
                return RedirectToAction("Details");
        }

        public IActionResult Search(string firstName, string lastName)
        {
            if (firstName != null && lastName != null && _customerHandler.CustomerList().Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower()) != null) // both names included
            {
                var customers = _customerHandler.CustomerList().Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower());
                return View("Lookup", customers);
            }
            if (firstName == null && _customerHandler.CustomerList().Where(x => x.LastName.ToLower() == lastName.ToLower()) != null) // only last name given
            {
                var customers = _customerHandler.CustomerList().Where(x => x.LastName.ToLower() == lastName.ToLower());
                return View("Lookup", customers);
            }
            if (lastName == null && _customerHandler.CustomerList().Where(x => x.FirstName.ToLower() == firstName.ToLower()) != null) // only first name given
            {
                var customers = _customerHandler.CustomerList().Where(x => x.FirstName.ToLower() == firstName.ToLower());
                return View("Lookup", customers);
            }
            return View("Lookup", _customerHandler.CustomerList());
        }

        public IActionResult Lookup()
        {
            var customers = _customerHandler.CustomerList();
            return View(customers);
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
                return View("Details", cust);
            }
            else
            {
                ViewData["Message"] = $"Customer already exists with username {customer.Username}";
                return View();
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

            return View(customer);
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
                                 customerVm = c,
                             };
                var customer = tables.Where(x => x.customerVm.Username == objUser.Username).FirstOrDefault();
                Dictionary<int, int> cart = new Dictionary<int, int>();
                // set pref store session
                if (_storeHandler.PreferredStoreList().Where(x => x.CustomerId == customer.customerVm.Id).FirstOrDefault() != null)
                    HttpContext.Session.SetInt32("prefStore", _storeHandler.PreferredStoreList().Where(x => x.CustomerId == customer.customerVm.Id).FirstOrDefault().CustomerId);
                // serialize customer into a string
                HttpContext.Session.SetString("customerSession", JsonConvert.SerializeObject(customer, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
                ));
                // create cart if it doesnt already exist
                if (HttpContext.Session.GetString("cart") == null || HttpContext.Session.GetString("cart") == "{}")
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                return View("Details", customer);
            }
            else // login was not successful, return to login screen
                return View("Index");
        }

        public IActionResult History()
        {
            if (HttpContext.Session.Get("customerSession") == null || HttpContext.Session.GetString("customerSession") == "{}") // customer is not logged in
                return View("Index");
            var customer = JsonConvert.DeserializeObject<CustomerViewModel>(HttpContext.Session.GetString("customerSession"));

            var tables = from c in _customerHandler.CustomerList()
                         join i in _invoiceHandler.InvoiceList() on c.Id equals i.CustomerId
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
            var history = tables.Where(x => x.customerVm.Id == customer.customerVm.Id);
            return View(history);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}


