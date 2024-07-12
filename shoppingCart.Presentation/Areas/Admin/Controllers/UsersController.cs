using Microsoft.AspNetCore.Mvc;
using shoppingCart.DataAcess.Data;
using System.Security.Claims;

namespace shoppingCart.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}
