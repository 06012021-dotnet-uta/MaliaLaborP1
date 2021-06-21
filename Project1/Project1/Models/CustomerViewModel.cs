using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class CustomerViewModel
    {
        public Customer customerVm { get; set; }
        public PreferredStore preferredStoreVm { get; set; }
    }
}
