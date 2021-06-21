using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class Invoice
    {
        public Invoice()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
