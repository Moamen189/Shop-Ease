using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using System.Security.Claims;
using X.PagedList.Extensions;

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
        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            var PageSize = 8;
            var product = unitOfWork.Product.GetAll().ToPagedList(PageNumber , PageSize);
            return View(product);
        }

        public IActionResult Details(int ProductId) {
            ShoppingCartDetails obj = new ShoppingCartDetails()
            {
                ProductId = ProductId,
                 Product = unitOfWork.Product.GetFirstOrDefault(x => x.Id == ProductId, IncludeWord: "Category"),
                 Count = 1
            };
            return View(obj);
        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCartDetails shoppingCartDetails)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity; 
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartDetails.ApplicationUserId = claim.Value;

            ShoppingCartDetails CartObj = unitOfWork.ShoppingCartDetails.GetFirstOrDefault(
                x => x.ApplicationUserId == shoppingCartDetails.ApplicationUserId && x.ProductId == shoppingCartDetails.ProductId,
                IncludeWord: "Product"
                );
            if (CartObj == null)
            {
                unitOfWork.ShoppingCartDetails.Add(shoppingCartDetails);
            }
            else
            {
                unitOfWork.ShoppingCartDetails.increaseCount(CartObj, shoppingCartDetails.Count);
            }
            unitOfWork.ShoppingCartDetails.Add(shoppingCartDetails);
            unitOfWork.Complete();
            return RedirectToAction("Index");

        }
    }
}
