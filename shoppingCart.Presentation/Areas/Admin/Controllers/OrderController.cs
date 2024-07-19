using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Impementation;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> OrderHeaders = unitOfWork.Order.GetAll(IncludeWord: "ApplicationUser");
            return Json(new { data = OrderHeaders });
        }
    }
    }
