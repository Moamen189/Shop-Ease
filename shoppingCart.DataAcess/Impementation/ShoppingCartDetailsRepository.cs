using shoppingCart.DataAcess.Data;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.DataAcess.Impementation
{
	public class ShoppingCartDetailsRepository : GenericRepository<ShoppingCartDetails> ,  IShoppingCartDetailsRepository
	{
		private readonly ApplicationDbContext _context;

		public ShoppingCartDetailsRepository(ApplicationDbContext context) : base(context) 
        {
			_context = context;
		}

        public int decreaseCount(ShoppingCartDetails shoppingCartDetails, int count)
        {
            shoppingCartDetails.Count -= count;
            return shoppingCartDetails.Count;

        }

        public int increaseCount(ShoppingCartDetails shoppingCartDetails, int count)
        {
            shoppingCartDetails.Count -= count;
            return shoppingCartDetails.Count;
        }
    }
}
