namespace BasketService.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        protected IConnectionMultiplexer _connectionMultiplexer;
        public IDatabase RepositoryContext { get; init; }
        public ISubscriber Subscriber { get; init; }

        public BasketRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            RepositoryContext = connectionMultiplexer.GetDatabase();
            Subscriber = connectionMultiplexer.GetSubscriber();
        }
    }
}