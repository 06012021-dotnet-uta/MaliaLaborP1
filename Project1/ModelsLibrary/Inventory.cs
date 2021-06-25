using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class Inventory
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
