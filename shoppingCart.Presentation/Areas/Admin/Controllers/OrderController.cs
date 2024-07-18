using Microsoft.AspNetCore.Mvc;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
