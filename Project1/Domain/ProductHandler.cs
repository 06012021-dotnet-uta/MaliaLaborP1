using Microsoft.EntityFrameworkCore;
using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class ProductHandler
    {
        private Project1DBContext _context;

        public ProductHandler(Project1DBContext context)
        {
            this._context = context;            
        }

        /// <summary>
        /// Accesses the database to return list of products.
        /// </summary>
        /// <returns>List of Product objects within the database.</returns>
        public List<Product> ProductList()
        {
            List<Product> products = null;
            try
            {
                products = _context.Products.ToList();
            }
            catch(Exception)
            {
                Console.WriteLine("Exception.");
            }
            return products;
        }

        public Product SearchProduct(int productId)
        {
            Product product = null;
            try
            {
                product = _context.Products.Where(x => x.Id ==productId).FirstOrDefault();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return product;
        }

        /// <summary>
        /// Accesses the database to return list of pictures associated with products.
        /// </summary>
        /// <returns>List of ProductPicture obbjects within the database.</returns>
        public List<ProductPicture> PicturesList()
        {
            List<ProductPicture> pictures = null;
            try
            {
                pictures = _context.ProductPictures.ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return pictures;
        }

        /// <summary>
        /// Accesses the database to return Inventory objects associated with given ID
        /// </summary>
        /// <param name="storeId">ID of store to get inventory for.</param>
        /// <returns>Inventory objects with store ID matching ID given to method</returns>
        public List<Inventory> InventoryList(int storeId)
        {
            List<Inventory> inventoryList = null;
            try
            {
                inventoryList = _context.Inventories.Where(x => x.StoreId == storeId).ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return inventoryList;
        }
    }
}
