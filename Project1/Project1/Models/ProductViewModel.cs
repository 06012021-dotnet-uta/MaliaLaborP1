﻿using Project1DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class ProductViewModel
    {
        public Product productVm { get; set; }
        public ProductPicture productPictureVm { get; set; }
        public Inventory inventoryVm { get; set; }
        public string city { get; set; }
        public string region { get; set; }
    }
}
