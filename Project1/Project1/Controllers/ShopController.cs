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
        private readonly ProductHandler _productHandler;
        private readonly StoreHandler _storeHandler;
        private readonly InvoiceHandler _invoiceHandler;

        public ShopController(ProductHandler productHandler, StoreHandler storeHandler, ILogger<ShopController> logger, InvoiceHandler invoiceHandler)
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
            else
            {
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
            return View("Products", tables);
        }

        public IActionResult Details(int? storeId, int? productId)
        {
            if (storeId == null)
                return NotFound();
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
                    return NotFound();
                return View(product);
            }
            catch (Exception)
            {
                return NotFound();
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
                return View("Products", tables);
                // redirect to products page?
            }
            else
            {
                return View("NoStock");
                // no inventory page
            }
        }

        public IActionResult Remove()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
