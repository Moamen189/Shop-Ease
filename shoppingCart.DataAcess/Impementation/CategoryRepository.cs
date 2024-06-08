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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            this.context = context;
        }
        public void Update(Category category)
        {
            var categoryInDb = context.Categories.FirstOrDefault(x=> x.Id == category.Id);

            if (categoryInDb != null)
            {
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;
                categoryInDb.CreatedTime = DateTime.Now;
            }
        }
    }
}
