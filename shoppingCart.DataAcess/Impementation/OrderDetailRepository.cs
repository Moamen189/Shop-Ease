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
	public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDbContext context;

		public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
			this.context = context;
		}
        public void Update(OrderDetail OrderDetail)
		{
			context.OrderDetails.Update(OrderDetail);
		}
	}
}
