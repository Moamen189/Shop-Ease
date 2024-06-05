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


		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				NotFound();
			}

			var category = context.Categories.Find(id);
			return View(category);
		}



		[HttpPost]
		public IActionResult DeleteCategory(int? id)
		{


			var category = context.Categories.Find(id);
            if (category == null)
            {
                NotFound();
            }
            context.Categories.Remove(category);
			context.SaveChanges();
            TempData["message"] = "Category Deleted Successfully"; // TempData is a dictionary object that is used to share data between controller and view. It is a dictionary object that is derived from the TempDataDictionary class. TempData is used to store data for a short time, and it is removed after it is read.
            return RedirectToAction("Index");
        }

	}
}
