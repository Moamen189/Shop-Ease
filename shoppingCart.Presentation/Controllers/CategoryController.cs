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
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
