namespace OrderService.API.Repositories.Interfaces
{
    public interface ICacheOrderRepository
    {
        public IDatabase RepositoryContext { get; init; }
        public ISubscriber Subscriber { get; init; }
    }
}
