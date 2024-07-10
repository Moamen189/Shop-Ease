using Microsoft.AspNetCore.Mvc;
using shoppingCart.Entities.Models;
using System.Diagnostics;

namespace shoppingCart.Presentation.Controllers
{
    public class HomexController : Controller
    {
        private readonly ILogger<HomeController> _loggerw;

        public HomexController(ILogger<HomeController> logger)
        {
            _loggerw = logger;
        }

        public IActionResult Index()
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
