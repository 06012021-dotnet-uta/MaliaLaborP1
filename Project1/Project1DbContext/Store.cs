using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class Store
    {
        public Store()
        {
            Inventories = new HashSet<Inventory>();
            Invoices = new HashSet<Invoice>();
            PreferredStores = new HashSet<PreferredStore>();
        }

        public int Id { get; set; }
        public string Region { get; set; }
        public string City { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PreferredStore> PreferredStores { get; set; }
    }
}
