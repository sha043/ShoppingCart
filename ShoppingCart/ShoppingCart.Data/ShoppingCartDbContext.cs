using Microsoft.EntityFrameworkCore;
using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class ShoppingCartDbContext:DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(O => O.OrderDetailsList);
        }
        public DbSet<Order>? Order { get; set; }
        public DbSet<OrderDetails>? OrderDetails { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<Customer>? Customer { get; set; }
    }
}
