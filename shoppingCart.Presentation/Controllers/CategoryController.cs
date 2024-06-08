using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Data;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;

namespace shoppingCart.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
         
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = unitOfWork.Category.GetAll();
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
                unitOfWork.Category.Add(category);
                unitOfWork.Complete();
				TempData["Create"] = "Category Created Successfully";

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

            var category = unitOfWork.Category.GetFirstOrDefault(x=>x.Id == id);
            return View(category);
		}



		[HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
                unitOfWork.Category.Update(category); 
                unitOfWork.Complete();
				TempData["Update"] = "Category Updated Successfully";
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

			var category = unitOfWork.Category.GetFirstOrDefault(x=> x.Id == id);
			return View(category);
		}



		[HttpPost]
		public IActionResult DeleteCategory(int? id)
		{


			var category = unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                NotFound();
            }
            unitOfWork.Category.Remove(category);
            unitOfWork.Complete();
            TempData["Delete"] = "Category Deleted Successfully"; // TempData is a dictionary object that is used to share data between controller and view. It is a dictionary object that is derived from the TempDataDictionary class. TempData is used to store data for a short time, and it is removed after it is read.
            return RedirectToAction("Index");
        }

	}
}
