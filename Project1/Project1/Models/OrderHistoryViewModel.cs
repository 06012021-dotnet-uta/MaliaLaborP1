using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class OrderHistoryViewModel
    {
        public Invoice invoiceVm { get; set; }
        public OrderLine orderLineVm { get; set; }
        public Store storeVm { get; set; }
        public Customer customerVm { get; set; }
    }
}
