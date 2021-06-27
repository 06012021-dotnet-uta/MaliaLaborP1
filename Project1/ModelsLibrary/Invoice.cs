using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Project1DbContext
{
    public partial class Invoice
    {
        public Invoice()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        [Display(Name = "Invoice ID")]
        public int Id { get; set; }

        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
