using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Helpers;
using System.Text.Json;

namespace Shopping.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly MarketGOContext _context;
        public INotyfService _notyfService { get; }

        public CartsController(MarketGOContext context)
        {
            _context = context;
        }

        public class CartDetail
        {
            [Required]
            public int productID { get; set; }
            [Required]
            public int quantity { get; set; }

        }

        [Route("/Carts", Name = ("Cart"))]
        public IActionResult Index()
        {
            var accountID = HttpContext.Session.GetString("CustomerID");

            var lsCartDetail = _context.Carts.AsNoTracking()
                .Where(c => c.CustomerID == Int32.Parse(accountID))
                .OrderByDescending(c => c.UpdatedDate)
                .Include(c => c.Product)
                .ToList();

            return View(lsCartDetail);
        }

        [HttpGet]
        [Route("/addToCart")]
        public async Task<IActionResult> AddToCart()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("AddProductToCart")]
        [Route("/addToCart")]
        public async Task<IActionResult> AddToCart([FromBody] CartDetail cartdetail)
        {
            try
            {
                var accountID = HttpContext.Session.GetString("CustomerID");
                if (accountID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    // Tìm xem sản phẩm đã có trong giỏ hàng chưa
                    var cart = _context.Carts.Where(c => c.CustomerID.ToString() == accountID && c.ProductId == cartdetail.productID).FirstOrDefault();
                    if (cart != null)
                    {
                        cart.Quantity = cart.Quantity + cartdetail.quantity;
                        cart.UpdatedDate = DateTime.Now;
                        _context.Update(cart);
                    }
                    else
                    {
                        cart = new Cart
                        {
                            CustomerID = Int32.Parse(accountID),
                            ProductId = cartdetail.productID,
                            Quantity = cartdetail.quantity,
                            UpdatedDate = DateTime.Now
                        };
                        _context.Carts.Add(cart);
                    }
                    await _context.SaveChangesAsync();
                    var data = new
                    {
                        message = "Đã thêm sản phẩm vào giỏ hàng"
                    };
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound(cartdetail);
            }
        }

        [HttpGet]
        [Route("/miniCart", Name = ("miniCart"))]
        [AllowAnonymous]
        public async Task<IActionResult> MiniCart()
        {
            var accountID = HttpContext.Session.GetString("CustomerID");
            if (accountID != null)
            {
                var lsCartDetail = _context.Carts.AsNoTracking()
                    .Where(c => c.CustomerID == Int32.Parse(accountID))
                    .OrderByDescending(c => c.UpdatedDate)
                    .Include(c => c.Product)
                    .ToArray();

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };

                var data = JsonConvert.SerializeObject(lsCartDetail, Formatting.Indented, settings);

                return Ok(data);
            }
            return NotFound(new { message = "Không tìm thấy sản phẩm" });
        }
    }
}
