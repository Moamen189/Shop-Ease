using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.Entities.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IShoppingCartDetailsRepository ShoppingCartDetails { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderRepository Order { get; }
        IApplicationUserRepository ApplicationUser { get; }

        int Complete();
    }
}
