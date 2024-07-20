using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Impementation;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using shoppingCart.Entities.ViewModels;
using ShoppingCart.Utilities;
using Stripe;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public OrderVM OrderViewModel { get; set; }
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

        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = unitOfWork.Order.GetFirstOrDefault(u => u.Id == orderId, IncludeWord: "ApplicationUser"),
                OrderDetails = unitOfWork.OrderDetails.GetAll(x => x.OrderHeaderId == orderId, IncludeWord: "Product")
            };
            return View(orderVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult UpdaeOrderDetails()
		{
            var orderFromDB = unitOfWork.Order.GetFirstOrDefault(u => u.Id == OrderViewModel.OrderHeader.Id);
            orderFromDB.Name = OrderViewModel.OrderHeader.Name;
            orderFromDB.Phone = OrderViewModel.OrderHeader.Phone;
            orderFromDB.Address = OrderViewModel.OrderHeader.Address;
            orderFromDB.City = OrderViewModel.OrderHeader.City;
            if(OrderViewModel.OrderHeader.Carrier != null)
            {
                orderFromDB.Carrier = OrderViewModel.OrderHeader.Carrier;
            }
			if (OrderViewModel.OrderHeader.TrackingNumber != null)
			{
				orderFromDB.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
			}
            unitOfWork.Order.Update(orderFromDB);
            unitOfWork.Complete();
            TempData["Update"] = "Item has Updated Successfully";
			return RedirectToAction("Details","Order" , new {orderid = orderFromDB.Id});
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProccess()
        {
            unitOfWork.Order.UpdateOrderStatus(OrderViewModel.OrderHeader.Id, ShoppingCart.Utilities.SD.Proccessing, null);
            unitOfWork.Complete();
			TempData["Update"] = "Item has Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderViewModel.OrderHeader.Id});

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
            var orderFromDb = unitOfWork.Order.GetFirstOrDefault(x=> x.Id == OrderViewModel.OrderHeader.Id);
            orderFromDb.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
            orderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;
            orderFromDb.OrderStatus = SD.Shipped;
            orderFromDb.ShippingDate = DateTime.Now;
			unitOfWork.Order.Update(orderFromDb);
			unitOfWork.Complete();
			TempData["Update"] = "Item has Shipped Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderViewModel.OrderHeader.Id });

		}


        public IActionResult CancelOrder()
        {
			var orderFromDb = unitOfWork.Order.GetFirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);
            if (orderFromDb.PaymentStatus == SD.Approve)
            {

                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderFromDb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);
				unitOfWork.Order.UpdateOrderStatus(orderFromDb.Id, SD.Cancelled, SD.Refund);

			}
			else
            {

		    	unitOfWork.Order.UpdateOrderStatus(orderFromDb.Id, SD.Cancelled, SD.Cancelled);
            }
			unitOfWork.Complete();

			TempData["Update"] = "Item has Canceled Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderViewModel.OrderHeader.Id });
		}
	}
}
