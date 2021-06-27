using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project1DbContext;
using Project1.Models;

namespace Project1.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly ProductHandler _productHandler;
        private readonly StoreHandler _storeHandler;

        public ShopController(ProductHandler productHandler, StoreHandler storeHandler, ILogger<ShopController> logger)
        {
            this._productHandler = productHandler;
            this._storeHandler = storeHandler;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var stores = from s in _storeHandler.StoreList()
                         select new StoreHistoryViewModel
                         {
                             storeVm = s
                         };
            return View(stores);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var tables = from p in _productHandler.ProductList()
                         join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                         join o in _productHandler.InventoryList((int)id) on p.Id equals o.ProductId
                         from i in table1.DefaultIfEmpty()
                         select new ProductViewModel
                         {
                             productVm = p,
                             productPictureVm = i,
                             inventoryVm = o
                         };
            try
            {
                var product = tables.Where(x => x.productVm.Id == id).First();
                if (product == null)
                    return NotFound();
                return View(product);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public IActionResult Products(int? id)
        {

            // join tables together to pass into view
            var tables = from p in _productHandler.ProductList()
                         join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                         join o in _productHandler.InventoryList((int)id) on p.Id equals o.ProductId
                         from i in table1.DefaultIfEmpty()
                         select new ProductViewModel
                         {
                             productVm = p,
                             productPictureVm = i,
                             inventoryVm = o,
                             city = _storeHandler.SearchStore((int)id).City,
                             region = _storeHandler.SearchStore((int)id).Region
                         };
            return View(tables);
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
