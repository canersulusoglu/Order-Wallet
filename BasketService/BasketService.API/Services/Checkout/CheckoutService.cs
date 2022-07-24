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
        public async Task confirmBasket(string userId, string roomName)
        {
            RedisValue roomBasket = await _basketRepository.RepositoryContext.StringGetAsync(roomName);
            if (roomBasket.IsNullOrEmpty)
            {
                throw new BasketNotFoundException();
            }

            RoomBasketViewModel roomBasketVM = JsonConvert.DeserializeObject<RoomBasketViewModel>(roomBasket);
            roomBasketVM.ConfirmedBasketUserId = userId;
            roomBasketVM.RoomName = roomName;
            string roomOrder = JsonConvert.SerializeObject(roomBasketVM);
            /*
            RoomBasketViewModel roomBasket = new RoomBasketViewModel
            {
                ConfirmedBasketUserId = userId,
                RoomName = "101",
                UserBaskets = new List<UserBasketViewModel>
                {
                    new UserBasketViewModel
                    {
                        UserId="205",
                        UserBasketItems=new List<UserBasketItemViewModel>
                        {
                            new UserBasketItemViewModel{ProductName="Tea", ProductId="1", ProductPrice=1.5m, ProductQuantity=3},
                            new UserBasketItemViewModel{ProductName="Pisküvi", ProductId="3", ProductPrice=1.75m, ProductQuantity=2}
                        }
                    }
                }
            };

            string roomBasket = JsonConvert.SerializeObject(roomBasket.ToString());
            */

            var eventMessage = new BasketConfirmedIntegrationEvent(roomOrder);
            try
            {
                _eventBus.Publish(eventMessage);
                await _basketRepository.RepositoryContext.StringGetDeleteAsync(roomName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
