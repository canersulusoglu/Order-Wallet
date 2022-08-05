using Microsoft.EntityFrameworkCore;
using WalletService.API.Models;

namespace WalletService.API
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public virtual DbSet<UserWallet>? UserOrders { get; set; }
        public virtual DbSet<UserWalletItem>? UserOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserWallet>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.HasMany(x => x.Products).WithOne(x => x.UserOrder);
                entity.Property(x => x.UserEmail).IsRequired();
                entity.Property(x => x.UserName).IsRequired();
                entity.Property(x => x.DebtAmount).IsRequired();
                entity.Property(x => x.PaymentAmount).IsRequired();
            });

            modelBuilder.Entity<UserWalletItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).UseIdentityColumn();
                entity.HasOne(x => x.UserOrder).WithMany(x => x.Products);
                entity.Property(x => x.ProductQuantity).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.ProductName).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
