using System;
using System.Collections.Generic;

namespace Shopping.Models
{
    public partial class Discount
    {
        public Discount()
        {
            ProductDiscounts = new HashSet<ProductDiscount>();
        }

        public int DiscountId { get; set; }
        public double ReducePercent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public byte Active { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Account Creator { get; set; } = null!;
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
}
