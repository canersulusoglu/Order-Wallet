namespace OrderService.API.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<OrderItem> RepositoryContext { get; init; }
        public OrderItemRepository(DatabaseContext databasecontext)
        {
            _databaseContext = databasecontext;
            RepositoryContext = databasecontext.Set<OrderItem>();
        }
        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
