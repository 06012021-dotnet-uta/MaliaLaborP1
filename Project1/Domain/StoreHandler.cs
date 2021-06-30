using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class StoreHandler : IStoreHandler
    {
        private Project1DBContext _context;

        public StoreHandler(Project1DBContext context)
        {
            this._context = context;
        }

        public bool SetPreferredStore(int storeId, int customerId)
        {
            bool success = false;

            try
            {
                if (_context.PreferredStores.Where(x => x.CustomerId == customerId).FirstOrDefault() == null) // new entry to table
                {
                    var temp = new PreferredStore()
                    {
                        CustomerId = customerId,
                        StoreId = storeId
                    };
                    _context.Add(temp);
                    _context.SaveChanges();
                }
                else // preferred store exists 
                {
                    var temp = _context.PreferredStores.Where(x => x.CustomerId == customerId).FirstOrDefault();
                    temp.StoreId = storeId;
                    _context.PreferredStores.Update(temp);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                return success;
            }

            return success;
        }

        public List<PreferredStore> PreferredStoreList()
        {
            List<PreferredStore> stores = null;
            try
            {
                stores = _context.PreferredStores.ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Exception.");
            }
            return stores;
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
