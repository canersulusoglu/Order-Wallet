namespace OrderService.API
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public virtual DbSet<Order>? Orders { get; set; }
        public virtual DbSet<OrderItem>? OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.HasMany(x => x.OrderItems).WithOne(x => x.Order);
                entity.Property(x => x.UserEmail).IsRequired();
                entity.Property(x => x.RoomName).IsRequired();
                entity.Property(x => x.EmployeeEmail).IsRequired();
                entity.Property(x => x.EmployeeName).IsRequired();
                entity.Property(x => x.EmployeeSurname).IsRequired();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).UseIdentityColumn();
                entity.HasOne(x => x.Order).WithMany(x => x.OrderItems);
                entity.Property(x => x.ProductQuantity).IsRequired();
                entity.Property(x => x.PaymentQuantity).HasDefaultValue(0);
                entity.Property(x => x.ProductPrice).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}