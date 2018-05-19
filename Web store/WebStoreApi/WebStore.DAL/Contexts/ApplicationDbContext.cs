using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Models.Entities;

namespace WebStore.DAL.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, int>
    {
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductCategory>(b =>
            {
                b.ToTable("ProductCategory");
                b.HasKey(pc => pc.Id);
                b.Property(pc => pc.Name).IsRequired().HasMaxLength(50);
            });

            builder.Entity<ProductSubCategory>(b =>
            {
                b.ToTable("ProductSubCategory");
                b.HasKey(psc => psc.Id);
                b.Property(psc => psc.Name).IsRequired().HasMaxLength(50);
                b.HasOne(psc => psc.Category).WithMany(pc => pc.SubCategories).HasForeignKey(psc => psc.CategoryId);
            });

            builder.Entity<ProductItem>(b =>
            {
                b.ToTable("ProductItem");
                b.HasKey(pi => pi.Id);
                b.Property(pi => pi.Name).IsRequired().HasMaxLength(200);
                b.Property(pi => pi.Price).IsRequired();
                b.Property(pi => pi.Description).IsRequired(false);
                b.Property(pi => pi.PictureUrl).IsRequired(false).HasMaxLength(2000);
                b.HasOne(pi => pi.SubCategory).WithMany(psc => psc.Products).HasForeignKey(pi => pi.SubCategoryId);
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
                b.Property(pi => pi.TotalPrice).IsRequired();
                b.HasOne(pi => pi.User).WithMany(pc => pc.Orders).HasForeignKey(pi => pi.UserId);
            });

            builder.Entity<OrderHistory>(b =>
            {
                b.ToTable("OrderHistory");
                b.HasKey(oh => oh.Id);
                b.Property(oh => oh.State).IsRequired();
                b.Property(oh => oh.StateChangeDate).IsRequired().HasDefaultValueSql("GETDATE()");
                b.HasOne(oh => oh.Order).WithMany(o => o.HistoryRecords).HasForeignKey(oh => oh.OrderId);
            });
        }
    }
}
