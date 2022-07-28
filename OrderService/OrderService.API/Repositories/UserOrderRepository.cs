namespace OrderService.API.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<UserOrder> RepositoryContext { get; init; }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
