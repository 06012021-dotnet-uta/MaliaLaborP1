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

        public List<Inventory> InventoryList(int id)
        {
            List<Inventory> inventoryList = null;
            try
            {
                inventoryList = _context.Inventories.Where(x => x.StoreId == id).ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return inventoryList;
        }

        public Store StoreList(int id)
        {
            Store store = null;
            try
            {
                store = _context.Stores.Where(x => x.Id == id).First();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return store;
        }
    }
}
