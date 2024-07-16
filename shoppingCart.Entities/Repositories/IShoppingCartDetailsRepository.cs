using shoppingCart.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.Entities.Repositories
{
	public interface IShoppingCartDetailsRepository : IGenericRepository<ShoppingCartDetails>
	{
		int increaseCount(ShoppingCartDetails shoppingCartDetails, int count);
		int decreaseCount(ShoppingCartDetails shoppingCartDetails, int count);
    }
}
