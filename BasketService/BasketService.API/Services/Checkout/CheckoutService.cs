namespace BasketService.API.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IBasketRepository<RoomBasketViewModel> _roomBasketRepository;
        private readonly IEventBus _eventBus;

        public CheckoutService(IBasketRepository<RoomBasketViewModel> roomBasketRepository, IEventBus eventBus)
        {
            _roomBasketRepository = roomBasketRepository;
            _eventBus = eventBus;
        }
        public Task confirmBasket(string roomName)
        {
            /*
            RoomOrderViewModel? roomBasket = _roomBasketRepository.GetStringKey(roomName);
            if (roomBasket == null)
            {
                throw new BasketNotFoundException();
            }
            */

            RoomBasketViewModel test = new RoomBasketViewModel
            {
                RoomName = "101",
                UserBaskets = new List<UserBasketViewModel>
                {
                    new UserBasketViewModel
                    {
                        UserEmail="cscaner26@gmail.com",
                        UserBasketItems=new List<UserBasketItemViewModel>
                        {
                            new UserBasketItemViewModel{ProductName="Tea", ProductPrice=1.5m, ProductQuantity=3}
                        }
                    }
                }
            };

            string roomOrder = JsonConvert.SerializeObject(test);

            var eventMessage = new BasketConfirmedIntegrationEvent(roomOrder);
            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
