namespace OrderService.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<Order> RepositoryContext { get; init; }
        public OrderRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            RepositoryContext = databaseContext.Set<Order>();
        }
        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
