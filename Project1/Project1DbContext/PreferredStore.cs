using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class PreferredStore
    {
        public int CustomerId { get; set; }
        public int StoreId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
    }
}
