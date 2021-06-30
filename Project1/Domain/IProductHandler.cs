using Project1DbContext;
using System.Collections.Generic;

namespace Domain
{
    public interface IProductHandler
    {
        bool CheckInventory(int productId, int amount, int storeId);
        List<Inventory> InventoryList(int storeId);
        List<ProductPicture> PicturesList();
        List<Product> ProductList();
        Product SearchProduct(int productId);
    }
}