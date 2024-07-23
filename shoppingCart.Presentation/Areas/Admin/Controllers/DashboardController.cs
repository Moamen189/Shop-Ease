using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.Entities.Repositories;
using ShoppingCart.Utilities;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.Orders = unitOfWork.Order.GetAll().Count();
            ViewBag.Products = unitOfWork.Product.GetAll().Count();
            ViewBag.Users = unitOfWork.ApplicationUser.GetAll().Count();
            ViewBag.ApprovedOrders = unitOfWork.Order.GetAll(x=> x.OrderStatus == SD.Approve).Count();
            return View();
        }
    }
}
