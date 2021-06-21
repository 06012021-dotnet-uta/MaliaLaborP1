using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
            PreferredStores = new HashSet<PreferredStore>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PreferredStore> PreferredStores { get; set; }
    }
}
