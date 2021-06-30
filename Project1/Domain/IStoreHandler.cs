using Project1DbContext;
using System.Collections.Generic;

namespace Domain
{
    public interface IStoreHandler
    {
        List<PreferredStore> PreferredStoreList();
        Store SearchStore(int id);
        bool SetPreferredStore(int storeId, int customerId);
        List<Store> StoreList();
    }
}