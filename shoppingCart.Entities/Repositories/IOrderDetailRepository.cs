using shoppingCart.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.Entities.Repositories
{
	public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
	{
		void Update(OrderDetail OrderDetail);
		//void Remove(int id);
	}
}
