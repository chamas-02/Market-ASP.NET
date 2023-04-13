using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopping.Models;
using Shopping.ModelViews;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //Xianchew ADD
        private readonly MarketGOContext _context;

        public HomeController(MarketGOContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Xianchew ADD
            var viewConfig = new Dictionary<string, int>() {
                {"category_num", 4 },
                {"product_num", 8 },
                {"discount_prod_num", 3 },
                {"bestseller_num", 3 },
                {"arrival_prod_num", 6 },
                {"mini_cart_num", 3},
            };

            HomeViewVM model = new HomeViewVM();

            var lsProducts = _context.Products.AsNoTracking()
                .Where(x => x.Active == true && x.HomeFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(viewConfig["product_num"])
                .ToList();

            List<ProductHomeVM> lsProductViews = new List<ProductHomeVM>();
            var lsCats = _context.Categories
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderByDescending(x => x.Ordering)
                .ToList()
                .Take(viewConfig["category_num"]);

            foreach (var item in lsCats)
            {
                ProductHomeVM productHome = new ProductHomeVM();
                productHome.category = item;
                productHome.lsProducts = lsProducts.Where(x => x.CatId == item.CatId).ToList();
                lsProductViews.Add(productHome);

                model.Products = lsProductViews;
                ViewBag.AllProducts = lsProducts;
            }

            return View(model);
        }

        // ============ CONTACT ============ //
        public IActionResult Contact()
        {
            return View();
        }

        // ============ ABOUT ============ //
        [Route("about-us.html", Name = "About")]
        public IActionResult About()
        {
            return View();
        }

        // ============ Privacy ============ //
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}