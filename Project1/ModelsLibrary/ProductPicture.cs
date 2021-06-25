using System;
using System.Collections.Generic;

#nullable disable

namespace Project1DbContext
{
    public partial class ProductPicture
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SmallPath { get; set; }
        public string LargePath { get; set; }
        public string AltText { get; set; }
        public string TitleText { get; set; }

        public virtual Product Product { get; set; }
    }
}
