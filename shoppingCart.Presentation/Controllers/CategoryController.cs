using Microsoft.AspNetCore.Mvc;
using shoppingCart.Presentation.Data;
using shoppingCart.Presentation.Models;

namespace shoppingCart.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create() { 
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // With any POST endpoint, it is a good practice to use ValidateAntiForgeryToken attribute to prevent CSRF attacks.
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

		[HttpGet]
		public IActionResult Edit(int? id)
		{
            if (id == null || id == 0)
            {
                NotFound();
            }

            var category = context.Categories.Find(id);
            return View(category);
		}



		[HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				context.Categories.Update(category);
				context.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(category);
		}

	}
}
