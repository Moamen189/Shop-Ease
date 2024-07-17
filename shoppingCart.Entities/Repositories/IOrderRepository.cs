using shoppingCart.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.Entities.Repositories
{
	public interface IOrderRepository : IGenericRepository<OrderHeader>
	{
		void Update(OrderHeader OrderHeader);
		void UpdateOrderStatus(int id , string OrderStatus, string PaymentSatus );
	}
}
