using Shopping.Models;

namespace Shopping.ModelViews
{
    public class OrderViewVM
    {
        public Customer Customer { get; set; }

        public List<Cart> Details { get; set; }

        public List<Location> Locations { get; set; }

        public int TotalQuantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
