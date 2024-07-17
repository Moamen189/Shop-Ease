using shoppingCart.DataAcess.Data;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.DataAcess.Impementation
{
	public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderHeaderRepository(ApplicationDbContext context):base(context) 
        {
			_context = context;
		}
        public void Update(OrderHeader OrderHeader)
		{
			_context.OrderHeaders.Update(OrderHeader);
		}

		public void UpdateOrderStatus(int id, string OrderStatus, string PaymentSatus)
		{
			var order = _context.OrderHeaders.FirstOrDefault(x => x.Id ==id);
			if(order != null)
			{
				order.OrderStatus = OrderStatus;
				if(PaymentSatus != null)
				{

					order.PaymentStatus = PaymentSatus;
				}
			}
		}
	}
}
