namespace BasketService.API.Repositories
{
    public class BasketRepository : IBasketRepository<RoomBasketViewModel>
    {
        protected IConnectionMultiplexer _connectionMultiplexer;
        protected IDatabase RepositoryContext { get; set; }
        protected ISubscriber Subscriber { get; set; }

        public BasketRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            RepositoryContext = connectionMultiplexer.GetDatabase();
            Subscriber = connectionMultiplexer.GetSubscriber();
        }

        public RoomBasketViewModel? DeleteStringKey(string key) => JsonConvert.DeserializeObject<RoomBasketViewModel>(RepositoryContext.StringGetDelete(key).ToString());
        public RoomBasketViewModel? GetStringKey(string key) => JsonConvert.DeserializeObject<RoomBasketViewModel>(RepositoryContext.StringGet(key).ToString());
        public RoomBasketViewModel UpdateStringKey(string key, RoomBasketViewModel entity) => JsonConvert.DeserializeObject<RoomBasketViewModel>(RepositoryContext.StringGetSet(key, JsonConvert.SerializeObject(entity)).ToString());

    }
}