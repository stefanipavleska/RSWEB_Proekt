using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCProekt.Data;
using MVCProekt.Areas.Identity.Data;


namespace MVCProekt.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MVCProektUser>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            MVCProektUser user = await UserManager.FindByEmailAsync("admin@mvcproekt.com");
            if (user == null)
            {
                var User = new MVCProektUser();
                User.Email = "admin@mvcproekt.com";
                User.UserName = "admin@mvcproekt.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            var roleCheck1 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck1) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }
            MVCProektUser user1 = await UserManager.FindByEmailAsync("user@mvcproekt.com");
            if (user1 == null)
            {
                var User = new MVCProektUser();
                User.Email = "user@mvcproekt.com";
                User.UserName = "user@mvcproekt.com";
                string userPWD = "User123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { await UserManager.AddToRoleAsync(User, "User"); }

            }
        }




        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCProektContext(
                serviceProvider.GetRequiredService<DbContextOptions<MVCProektContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Product.Any() || context.Cook.Any() || context.Category.Any() || context.Order.Any() || context.UserProduct.Any())
                {
                    return;   // DB has been seeded
                }

                context.Cook.AddRange(
                    new Cook
                    {
                        /* Id = 1, */
                        FirstName = "Lucas",
                        LastName = "Smith",
                        Gender = "Male"
                    },
                    new Cook
                    {
                        /* Id = 2, */
                        FirstName = "Oliver",
                        LastName = "Miller",
                        Gender = "Male"
                        },
                    new Cook
                    {
                         /* Id = 3, */
                         FirstName = "Emma",
                         LastName = "Clark",
                         Gender = "Female"
                    });
                context.SaveChanges();

                context.Category.AddRange(
                    new Category {  /* Id = 1, */ CategoryName = "Fast Food" },
                    new Category {  /* Id = 2, */ CategoryName = "Healty Food" },
                    new Category {  /* Id = 3, */ CategoryName = "Drink" },
                    new Category {  /* Id = 4, */ CategoryName = "Dessert" }
                    );
                context.SaveChanges();

                context.Product.AddRange(
                new Product
                {
                    //Id = 1,
                    ProductName = "Hamburger",
                    Price = 2,
                    ProductImage = "Hamburgerr.jpg",
                    CookId = 1
                },
                new Product
                {
                    //Id = 2,
                    ProductName = "Cheeseburger",
                    Price = 3,
                    ProductImage = "Cheeseburger.jpg",
                    CookId = 1
                },
                new Product
                {
                    //Id = 3,
                    ProductName = "Chicken burger",
                    Price = 4,
                    ProductImage = "Chickenburger.jpg",
                    CookId = 2
                },
                new Product
                {
                    //Id = 4,
                    ProductName = "Coca Cola",
                    Price = 2,
                    ProductImage = "CocaCola.jpg",
                    CookId = 3
                },
                 new Product
                 {
                     //Id = 5,
                     ProductName = "French Fries",
                     Price = 3,
                     ProductImage = "Frenchfries.jpg",
                     CookId = 3
                 },
                  new Product
                  {
                      //Id = 6,
                      ProductName = "Fried chicken",
                      Price = 4,
                      ProductImage = "Friedchicken.jpg",
                      CookId = 2
                  },
                   new Product
                   {
                       //Id = 7,
                       ProductName = "Ice Cream",
                       Price = 2,
                       ProductImage = "Icecream.jpg",
                       CookId = 2
                   },
                    new Product
                    {
                        //Id = 8,
                        ProductName = "Pepsi",
                        Price = 2,
                        ProductImage = "Pepsi.png",
                        CookId = 1
                    },
                     new Product
                     {
                         //Id = 9,
                         ProductName = "Pizza",
                         Price = 5,
                         ProductImage = "Pizza.jpg",
                         CookId = 1
                     },
                      new Product
                      {
                          //Id = 10,
                          ProductName = "Salad",
                          Price = 3,
                          ProductImage = "Salad.jpg",
                          CookId = 3
                      },
                       new Product
                       {
                           //Id = 11,
                           ProductName = "Starbucks coffee",
                           Price = 4,
                           ProductImage = "Starbucks.jpg",
                           CookId = 2
                       },
                        new Product
                        {
                            //Id = 12,
                            ProductName = "Subway",
                            Price = 4,
                            ProductImage = "Subway.jpg",
                            CookId = 2
                        }
                );
                context.SaveChanges();

                context.Order.AddRange(
                    new Order
                    {
                        /* Id = 1, */
                        ProductId = 1,
                        Username = "stefanipavleska",
                        PhoneNumber = 070999999,
                        Address = "Ul. Mis Son br. 28A",
                        DateOrder = DateTime.Parse("2023-6-26")
                    },
                       new Order
                       {
                           /* Id = 2, */
                           ProductId = 5,
                           Username = "petkopetkovski",
                           PhoneNumber = 071555555,
                           Address = "Ul. Blaze Markovski br. 13",
                           DateOrder = DateTime.Parse("2023-6-18")
                       },
                          new Order
                          {
                              /* Id = 3, */
                              ProductId = 6,
                              Username = "milemilevski",
                              PhoneNumber = 075111111,
                              Address = "Ul. Petko Perovski br. 15",
                              DateOrder = DateTime.Parse("2023-6-30")
                          }
                    );
                context.SaveChanges();

                context.UserProduct.AddRange(
                    new UserProduct { /*Id = 1*/ Username = "stefanipavleska", ProductId = 1 },
                    new UserProduct { /*Id = 1*/ Username = "petkopetkovski", ProductId = 2 },
                    new UserProduct { /*Id = 1*/ Username = "milemilevski", ProductId = 3 }
                    );
                context.SaveChanges();

                context.ProductCategory.AddRange(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 2, CategoryId = 1 },
                new ProductCategory { ProductId = 3, CategoryId = 1 },
                new ProductCategory { ProductId = 4, CategoryId = 3 },
                new ProductCategory { ProductId = 5, CategoryId = 1 },
                new ProductCategory { ProductId = 6, CategoryId = 1 },
                new ProductCategory { ProductId = 7, CategoryId = 4 },
                new ProductCategory { ProductId = 8, CategoryId = 3 },
                new ProductCategory { ProductId = 9, CategoryId = 1 },
                new ProductCategory { ProductId = 10, CategoryId = 2 },
                new ProductCategory { ProductId = 11, CategoryId = 3 },
                new ProductCategory { ProductId = 12, CategoryId = 1 }
                );
                context.SaveChanges();
            }
        }
    }
}
