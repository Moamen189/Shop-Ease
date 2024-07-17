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
    public class ApplicationUserRepository : GenericRepository<ApplicationUser> , IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(ApplicationDbContext context):base(context)
        {
            this.context = context;
        }
    }
}
