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
            List<Store> storeList = _storeHandler.StoreList();
            return View(storeList);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var tables = from p in _productHandler.ProductList()
                         join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                         from i in table1.DefaultIfEmpty()
                         select new ProductViewModel
                         {
                             productVm = p,
                             productPictureVm = i
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

        public IActionResult Products()
        {
            // pass single table into view
            //List<Product> productsList = _productHandler.ProductList();
            //return View(productsList);

            // join tables together to pass into view
            var tables = from p in _productHandler.ProductList()
                         join i in _productHandler.PicturesList() on p.Id equals i.ProductId into table1
                         from i in table1.DefaultIfEmpty()
                         select new ProductViewModel
                         {
                             productVm = p,
                             productPictureVm = i
                         };
            // need to add shop inventory to join
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
