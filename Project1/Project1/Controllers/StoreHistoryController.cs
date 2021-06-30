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
    public class StoreHistoryController : Controller
    {
        private readonly ILogger<StoreHistoryController> _logger;
        private readonly IProductHandler _productHandler;
        private readonly IStoreHandler _storeHandler;
        private readonly IInvoiceHandler _invoiceHandler;

        public StoreHistoryController(IProductHandler productHandler, IStoreHandler storeHandler, IInvoiceHandler invoiceHandler, ILogger<StoreHistoryController> logger)
        {
            this._productHandler = productHandler;
            this._storeHandler = storeHandler;
            this._invoiceHandler = invoiceHandler;
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
           var tables = from s in _storeHandler.StoreList()
                        join i in _invoiceHandler.SearchInvoicesByStore((int)id) on s.Id equals i.StoreId
                        join o in _invoiceHandler.OrderLineList() on i.Id equals o.InvoiceId
                        join p in _productHandler.ProductList() on o.ProductId equals p.Id          
                        select new StoreHistoryViewModel
                        {
                            invoiceVm = i,
                            orderLineVm = o,
                            storeVm = s,
                            productVm = p
                        };
            try
            {
                var returnTables = tables.Where(x => x.storeVm.Id == id).ToList();
                if (returnTables == null)
                    return NotFound();
                return View(returnTables);
            }
            catch (Exception)
            {                
                return NotFound();
            }
        }
    }
}
