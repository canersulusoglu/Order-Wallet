namespace OrderService.API.Repositories.Interfaces
{
    public interface IUserOrderRepository
    {
        public DbSet<UserOrder> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
