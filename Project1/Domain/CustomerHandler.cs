using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CustomerHandler
    {
        private Project1DBContext _context;
        private Customer _currentCustomer;

        public CustomerHandler(Project1DBContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Returns true if customer credentials match an entry in the database.
        /// </summary>
        /// <param name="username">Username of customer.</param>
        /// <param name="password">Password of customer.</param>
        /// <returns>Boolean reflecting if customer exists with specified credentials.</returns>
        public bool LoginCustomer(string username, string password)
        {
            bool success = false;
            try
            {
                var temp = _context.Customers.Where(x => x.Username == username && x.Password == password).First();
                if(temp != null)
                {
                    _currentCustomer = temp;
                    success = true;
                }
            }
            catch (Exception)
            {
                // put log info here
                return success;                
            }
            return success;
        }

        /// <summary>
        /// Accesses the database and returns Customer entries.
        /// </summary>
        /// <returns>List of Customer entries within the databbase.</returns>
        public List<Customer> CustomerList()
        {
            List<Customer> customers = null;
            try
            {
                customers = _context.Customers.ToList();
            }
            catch
            {
                Console.WriteLine("Exception.");
            }
            return customers;
        }


        public Customer SearchCustomer(int id)
        {
            Customer customer = null;
            try
            {
                customer = _context.Customers.Where(x => x.Id == id).FirstOrDefault();
            }
            catch
            {
                Console.WriteLine("Exception.");
            }
            return customer;
        }

        public PreferredStore GetPreferredStore(int customerId)
        {
            PreferredStore store = null;
            try
            {
                store = _context.PreferredStores.Where(x => x.CustomerId == customerId).FirstOrDefault();
            }
            catch
            {
                Console.WriteLine("Exception.");
            }
            return store;
        }
    }
}
