namespace BasketService.API.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
     
        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository=basketRepository;
        }

        public async Task DeleteBasket(string roomName)
        {
            RedisValue deletedBasket = await _basketRepository.RepositoryContext.StringGetDeleteAsync(roomName);
            if(deletedBasket.IsNullOrEmpty)
            {
                throw new BasketNotFoundException();
            }
        }

        public async Task<RoomBasketViewModel> GetBasket(string roomName)
        {
            RedisValue basket = await _basketRepository.RepositoryContext.StringGetAsync(roomName);
            if (basket.IsNullOrEmpty)
            {
                throw new BasketNotFoundException();
            }
            return JsonConvert.DeserializeObject<RoomBasketViewModel>(basket.ToString());
        }

        public async Task UpdateBasket(string roomName, RoomBasketViewModel roomBasketViewModel)
        {
            string storedValue = JsonConvert.SerializeObject(roomBasketViewModel);
            await _basketRepository.RepositoryContext.StringSetAsync(roomName, storedValue);
        }
    }
}
