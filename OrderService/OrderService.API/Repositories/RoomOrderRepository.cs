namespace OrderService.API.Repositories
{
    public class RoomOrderRepository : IRoomOrderRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<RoomOrder> RepositoryContext { get; init; }
        public RoomOrderRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            RepositoryContext = databaseContext.Set<RoomOrder>();
        }
        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
