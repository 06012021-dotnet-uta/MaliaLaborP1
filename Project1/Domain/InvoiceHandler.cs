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
        private Dictionary<int, int> _shoppingCart; //.Key is product ID and .Value is count
        private ProductHandler _productHandler;

        public InvoiceHandler(Project1DBContext context, ProductHandler productHandler)
        {
            this._context = context;
            this._productHandler = productHandler;
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


        public void ReadCart(Dictionary<int, int> cartDictionary)
        {
            _shoppingCart = cartDictionary;
        }

        //add
        public bool Add(int productId, int storeId, int amount)
        {
            bool success = false;

            var product = _productHandler.SearchProduct(productId);
            if (product != null)
            {
                if (_shoppingCart.ContainsKey(product.Id)) // item exists in cart already
                {
                    if (_productHandler.CheckInventory(productId, amount + _shoppingCart[product.Id], storeId))
                    {
                        _shoppingCart[product.Id] += amount;
                        success = true;
                    }
                }
                else // item does not exist in cart before add
                {
                    if (_productHandler.CheckInventory(productId, amount, storeId))
                    {
                        _shoppingCart[product.Id] = amount;
                        success = true;
                    }
                }
            }
            return success;
        }

        //remove
        public bool Remove(int productId, int amount)
        {
            bool success = false;
            var product = _productHandler.SearchProduct(productId);
            if (product != null)
            {
                // if the product is in the cart and count to remove is less than or equal to count in cart
                if (_shoppingCart.ContainsKey(product.Id) && _shoppingCart.Where(x => x.Key == product.Id).First().Value >= amount)
                {
                    _shoppingCart[product.Id] -= amount;
                    success = true;
                }
            }
            return success;
        }

        //checkout
        public bool Checkout()
        {
            bool success = false;

            //add to db, maybe return orderid

            return success;
        }

        public decimal Total()
        {
            decimal total = 0;

            foreach (var id in _shoppingCart)
            {
                var product = _productHandler.SearchProduct(id.Key);
                if (product != null)
                    total += product.Price * id.Value;
            }
            return total;
        }

        public decimal LineTotal(int productId)
        {
            decimal total = 0;
            if (_shoppingCart.ContainsKey(productId))
            {
                total += _productHandler.SearchProduct(productId).Price * _shoppingCart.ElementAt(productId).Value;
            }
            return total;
        }

        //get cart
        public Dictionary<int, int> GetCart()
        {
            return _shoppingCart;
        }
    }
}
