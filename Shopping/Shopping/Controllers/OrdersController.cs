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
using static Shopping.Controllers.CartsController;
using AspNetCoreHero.ToastNotification.Helpers;

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

        public class DetailOrder
        {
            public int totalPrice { get; set; }
            public string note { get; set; }
            public string address { get; set; }
            public int ward { get; set; }
            public int district { get; set; }
            public int location { get; set; }
            public List<int> cartids { get; set; }
        }

        [HttpPost]
        [Route("/Order")]
        public async Task<IActionResult> Order([FromBody] DetailOrder detailOrder)
        {
            try
            {
                var accountID = HttpContext.Session.GetString("CustomerID");

                Order order = new Order
                {
                    CustomerID = Int32.Parse(accountID),
                    OrderDate = DateTime.Now,
                    TransactStatusId = 1,
                    TotalMoney = detailOrder.totalPrice,
                    Note = detailOrder.note,
                    Address = detailOrder.address,
                    Ward = detailOrder.ward,
                    District = detailOrder.district,
                    LocationId = detailOrder.location,
                };

                foreach (int i in detailOrder.cartids)
                {
                    var cart = _context.Carts.Where(c => c.CartId == i).Include(c => c.Product).First();
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = cart.ProductId,
                        Amount = cart.Quantity,
                        Price = cart.Product.Price,
                        CreateDate = DateTime.Now,
                        TotalMoney = cart.Quantity * cart.Product.Price,
                    });
                    _context.Remove(cart);
                }

                _context.Orders.Add(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt hàng thành công" });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/Order/{id}")]
        public async Task<IActionResult> OrderDetail(int id)
        {
            try
            {
                var accountID = HttpContext.Session.GetString("CustomerID");

                var order = from o in _context.Orders
                            join w in _context.Locations on o.Ward equals w.LocationId
                            join d in _context.Locations on o.District equals d.LocationId
                            join c in _context.Locations on o.LocationId equals c.LocationId
                            where o.OrderId == id
                            select new
                            {
                                TotalPrice = o.TotalMoney,
                                OrderDate = o.OrderDate,
                                ShipDate = o.ShipDate,
                                Note = o.Note,
                                Address = string.Format("{0}, {1}, {2}, {3}", o.Address, w.Name, d.Name, c.Name)
                            };

                var details = from od in _context.OrderDetails
                              join p in _context.Products on od.ProductId equals p.ProductId
                              where od.OrderId == id
                              select new
                              {
                                  ProductName = p.ProductName,
                                  Quantity = od.Amount,
                                  SubTotal = od.TotalMoney
                              };

                return Ok(new { order = order, details = details });
            }
            catch
            {
                return BadRequest(new { message = "Không tìm thấy đơn hàng của bạn" });
            }
        }

    }
}
