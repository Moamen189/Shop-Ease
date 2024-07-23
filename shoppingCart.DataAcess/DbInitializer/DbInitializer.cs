using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shoppingCart.DataAcess.Data;
using shoppingCart.Entities.Models;
using ShoppingCart.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.DataAcess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext Context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(ApplicationDbContext Context, UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            this.Context = Context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void Initialize()
        {
            //Migration 

            try
            {
                if (Context.Database.GetPendingMigrations().Count() > 0)
                {
                    Context.Database.Migrate();
                }
            }
            catch (Exception ex) {

                throw;
            }

            //Roles
            if (!roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            }

            //User

            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin@shop.com",
                Email = "Admin@shop.com",
                Name = "Adminstrator",
                PhoneNumber = "1234567890",
                Address= "Suez"
            } , "P@$$w0rd").GetAwaiter().GetResult();

            ApplicationUser user = Context.ApplicationUsers.FirstOrDefault(u => u.Email == "Admin@shop.com");
            userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();
            
            return;
        }
    }
}
