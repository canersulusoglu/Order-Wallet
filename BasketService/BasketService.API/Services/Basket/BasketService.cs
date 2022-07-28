namespace BasketService.API.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
     
        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository=basketRepository;
        }

        public async Task<RoomBasket> GetBasket(string basketUserEmail)
        {
            RedisValue basket = await _basketRepository.RepositoryContext.StringGetAsync(basketUserEmail);
            if (basket.IsNullOrEmpty)
            {
                throw new BasketNotFoundException();
            }
            return JsonConvert.DeserializeObject<RoomBasket>(basket.ToString());
        }

        public async Task UpdateBasket(string roomName, string confirmedBasketUserEmail, string confirmedBasketUserName, UpdateBasketViewModel basketUsers)
        {
            RoomBasket updatedBasket = new RoomBasket
            {
                RoomName = roomName,
                ConfirmedBasketUserEmail = confirmedBasketUserEmail,
                ConfirmedBasketUserName = confirmedBasketUserName,
                Users = basketUsers.Users,
            };
            var storedValueBasket = JsonConvert.SerializeObject(updatedBasket);
            await _basketRepository.RepositoryContext.StringSetAsync(confirmedBasketUserEmail, storedValueBasket);
        }

        public async Task DeleteBasket(string basketUserEmail)
        {
            RedisValue deletedBasket = await _basketRepository.RepositoryContext.StringGetDeleteAsync(basketUserEmail);
            if (deletedBasket.IsNullOrEmpty)
            {
                throw new BasketNotFoundException();
            }
        }
    }
}
