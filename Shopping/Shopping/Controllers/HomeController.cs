using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shopping.Models;
using System.Diagnostics;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly MarketGOContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(MarketGOContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public class MyIndexModel
        {   
            public MyIndexModel() { }

            public MyIndexModel(Product product = null, List<Category> categories = null, List<Product> products = null, List<ProductDiscount> discountProducts = null, List<Product> bestSellerProducts = null, List<Product> arrivalProducts = null)
            {
                Product = product;
                Categories = categories;
                Products = products;
                DiscountProducts = discountProducts;
                BestSellerProducts = bestSellerProducts;
                ArrivalProducts = arrivalProducts;
            }

            public Product Product { get; set; }
            public List<Category> Categories { get; set; }
            public List<Product> Products { get; set; }
            public List<ProductDiscount> DiscountProducts { get; set; }
            public List<Product> BestSellerProducts { get; set; }
            public List<Product> ArrivalProducts { get; set; }



        }

        public IActionResult Index()
        {
            var viewConfig = new Dictionary<string, int>() {
                {"category_num", 4 },
                {"product_num", 8 },
                {"discount_prod_num", 3 },
                {"bestseller_num", 3 },
                {"arrival_prod_num", 6}
            };
            foreach (var kvp in viewConfig)
                Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
            MyIndexModel myIndexModel = new MyIndexModel();
            using (_context)
            {
                var categories = _context.Categories;
                var products = _context.Products;
                var product = products.First();
                var discount_prods = (from dp in _context.ProductDiscounts
                                     join p in products on dp.ProductId equals p.ProductId
                                     join d in _context.Discounts on dp.DiscountId equals d.DiscountId
                                     where dp.Active == 1 
                                     select dp).Include(dp => dp.Discount);
                var bestseller_prods = from p in products
                                       orderby p.UnitsInStock
                                       select p;
                var arrival_prods = from p in products
                                    orderby p.DateCreated
                                    select p;
                myIndexModel = new MyIndexModel(product, 
                    categories.Take(viewConfig["category_num"]).ToList(), 
                    products.Take(viewConfig["product_num"]).ToList(), 
                    discount_prods.Take(viewConfig["discount_prod_num"]).ToList(),
                    bestseller_prods.Take(viewConfig["bestseller_num"]).ToList(), 
                    arrival_prods.Take(viewConfig["arrival_prod_num"]).ToList());
            }

            return View(myIndexModel);
        }

        // ============ CONTACT ============ //
        public IActionResult Contact()
        {
            return View();
        }

        // ============ ABOUT ============ //
        public IActionResult About()
        {
            return View();
        }

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