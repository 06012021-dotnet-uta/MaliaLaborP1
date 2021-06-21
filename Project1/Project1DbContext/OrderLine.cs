using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class OrderLine
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public int Amount { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
    }
}
