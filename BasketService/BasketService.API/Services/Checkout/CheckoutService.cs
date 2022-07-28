namespace BasketService.API.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IEventBus _eventBus;

        public CheckoutService(IBasketRepository basketRepository, IEventBus eventBus)
        {
            _basketRepository = basketRepository;
            _eventBus = eventBus;
        }
        public async Task confirmBasket(string basketUserEmail, string basketUserName, string orderMessage)
        {
            RedisValue basket = await _basketRepository.RepositoryContext.StringGetAsync(basketUserEmail);
            if (basket.IsNullOrEmpty)
            {
                throw new BasketNotFoundException();
            }

            RoomBasket roomBasket = JsonConvert.DeserializeObject<RoomBasket>(basket.ToString());
            roomBasket.OrderId = Guid.NewGuid().ToString();
            roomBasket.OrderMessage = orderMessage;
            roomBasket.ConfirmedBasketUserEmail = basketUserEmail;
            roomBasket.ConfirmedBasketUserName = basketUserName;
            roomBasket.OrderDate = DateTime.UtcNow;

            string roomBasketJSON = JsonConvert.SerializeObject(roomBasket);
            var eventMessage = new BasketConfirmedIntegrationEvent { RoomBasketAndOrder = roomBasketJSON };
            try
            {
                _eventBus.Publish(eventMessage);
                await _basketRepository.RepositoryContext.StringGetDeleteAsync(basketUserEmail);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
