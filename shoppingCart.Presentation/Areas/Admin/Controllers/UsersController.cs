using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Data;
using ShoppingCart.Utilities;
using System.Security.Claims;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _Context;

        public UsersController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            return View(_Context.ApplicationUsers.Where(x => x.Id != userId).ToList());
        }

        public IActionResult LockUnlock(string? id)
        {
            var user = _Context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user ==  null)
            {
                return NotFound();
            }
            if(user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _Context.SaveChanges();
            return RedirectToAction("Index", "Users", new { Areas = "Admin" });
        }
    }
}
