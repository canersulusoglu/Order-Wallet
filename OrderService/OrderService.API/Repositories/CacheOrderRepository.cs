namespace OrderService.API.Repositories
{
    public class CacheOrderRepository : ICacheOrderRepository
    {
        protected IConnectionMultiplexer _connectionMultiplexer;
        public IDatabase RepositoryContext { get; init; }
        public ISubscriber Subscriber { get; init; }
        public CacheOrderRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            RepositoryContext = connectionMultiplexer.GetDatabase();
            Subscriber = connectionMultiplexer.GetSubscriber();
        }

        /*
        public List<string> ScanKeys(string match)
        {
            var schemas = new List<string>();
            int nextCursor = 0;
            do
            {
                RedisResult redisResult = RepositoryContext.Execute("SCAN", nextCursor.ToString(), "MATCH", match);
                var innerResult = (RedisResult[])redisResult;

                nextCursor = int.Parse((string)innerResult[0]);

                List<string> resultLines = ((string[])innerResult[1]).ToList();
                schemas.AddRange(resultLines);
            }
            while (nextCursor != 0);

            return schemas;
        }

        public RoomOrderViewModel? GetStringKey(string key) => JsonConvert.DeserializeObject<RoomOrderViewModel>(RepositoryContext.StringGet(key).ToString());

        public void AddStringKey(string key, RoomOrderViewModel entity) => RepositoryContext.StringAppend(key, JsonConvert.SerializeObject(entity));

        public RoomOrderViewModel? UpdateStringKey(string key, RoomOrderViewModel entity) => JsonConvert.DeserializeObject<RoomOrderViewModel>(RepositoryContext.StringGetSet(key, JsonConvert.SerializeObject(entity)).ToString());

        public RoomOrderViewModel? DeleteStringKey(string key) => JsonConvert.DeserializeObject<RoomOrderViewModel>(RepositoryContext.StringGetDelete(key).ToString());

        public void SubscribeToChannel(string channel) => Subscriber.Subscribe(channel);

        public void UnSubscribeFromChannel(string channel) => Subscriber.Unsubscribe(channel);
        */
    }
}
