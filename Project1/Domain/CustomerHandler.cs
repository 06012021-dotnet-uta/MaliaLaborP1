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
