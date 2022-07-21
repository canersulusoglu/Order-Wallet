namespace OrderService.API.IntegrationEvents.EventHandling
{
    public class BasketConfirmedIntegrationEventHandling : IIntegrationEventHandler<BasketConfirmedIntegrationEvent>
    {
        private readonly ICacheOrderRepository<RoomOrderViewModel> _cacheOrderRepository;

        public BasketConfirmedIntegrationEventHandling(ICacheOrderRepository<RoomOrderViewModel> cacheOrderRepository)
        {
            _cacheOrderRepository = cacheOrderRepository;
        }

        public Task Handle(BasketConfirmedIntegrationEvent @event)
        {
            // Basket.API 'den gelen sipariş oluşturma isteği
            if (@event.Id != Guid.Empty)
            {
                dynamic roomOrder = JObject.Parse(@event.RoomOrder);
                Console.WriteLine(roomOrder);

                _cacheOrderRepository.AddStringKey(roomOrder.RoomName, roomOrder);
            }
            return Task.CompletedTask;
        }
    }
}
