using Microsoft.EntityFrameworkCore;
using shoppingCart.DataAcess.Data;
using shoppingCart.DataAcess.Impementation;
using shoppingCart.Entities.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ShoppingCart.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<Stripex>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser , IdentityRole>(options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(3))
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders().AddDefaultUI();
builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
Stripe.StripeConfiguration.ApiKey = app.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
