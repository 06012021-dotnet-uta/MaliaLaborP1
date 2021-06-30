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

        /// <summary>
        /// Accesses the database and searches Invoices table associated with specified Store.
        /// </summary>
        /// <param name="storeId">ID of Store for Invoices table to compare against.</param>
        /// <returns>List of Invoices associated with Store with given ID</returns>
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

        /// <summary>
        /// Accesses the database and searches Invoices table associated with specified Customer.
        /// </summary>
        /// <param name="customerId">ID of Store for Customer table to compare against.</param>
        /// <returns>List of Invoices associated with Customer with given ID</returns>
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

        /// <summary>
        /// Accesses the database and returns OrderLines table.
        /// </summary>
        /// <returns>List of OrderLines entries</returns>
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

        /// <summary>
        /// Accepts Dictionary containing product ID's and amount to order, and saves to _shoppingCart
        /// </summary>
        /// <param name="cartDictionary">Deserialized shopping cart Dictionary</param>
        public void ReadCart(Dictionary<int, int> cartDictionary)
        {
            _shoppingCart = cartDictionary;
        }

        /// <summary>
        /// Attempts to add products to shopping cart.
        /// </summary>
        /// <param name="productId">ID of Product to add to cart.</param>
        /// <param name="storeId">ID of Store shopping from.</param>
        /// <param name="amount">Amount of product to add to cart.</param>
        /// <returns>Boolean reflecting success of adding products to the cart.</returns>
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

        /// <summary>
        /// Attempts to remove a specified number of a product from the shopping cart.
        /// </summary>
        /// <param name="productId">ID of product to remove from cart.</param>
        /// <param name="amount">Amount of product to remove from cart.</param>
        /// <returns>Boolean reflecting the success of removing products from the cart.</returns>
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
                    if (_shoppingCart[productId] == 0)
                        _shoppingCart.Remove(productId);
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        /// Attempts to add entries to database for a new invoice and its related lines.
        /// </summary>
        /// <returns>Boolean reflecting the success of making changes to the database.</returns>
        public bool Checkout()
        {
            bool success = false;

            //add to db, maybe return orderid

            return success;
        }

        /// <summary>
        /// Adds up grand total of cart products.
        /// </summary>
        /// <returns>Grand total of products in cart.</returns>
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

        /// <summary>
        /// Adds up the total of a specified product.
        /// </summary>
        /// <param name="productId">ID of the product in cart to get total for.</param>
        /// <returns>Total for specified product.</returns>
        public decimal LineTotal(int productId)
        {
            decimal total = 0;
            if (_shoppingCart.ContainsKey(productId))
            {
                total += _productHandler.SearchProduct(productId).Price * _shoppingCart.ElementAt(productId).Value;
            }
            return total;
        }

        /// <summary>
        /// Gets cart after changes have been made.
        /// </summary>
        /// <returns>Dictionary containing product ID's and amounts.</returns>
        public Dictionary<int, int> GetCart()
        {
            return _shoppingCart;
        }

        /// <summary>
        /// Resets cart if all amounts are 0.
        /// </summary>
        public void CleanCart()
        {
            bool isEmpty = true;
            foreach(var item in _shoppingCart)
            {
                if (item.Value != 0)
                    isEmpty = false;
            }
            if(isEmpty)
            {
                _shoppingCart = new Dictionary<int, int>();
            }
        }

        public bool NewOrder(int storeId, int customerId, Dictionary<int,int> cart)
        {
            bool success = false;
            try
            {
                Invoice newInvoice = new();
                newInvoice.StoreId = storeId;
                newInvoice.CustomerId = customerId;

                _context.Add(newInvoice);
                _context.SaveChanges();
                int returnId = newInvoice.Id;
                foreach(var item in cart)
                {
                    OrderLine newLine = new()
                    {
                        ProductId = item.Key,
                        Amount = item.Value,
                        InvoiceId = returnId
                    };
                    _context.Add(newLine);

                    var inventoriesList = _context.Inventories.Where(x => x.ProductId == item.Key).ToList();
                    foreach (var listing in inventoriesList)
                    {
                        if (listing.StoreId == storeId)
                        {
                            // update product count in inventory
                            listing.Amount -= item.Value;
                            _context.Inventories.Update(listing);
                        }
                    }
                    _context.SaveChanges();
                }
                success = true;
            }
            catch (Exception)
            {

                return success;
            }
            return success;
        }
    }
}
