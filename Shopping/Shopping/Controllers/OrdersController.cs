 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Configuration;
using Shopping.Models;
using Shopping.ModelViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using static Shopping.Controllers.OrdersController;
using Microsoft.AspNetCore.Authorization;

namespace Shopping.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly MarketGOContext _context;

        public OrdersController(MarketGOContext context)
        {
            _context = context;
        }

        public class CartDetails
        {
            public List<string> carts { get; set; }
            public string totalQuantity { get; set; }
            public string totalPrice { get; set; }
        }

        // GET: Orders
        [HttpGet]
        [Route("/Order/Checkout")]
        public async Task<IActionResult> Index()
        {
            OrderViewVM model = new OrderViewVM();

            var accountID = HttpContext.Session.GetString("CustomerID");
            model.Customer = _context.Customers.Find(Int32.Parse(accountID));
            model.Locations = _context.Locations.ToList();

            var data = HttpContext.Request.Query["data"];
            if (!String.IsNullOrEmpty(data))
            {
                // Decode the data parameter from URL encoding to a string
                var decodedData = HttpUtility.UrlDecode(data);

                // Parse the JSON string to a C# object
                var cartsData = JObject.Parse(decodedData);

                var carts = cartsData["carts"].ToObject<List<int>>();
                var totalQuantity = cartsData["totalQuantity"].ToObject<int>();
                var totalPrice = cartsData["totalPrice"].ToObject<int>();

                // Tạo danh sách thông tin đơn hàng 
                List<Cart> cartDetails = new List<Cart>();
                foreach (var i in carts)
                {
                    var cart = _context.Carts.Where(c => c.CartId == i)
                        .Include(c => c.Product)
                        .FirstOrDefault();
                    if (cart != null)
                    {
                        cartDetails.Add(cart);
                    }
                }
                model.Details = cartDetails;
                model.TotalQuantity = totalQuantity;
                model.TotalPrice = totalPrice;
            }
            return View(model);
        }

        [HttpPost]
        [Route("/Order/Checkout")]
        public IActionResult Index([FromBody] CartDetails cartDetails)
        {
            return View();
        }

    }
}
