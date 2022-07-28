namespace OrderService.API
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public virtual DbSet<RoomOrder>? RoomOrders { get; set; }
        public virtual DbSet<UserOrder>? UserOrders { get; set; }
        public virtual DbSet<UserOrderItem>? UserOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomOrder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.HasMany(x => x.Users).WithOne(x => x.RoomOrder);
                entity.Property(x => x.EmployeeEmail).IsRequired();
                entity.Property(x => x.RoomName).IsRequired();
                entity.Property(x => x.ConfirmedBasketUserEmail).IsRequired();
                entity.Property(x => x.ConfirmedBasketUserName).IsRequired();
            });

            modelBuilder.Entity<UserOrder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.HasOne(x => x.RoomOrder).WithMany(x => x.Users);
                entity.HasMany(x => x.Products).WithOne(x => x.UserOrder);
                entity.Property(x => x.UserEmail).IsRequired();
                entity.Property(x => x.UserName).IsRequired();
            });

            modelBuilder.Entity<UserOrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.HasOne(x => x.UserOrder).WithMany(x => x.Products);
                entity.Property(x => x.ProductQuantity).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.ProductPrice).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}