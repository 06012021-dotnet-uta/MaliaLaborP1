using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OrderHistoryHandler
    {
        private Project1DBContext _context;

        public OrderHistoryHandler(Project1DBContext context)
        {
            this._context = context;
        }

        public List<Store> StoreList()
        {
            List<Store> stores = null;
            try
            {
                stores = _context.Stores.ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return stores;
        }

        public List<Invoice> InvoiceList()
        {
            List<Invoice> invoices = null;
            try
            {
                invoices = _context.Invoices.ToList();
            }
            catch
            {
                Console.WriteLine("Exception.");
            }
            return invoices;
        }

        public List<OrderLine> OrderLineList()
        {
            List<OrderLine> orderLines = null;
            try
            {
                orderLines = _context.OrderLines.ToList();
            }
            catch
            {
                Console.WriteLine("Exception.");
            }
            return orderLines;
        }

        public List<Customer> CustomerList()
        {
            List<Customer> customers = null;
            try
            {
                customers = _context.Customers.ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return customers;
        }
    }
}
