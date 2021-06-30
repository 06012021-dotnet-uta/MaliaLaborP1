using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project1DbContext;
using Project1.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project1.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly IProductHandler _productHandler;
        private readonly IStoreHandler _storeHandler;
        private readonly IInvoiceHandler _invoiceHandler;

        public ShopController(IProductHandler productHandler, IStoreHandler storeHandler, ILogger<ShopController> logger, IInvoiceHandler invoiceHandler)
        {
            this._productHandler = productHandler;
            this._storeHandler = storeHandler;
            this._logger = logger;
            this._invoiceHandler = invoiceHandler;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("storeSession") != null && HttpContext.Session.GetInt32("storeSession") != 0) // store is set already, ask to change store
            {                
                return View("ChangeStore");
            }
            else // store not set
            {
                if (HttpContext.Session.GetString("customerSession") != null && HttpContext.Session.GetString("customerSession") != "{}") // customer logged in, look for preferred store
                {
                    var customer = JsonConvert.DeserializeObject<CustomerViewModel>(HttpContext.Session.GetString("customerSession"));
                    // set pref store session stuff
                    if (HttpContext.Session.GetInt32("prefStore") != 0 && HttpContext.Session.GetInt32("prefStore") != null) // preferred store is set
                    {
                        HttpContext.Session.SetInt32("storeSession", (int)HttpContext.Session.GetInt32("prefStore"));
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(new Dictionary<int, int>()));
                        return RedirectToAction("Products");
                    }
                }
                    var stores = from s in _storeHandler.StoreList()
                             select new StoreHistoryViewModel
                             {
                                 storeVm = s
                             };
                return View(stores);
            }
        }

        public IActionResult ChangeStore()
        {
            return View();
        }

        public IActionResult ClearStore()
        {
            HttpContext.Session.SetInt32("storeSession", 0);
            HttpContext.Session.SetString("cart", "{}");
            var stores = from s in _storeHandler.StoreList()
                         select new StoreHistoryViewModel
                         {
                             storeVm = s
                         };
            return View("Index", stores);
        }

        public IActionResult SetStore(int? id)
        {
            var store = _storeHandler.SearchStore((int)id);

            HttpContext.Session.SetInt32("storeSession", store.Id);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(new Dictionary<int, int>()));

            var tables = from p in _productHandler.ProductList()
                         join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                         join o in _productHandler.InventoryList(store.Id) on p.Id equals o.ProductId
                         from i in table1.DefaultIfEmpty()
                         select new ProductViewModel
                         {
                             productVm = p,
                             productPictureVm = i,
                             inventoryVm = o,
                             city = store.City,
                             region = store.Region
                         };
            return RedirectToAction("Products");
        }

        public IActionResult Details(int? storeId, int? productId)
        {
            if (storeId == null)
            {
                ViewData["Title"] = "Error - Not Found";
                ViewData["Message"] = "Store ID not found. Please try again.";
                return View("CustomError");
            }
            var tables = from p in _productHandler.ProductList()
                         join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                         join o in _productHandler.InventoryList((int)storeId) on p.Id equals o.ProductId
                         from i in table1.DefaultIfEmpty()
                         select new ProductViewModel
                         {
                             productVm = p,
                             productPictureVm = i,
                             inventoryVm = o
                         };
            try
            {
                var product = tables.Where(x => x.productVm.Id == productId).First();
                if (product == null)
                {
                    ViewData["Title"] = "Error - Not Found";
                    ViewData["Message"] = "Product ID not found. Please try again.";
                    return View("CustomError");
                }
                return View(product);
            }
            catch (Exception)
            {
                ViewData["Title"] = "Error - Not Found";
                ViewData["Message"] = "Product ID not found. Please try again.";
                return View("CustomError");
            }
        }

        public IActionResult Products()
        {
            if (HttpContext.Session.GetInt32("storeSession") != null && HttpContext.Session.GetInt32("storeSession") != 0)
            {
                var store = _storeHandler.SearchStore((int)HttpContext.Session.GetInt32("storeSession"));
                // join tables together to pass into view
                var tables = from p in _productHandler.ProductList()
                             join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                             join o in _productHandler.InventoryList(store.Id) on p.Id equals o.ProductId
                             from i in table1.DefaultIfEmpty()
                             select new ProductViewModel
                             {
                                 productVm = p,
                                 productPictureVm = i,
                                 inventoryVm = o,
                                 city = store.City,
                                 region = store.Region
                             };
                return View(tables);
            }
            else
            {
                var stores = from s in _storeHandler.StoreList()
                             select new StoreHistoryViewModel
                             {
                                 storeVm = s
                             };
                return View("Index", stores);
            }
        }

        public IActionResult Add(int storeId, int productId, int amount)
        {
            bool success = false;
            if (HttpContext.Session.GetString("cart") != null && HttpContext.Session.GetString("cart") != "{}")
            {
                _invoiceHandler.ReadCart(JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("cart")));
                success = _invoiceHandler.Add(productId, storeId, amount);
                if(success)
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_invoiceHandler.GetCart()));
            }
            else
            {
                _invoiceHandler.ReadCart(new Dictionary<int, int>());
                success = _invoiceHandler.Add(productId, storeId, amount);
                if(success)
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_invoiceHandler.GetCart()));
            }
            if (success)
            {
                return View("Cart", _productHandler.ProductList());
            }
            else
            {
                ViewData["Title"] = "Error - Stock";
                ViewData["Message"] = "Error, not enough stock present at selected store.";
                return View("CustomError");
            }
        }

        public IActionResult Remove(int productId, int amount)
        {
            if (HttpContext.Session.GetString("cart") != null && HttpContext.Session.GetString("cart") != "{}")
            {
                _invoiceHandler.ReadCart(JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("cart")));
                bool success = _invoiceHandler.Remove(productId, amount);
                if (success)
                {
                    // clean cart
                    _invoiceHandler.CleanCart();
                    // set cart session
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_invoiceHandler.GetCart()));
                    return View("Cart", _productHandler.ProductList());
                }
                else // remove count greater than amount in cart
                {
                    ViewData["Title"] = "Error - Count";
                    ViewData["Message"] = "Amount attempted to remove from cart is greater than number present in cart.";
                    return View("CustomError");
                }
            }
            else // cart is empty
            {
                ViewData["Title"] = "Error - Empty Cart";
                ViewData["Message"] = "Cart is empty. Add some items before attempting to remove.";
                return View("CustomError");
            }
        }

        public IActionResult Edit(int productId)
        {
            ViewBag.productId = productId;
            return View();
        }

        public IActionResult Cart()
        {
            if(HttpContext.Session.GetString("cart") == null || HttpContext.Session.GetString("cart") == "{}")
            {
                ViewData["Message"] = "Shopping cart is empty, please add some items and try again.";
                return View("CustomError");
            }
            var products = _productHandler.ProductList();
            
            return View(products);
        }

        public IActionResult Checkout()
        {
            if(HttpContext.Session.GetString("customerSession") == null || HttpContext.Session.GetString("customerSession") == "{}")
            {
                ViewData["Message"] = "A Customer account is required to checkout, please login or create an account.";
                return RedirectToAction("Index", "Account");
            }
            if (HttpContext.Session.GetString("cart") == null || HttpContext.Session.GetString("cart") == "{}")
            {
                ViewData["Message"] = "Shopping Cart is empty, please add some items and try again.";
                return View("CustomError");
            }
            int customerId = JsonConvert.DeserializeObject<CustomerViewModel>(HttpContext.Session.GetString("customerSession")).customerVm.Id;
            int storeId = (int)HttpContext.Session.GetInt32("storeSession");
            bool success = _invoiceHandler.NewOrder(storeId, customerId, JsonConvert.DeserializeObject<Dictionary<int,int>>(HttpContext.Session.GetString("cart")));
            if(!success)
            {
                ViewData["Message"] = "Something went wrong.";
                return View("CustomError");
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(new Dictionary<int, int>()));
            return View("OrderSuccess");
        }
    }
}
