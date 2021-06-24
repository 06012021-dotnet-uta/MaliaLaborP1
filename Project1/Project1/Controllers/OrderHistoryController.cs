using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Controllers
{
    public class OrderHistoryController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly ProductHandler _productHandler;
        private readonly OrderHistoryHandler _orderHistoryHandler;

        public OrderHistoryController(ProductHandler productHandler, OrderHistoryHandler orderHistoryHandler, ILogger<ShopController> logger)
        {
            this._productHandler = productHandler;
            this._orderHistoryHandler = orderHistoryHandler;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var stores = from s in _orderHistoryHandler.StoreList()
                         select new OrderHistoryViewModel
                         {
                             storeVm = s
                         };
            return View(stores);
        }

        public IActionResult Details(int? id)
        {
            var tables = from s in _orderHistoryHandler.StoreList()
                         from i in _orderHistoryHandler.InvoiceList()
                         from o in _orderHistoryHandler.OrderLineList()
                         select new OrderHistoryViewModel
                         {
                             storeVm = s,
                             invoiceVm = i,
                             orderLineVm = o
                         };

            // join tables somehow
            //var tables = from s in _orderHistoryHandler.StoreList()                       
            //             join i in _orderHistoryHandler.InvoiceList() on s.Id equals i.StoreId
            //             join o in _orderHistoryHandler.OrderLineList() on i.Id equals o.InvoiceId
            //             select new 
            //             {
            //                 storeId = s.Id,
            //                 invoiceId = i.Id,
            //                 orderLines = o
            //             };
            try
            {
                var store = tables.Where(x => x.storeVm.Id == id).First();
                if (store == null)
                    return NotFound();
                return View(store);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
