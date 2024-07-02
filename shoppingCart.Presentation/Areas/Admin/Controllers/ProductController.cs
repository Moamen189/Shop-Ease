using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shoppingCart.Entities.Models;
using shoppingCart.Entities.Repositories;
using shoppingCart.Entities.ViewModels;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {

            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetData()
		{
            var products = unitOfWork.Product.GetAll(IncludeWord:"Category");
			return Json(new {data = products});
		}
		[HttpGet]
        public IActionResult Create()
        {
            ProductVM producVM = new ProductVM()
            {
                Product = new Product(),
                CategorieList = unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(producVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // With any POST endpoint, it is a good practice to use ValidateAntiForgeryToken attribute to prevent CSRF attacks.
        public IActionResult Create(ProductVM productVM , IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(rootPath, @"Images\Products");
                    var extension = Path.GetExtension(file.FileName);
                    using(var fileStream = new FileStream(Path.Combine(Upload,fileName+extension) , FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.Image = @"Images\Products\" + fileName + extension;
                }
                unitOfWork.Product.Add(productVM.Product);
                unitOfWork.Complete();
                TempData["Create"] = "Product Created Successfully";

                return RedirectToAction("Index");
            }

            return View(productVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
			ProductVM producVM = new ProductVM()
			{
				Product = unitOfWork.Product.GetFirstOrDefault(x => x.Id == id),
				CategorieList = unitOfWork.Category.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				})
			};
			return View(producVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM , IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(rootPath, @"Images\Products");
                    var extension = Path.GetExtension(file.FileName);
                    if(productVM.Product.Image != null)
                    {
                        var imagePath = Path.Combine(rootPath, productVM.Product.Image.TrimStart('\\'));
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(Upload, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.Image = @"Images\Products\" + fileName + extension;
                }
                unitOfWork.Product.Update(productVM.Product);
                unitOfWork.Complete();
                TempData["Update"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }

            return View(productVM.Product);
        }


        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {

            var rootPath = webHostEnvironment.WebRootPath;
            var product = unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unitOfWork.Product.Remove(product);
            var imagePath = Path.Combine(rootPath, product.Image.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            unitOfWork.Complete();
            TempData["Delete"] = "Product Deleted Successfully"; // TempData is a dictionary object that is used to share data between controller and view. It is a dictionary object that is derived from the TempDataDictionary class. TempData is used to store data for a short time, and it is removed after it is read.
            return Json(new { success = true, message = "File has been deleted" });
        }
    }
}
