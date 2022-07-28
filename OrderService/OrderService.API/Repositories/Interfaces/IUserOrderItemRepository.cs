namespace OrderService.API.Repositories.Interfaces
{
    public interface IUserOrderItemRepository
    {
        public DbSet<UserOrderItem> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
