using Project1DbContext;
using System.Collections.Generic;

namespace Domain
{
    public interface ICustomerHandler
    {
        bool Add(Customer customer);
        List<Customer> CustomerList();
        PreferredStore GetPreferredStore(int customerId);
        string LoginCustomer(string username, string password);
        List<PreferredStore> PreferredStoreList();
        Customer SearchCustomer(int id);
    }
}