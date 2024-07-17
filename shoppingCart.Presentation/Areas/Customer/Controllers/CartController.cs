using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Impementation;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using shoppingCart.Entities.ViewModels;
using ShoppingCart.Utilities;
using Stripe.Checkout;
using System.Security.Claims;

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
		[HttpGet]
		public IActionResult Summary()
		{
			var claimsIdentity = (System.Security.Claims.ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
			ShoppingCartVM = new ShoppingCartVM()
			{
				CartsList = unitOfWork.ShoppingCartDetails.GetAll(x => x.ApplicationUserId == claim.Value, IncludeWord: "Product"),
				OrderHeader = new()
			};
			ShoppingCartVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == claim.Value);
			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			foreach (var list in ShoppingCartVM.CartsList)
			{
				ShoppingCartVM.TotalCarts += (list.Product.Price * list.Count);

			}
			return View();
		}
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult POSTSummary(ShoppingCartVM ShoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.CartsList = unitOfWork.ShoppingCartDetails.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "Product");


            ShoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;


            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            }

            unitOfWork.Order.Add(ShoppingCartVM.OrderHeader);
            unitOfWork.Complete();

            foreach (var item in ShoppingCartVM.CartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                unitOfWork.OrderDetails.Add(orderDetail);
                unitOfWork.Complete();
            }

            var domain = "https://localhost:7020/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            ShoppingCartVM.OrderHeader.SessionId = session.Id;

            unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.CartsList);
            //         _unitOfWork.Complete();
            //         return RedirectToAction("Index","Home");

        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = unitOfWork.Order.GetFirstOrDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                unitOfWork.Order.UpdateOrderStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                unitOfWork.Complete();
            }
            List<ShoppingCartDetails> shoppingcarts = unitOfWork.ShoppingCartDetails.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            HttpContext.Session.Clear();
            unitOfWork.ShoppingCartDetails.RemoveRange(shoppingcarts);
            unitOfWork.Complete();
            return View(id);
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
