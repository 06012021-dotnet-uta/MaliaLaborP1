using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class StoreViewModel
    {
        public Store storeVm { get; set; }
        //public Invoice invoiceVm { get; set; } //for order history, add other tables needed for join also
    }
}
