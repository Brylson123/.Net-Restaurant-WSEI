using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant
{   

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<Uzytkownik, IdentityRole>()
                .AddEntityFrameworkStores<RestaurantDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
                db.Database.Migrate();
                SeedData(scope.ServiceProvider);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }

        private static void SeedData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Uzytkownik>>();
            var dbContext = serviceProvider.GetRequiredService<RestaurantDbContext>();


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }


            if (!userManager.Users.Any(u => u.UserName == "admin@restaurant.com"))
            {
                var admin = new Uzytkownik
                {
                    UserName = "admin@restaurant.com",
                    Email = "admin@restaurant.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(admin, "Admin@123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }

            if (!dbContext.Kategorie.Any())
            {
                dbContext.Kategorie.AddRange(
                    new KategoriaDania { Nazwa = "Przystawki" },
                    new KategoriaDania { Nazwa = "Dania g³ówne" },
                    new KategoriaDania { Nazwa = "Desery" },
                    new KategoriaDania { Nazwa = "Napoje" }
                );

                dbContext.SaveChanges();
            }
        }
    }
}
