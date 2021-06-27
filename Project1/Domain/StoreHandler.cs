using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class StoreHandler
    {
        private Project1DBContext _context;

        public StoreHandler(Project1DBContext context)
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

        public Store SearchStore(int id)
        {
            Store store = null;
            try
            {
                store = _context.Stores.Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return store;
        }
    }
}
