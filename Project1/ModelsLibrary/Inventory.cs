using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Project1DbContext
{
    public partial class Inventory
    {
        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
