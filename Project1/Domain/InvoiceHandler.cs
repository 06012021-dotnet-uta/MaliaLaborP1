using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class InvoiceHandler
    {
        private Project1DBContext _context;

        public InvoiceHandler(Project1DBContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Accesses the database and retrieves a list of Invoice objects.
        /// </summary>
        /// <returns>List of Invoice objects from the database.</returns>
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

        public List<Invoice> SearchInvoicesByStore(int storeId)
        {
            List <Invoice> invoices = null;
            try
            {
                invoices = _context.Invoices.Where(x => x.StoreId == storeId).ToList();
            }
            catch
            {
                Console.WriteLine("Exception.");
            }
            return invoices;
        }

        public List<Invoice> SearchInvoicesByCustomer(int customerId)
        {
            List<Invoice> invoices = null;
            try
            {
                invoices = _context.Invoices.Where(x => x.CustomerId == customerId).ToList();
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
    }
}
