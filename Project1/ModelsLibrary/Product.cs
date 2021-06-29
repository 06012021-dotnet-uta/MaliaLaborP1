using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name ="Product ID")]
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        public decimal Price { get; set; }
        
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ICollection<ProductPicture> ProductPictures { get; set; }
    }
}
