using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using MVCProekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVCProekt.Areas.Identity.Data;



//using MVCProekt.Areas.Identity.Data;

namespace MVCProekt.Data
{
    public class MVCProektContext : IdentityDbContext<MVCProektUser>
    {
        public MVCProektContext (DbContextOptions<MVCProektContext> options)
            : base(options)
        {
        }

        public DbSet<MVCProekt.Models.Product> Product { get; set; } = default!;

        public DbSet<MVCProekt.Models.Cook>? Cook { get; set; }

        public DbSet<MVCProekt.Models.Category>? Category { get; set; }

        public DbSet<MVCProekt.Models.Order>? Order { get; set; }

        public DbSet<MVCProekt.Models.UserProduct>? UserProduct { get; set; }
        public DbSet<MVCProekt.Models.ProductCategory>? ProductCategory { get; set; }

        /*  protected override void OnModelCreating(ModelBuilder builder)
          {
              builder.Entity<Product>()
                  .HasOne<Cook>(a => a.Cook)
                  .WithMany(a => a.Products)
                  .HasForeignKey(p => p.CookId);

              builder.Entity<ProductCategory>()
                  .HasOne<Product>(a => a.Product)
                  .WithMany(a => a.Categories) 
                  .HasForeignKey(p => p.ProductId);

              builder.Entity<ProductCategory>()
                  .HasOne<Category>(p => p.Category)
                  .WithMany(p => p.Categories) 
                  .HasForeignKey(p => p.CategoryId);

              builder.Entity<Order>()
                  .HasOne<Product>(p => p.Product)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(p => p.ProductId);

              builder.Entity<UserProduct>()
                  .HasOne<Product>(p => p.Product)
                  .WithMany(p => p.UserProducts)
                  .HasForeignKey(p => p.ProductId);
          } */

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
