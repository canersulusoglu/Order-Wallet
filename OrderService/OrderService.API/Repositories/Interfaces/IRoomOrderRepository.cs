namespace OrderService.API.Repositories.Interfaces
{
    public interface IRoomOrderRepository
    {
        public DbSet<RoomOrder> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
