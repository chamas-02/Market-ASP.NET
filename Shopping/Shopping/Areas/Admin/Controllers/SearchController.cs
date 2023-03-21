using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly MarketGOContext _content;

        public SearchController(MarketGOContext content) 
        {  
            _content = content;
        }

        // GET: SEARCH/FIND PRODUCT
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if(string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            ls = _content.Products
                .AsNoTracking()
                .Include(a => a.Cat)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }

        // GET: SEARCH/FIND CATEGORY
        [HttpPost]
        public IActionResult FindCategory(string keyword)
        {
            List<Category> ls = new List<Category>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListCategoriesSearchPartial", null);
            }
            ls = _content.Categories
                .AsNoTracking()
                .Include(a => a.Products)
                .Where(x => x.CatName.Contains(keyword))
                .OrderByDescending(x => x.CatId)
                .Take(10)
                .ToList();
            if (ls == null)
            {
                return PartialView("ListCategoriesSearchPartial", null);
            }
            else
            {
                return PartialView("ListCategoriesSearchPartial", ls);
            }
        }
    }
}
