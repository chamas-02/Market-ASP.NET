using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
