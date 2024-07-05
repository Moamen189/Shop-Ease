using Microsoft.AspNetCore.Mvc;
using shoppingCart.Entities.Repositories;
using shoppingCart.Entities.ViewModels;

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

        public IActionResult Details(int id) {
            ShoppingCart obj = new ShoppingCart()
            {
                 Product = unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, IncludeWord: "Category"),
                 Count = 1
            };
            return View(obj);
        
        }
    }
}
