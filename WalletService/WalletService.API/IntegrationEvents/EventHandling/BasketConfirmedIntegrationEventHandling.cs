using Newtonsoft.Json;

namespace OrderService.API.IntegrationEvents.EventHandling
{
    public class BasketConfirmedIntegrationEventHandling : IIntegrationEventHandler<BasketConfirmedIntegrationEvent>
    {
        private readonly ICacheOrderRepository _cacheOrderRepository;

        public BasketConfirmedIntegrationEventHandling(ICacheOrderRepository cacheOrderRepository)
        {
            _cacheOrderRepository = cacheOrderRepository;
        }

        public async Task Handle(BasketConfirmedIntegrationEvent @event)
        {
            if (@event.Id != Guid.Empty)
            {
                RoomOrder roomOrder = JsonConvert.DeserializeObject<RoomOrder>(@event.RoomBasketAndOrder);

                // Sipariş değeri
                var storedValue = JsonConvert.SerializeObject(roomOrder);
                await _cacheOrderRepository.RepositoryContext.StringSetAsync(roomOrder.OrderId, storedValue);

                // Sipariş verilen diğer kullanıcılar
                foreach (UserOrder userOrder in roomOrder.Users)
                {
                    var userOrderIdsKey = "User" + userOrder.UserEmail + "OrderIds";
                    await _cacheOrderRepository.RepositoryContext.ListLeftPushAsync(userOrderIdsKey, roomOrder.OrderId);
                }

                // Sipariş veren kullanıcı
                var orderOwnerIdsKey = "User" + roomOrder.ConfirmedBasketUserEmail + "OrderIds";
                await _cacheOrderRepository.RepositoryContext.ListLeftPushAsync(orderOwnerIdsKey, roomOrder.OrderId);

                // Tüm siparişler
                await _cacheOrderRepository.RepositoryContext.ListLeftPushAsync("OrderIds", roomOrder.OrderId);

                // Kantinciye bildirim yollamak için
                var employeeChannel = "orderwallet-employees";
                await _cacheOrderRepository.Subscriber.PublishAsync(employeeChannel, storedValue);
            }
            else
            {
                Console.WriteLine("BasketConfirmedIntegrationEvent is null!");
            }
        }
    }
}
