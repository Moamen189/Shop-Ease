using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.Entities.Repositories;
using shoppingCart.Entities.ViewModels;

namespace shoppingCart.Presentation.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = unitOfWork.ShoppingCartDetails.GetAll(x => x.ApplicationUserId == claim.Value , IncludeWord:"Product")
            };
            foreach(var list in ShoppingCartVM.CartsList)
			{
				ShoppingCartVM.TotalCarts += (list.Product.Price * list.Count);
				
			}
            return View();
        }

        public IActionResult Plus(int cartId)
		{
			var cart = unitOfWork.ShoppingCartDetails.GetFirstOrDefault(x => x.Id == cartId, IncludeWord: "Product");
			unitOfWork.ShoppingCartDetails.increaseCount(cart, 1);
			unitOfWork.Complete();
			return RedirectToAction("Index");
		}

        public IActionResult Minus(int cartId)
        {
			var cart = unitOfWork.ShoppingCartDetails.GetFirstOrDefault(x => x.Id == cartId, IncludeWord: "Product");
            if(cart.Count == 1)
            {
				unitOfWork.ShoppingCartDetails.Remove(cart);
				unitOfWork.Complete();
				return RedirectToAction("Index" , "Home");

			}
			else
			{
				unitOfWork.ShoppingCartDetails.decreaseCount(cart, 1);
				unitOfWork.Complete();
			}
            return RedirectToAction("Index");
		}

		public IActionResult Remove(int cartId)
		{
			var cart = unitOfWork.ShoppingCartDetails.GetFirstOrDefault(x => x.Id == cartId, IncludeWord: "Product");
			unitOfWork.ShoppingCartDetails.Remove(cart);
			unitOfWork.Complete();
			return RedirectToAction("Index");
		}
    }
}
