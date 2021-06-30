using Project1DbContext;
using System.Collections.Generic;

namespace Domain
{
    public interface IInvoiceHandler
    {
        bool Add(int productId, int storeId, int amount);
        void CleanCart();
        Dictionary<int, int> GetCart();
        List<Invoice> InvoiceList();
        decimal LineTotal(int productId);
        bool NewOrder(int storeId, int customerId, Dictionary<int, int> cart);
        List<OrderLine> OrderLineList();
        void ReadCart(Dictionary<int, int> cartDictionary);
        bool Remove(int productId, int amount);
        List<Invoice> SearchInvoicesByCustomer(int customerId);
        List<Invoice> SearchInvoicesByStore(int storeId);
        decimal Total();
    }
}