using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Data
{
    public class NopAppContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public NopAppContext()
        {

        }
        public NopAppContext(DbContextOptions<NopAppContext> options) : base(options)
        {

        } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MealIngredient>()
                .HasKey(nameof(MealIngredient.MealId), nameof(MealIngredient.IngredientId));

            builder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Kitchen>()
                .Property(k => k.Id)
                .ValueGeneratedOnAdd();
        }

        public DbSet<Kitchen> Kitchens { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

    }
}
