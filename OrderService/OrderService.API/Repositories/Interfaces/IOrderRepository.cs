namespace OrderService.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public DbSet<Order> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
