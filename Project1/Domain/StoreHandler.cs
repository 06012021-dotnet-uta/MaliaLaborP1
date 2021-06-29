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

        /// <summary>
        /// Accesses the database and returns a List of Store objects.
        /// </summary>
        /// <returns>List of Store objects that exist in the database.</returns>
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

        /// <summary>
        /// Accesses the database to search for a Store with a given ID.
        /// </summary>
        /// <param name="id">ID of store to search for.</param>
        /// <returns>Store object with matching ID.</returns>
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
