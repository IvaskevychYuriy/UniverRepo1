using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Models.Entities;

namespace WebStore.DAL.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, int>
    {
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ProductCategory>(b =>
            {
                b.ToTable("ProductCategory");
                b.HasKey(pc => pc.Id);
                b.Property(pc => pc.Name).IsRequired().HasMaxLength(50);
            });

            builder.Entity<ProductItem>(b =>
            {
                b.ToTable("ProductItem");
                b.HasKey(pi => pi.Id);
                b.Property(pi => pi.Name).IsRequired().HasMaxLength(200);
                b.Property(pi => pi.Price).IsRequired();
                b.Property(pi => pi.Description).IsRequired(false);
                b.Property(pi => pi.PictureUrl).IsRequired(false).HasMaxLength(2000);
                b.HasOne(pi => pi.Category).WithMany(pc => pc.Products).HasForeignKey(pi => pi.CategoryId);
            });

            builder.Entity<CartItem>(b =>
            {
                b.ToTable("CartItem");
                b.HasKey(pi => pi.Id);
                b.Property(pi => pi.Quantity).IsRequired();
                b.HasOne(pi => pi.Product).WithMany(pc => pc.CartItems).HasForeignKey(pi => pi.ProductId);
                b.HasOne(pi => pi.Order).WithMany(pc => pc.CartItems).HasForeignKey(pi => pi.OrderId);
            });

            builder.Entity<Order>(b =>
            {
                b.ToTable("Order");
                b.HasKey(pi => pi.Id);
                b.Property(pi => pi.State).IsRequired();
                b.Property(pi => pi.TotalPrice).IsRequired();
                b.HasOne(pi => pi.User).WithMany(pc => pc.Orders).HasForeignKey(pi => pi.UserId);
            });
        }
    }
}
