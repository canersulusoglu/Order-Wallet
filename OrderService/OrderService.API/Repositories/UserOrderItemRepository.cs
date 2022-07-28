namespace OrderService.API.Repositories
{
    public class UserOrderItemRepository : IUserOrderItemRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<UserOrderItem> RepositoryContext { get; init; }
        public UserOrderItemRepository(DatabaseContext databasecontext)
        {
            _databaseContext = databasecontext;
            RepositoryContext = databasecontext.Set<UserOrderItem>();
        }
        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
