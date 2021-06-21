using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            OrderLines = new HashSet<OrderLine>();
            ProductPictures = new HashSet<ProductPicture>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ICollection<ProductPicture> ProductPictures { get; set; }
    }
}
