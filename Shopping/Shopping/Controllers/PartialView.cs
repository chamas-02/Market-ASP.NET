using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.ModelViews;

namespace Shopping.Controllers
{
    public class PartialView : Controller
    {
        private readonly MarketGOContext _context;
        public PartialView (MarketGOContext context)
        {
            _context = context;
        }
        public IActionResult _HeaderPartialView()
        {
            var lsCategorys = _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.CatId);

            return PartialView();
        }
    }
}
