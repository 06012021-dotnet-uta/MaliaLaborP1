using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Project1DbContext
{
    public partial class PreferredStore
    {
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
    }
}
