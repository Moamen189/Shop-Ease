using shoppingCart.DataAcess.Data;
using shoppingCart.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.DataAcess.Impementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        public ICategoryRepository Category {  get; private set; }

        public IProductRepository Product { get; private set; }

		public IShoppingCartDetailsRepository ShoppingCartDetails { get; private set; }

		public IOrderDetailRepository OrderDetails { get; private set; }

		public IOrderRepository Order { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ShoppingCartDetails = new ShoppingCartDetailsRepository(context);
            OrderDetails = new OrderDetailRepository(context);
            Order = new OrderHeaderRepository(context);

        }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
