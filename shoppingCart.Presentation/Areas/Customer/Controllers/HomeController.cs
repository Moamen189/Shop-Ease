using Microsoft.AspNetCore.Mvc;
using shoppingCart.Entities.Repositories;

namespace shoppingCart.Presentation.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var product = unitOfWork.Product.GetAll();
            return View(product);
        }
    }
}
