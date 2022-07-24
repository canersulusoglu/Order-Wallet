namespace OrderService.API.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public DbSet<OrderItem> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
