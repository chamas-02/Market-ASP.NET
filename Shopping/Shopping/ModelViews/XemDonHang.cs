using System;
using System.Collections.Generic;
using Shopping.Models;

namespace Shopping.ModelViews
{
    public class XemDonHang
    {
        public Order DonHang { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
