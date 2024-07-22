using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Impementation;
using shoppingCart.Entities.Repositories;
using ShoppingCart.Utilities;
using System.Security.Claims;

namespace shoppingCart.Presentation.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> Invoke()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.SessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));

                }
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionKey, unitOfWork.ShoppingCartDetails.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                    return View(0);
            }
           
        }
    }
}
