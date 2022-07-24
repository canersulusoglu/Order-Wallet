namespace BasketService.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        public IDatabase RepositoryContext { get; init; }
        public ISubscriber Subscriber { get; init; }
    }
}