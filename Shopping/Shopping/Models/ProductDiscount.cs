using System;
using System.Collections.Generic;

namespace Shopping.Models
{
    public partial class ProductDiscount
    {
        public int ProductDiscountId { get; set; }
        public int? DiscountId { get; set; }
        public int? ProductId { get; set; }
        public int? CreatorId { get; set; }
        public byte Active { get; set; }

        public virtual Account? Creator { get; set; }
        public virtual Discount? Discount { get; set; }
        public virtual Product? Product { get; set; }
    }
}
