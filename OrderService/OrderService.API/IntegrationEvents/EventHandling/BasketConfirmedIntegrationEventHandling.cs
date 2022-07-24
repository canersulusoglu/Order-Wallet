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
            // Request that is creating order from Basket.API checkoutBasket
            if (@event.Id != Guid.Empty)
            {
                dynamic roomOrder = JObject.Parse(@event.RoomOrder);
                string roomName = roomOrder.RoomName;
                string confirmedBasketUserId = roomOrder.ConfirmedBasketUserId;

                // Split room order to user orders
                List<UserOrderViewModel> userOrders = new List<UserOrderViewModel>();

                foreach (dynamic userBasket in roomOrder.UserBaskets)
                {
                    List<UserOrderItemViewModel> products = new List<UserOrderItemViewModel>();
                    foreach (dynamic basketItem in userBasket.UserBasketItems) 
                    {
                        products.Add(new UserOrderItemViewModel
                        {
                            ProductId = basketItem.ProductId,
                            ProductName = basketItem.ProductName,
                            ProductPrice = basketItem.ProductPrice,
                            ProductQuantity = basketItem.ProductQuantity
                        });
                    }
                    UserOrderViewModel userOrder = new UserOrderViewModel
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        UserId = userBasket.UserId,
                        CreatedOrderUserId = confirmedBasketUserId,
                        RoomName = roomName,
                        Products = products,
                        OrderDate = DateTime.UtcNow
                    };
                    userOrders.Add(userOrder);
                }

                var employeeChannel = "orderwallet-employees";
                foreach (UserOrderViewModel userOrder in userOrders)
                {
                    try
                    {
                        // Creating user orders
                        var storedValue = JsonConvert.SerializeObject(userOrder);
                        var roomOrderIdsKey = "Room" + roomName + "OrderIds";
                        var userOrderIdsKey = "User" + userOrder.UserId + "OrderIds";
                        await _cacheOrderRepository.RepositoryContext.SetAddAsync("Rooms", roomName);
                        await _cacheOrderRepository.RepositoryContext.ListLeftPushAsync(roomOrderIdsKey, userOrder.OrderId);
                        await _cacheOrderRepository.RepositoryContext.ListLeftPushAsync(userOrderIdsKey, userOrder.OrderId);
                        await _cacheOrderRepository.RepositoryContext.ListLeftPushAsync("OrderIds", userOrder.OrderId);
                        await _cacheOrderRepository.RepositoryContext.StringSetAsync(userOrder.OrderId, storedValue);
                        // Send a notification to employees
                        await _cacheOrderRepository.Subscriber.PublishAsync(employeeChannel, storedValue);
                    }
                    catch
                    {
                        Console.WriteLine("An error accured when creating user order!");
                    }
                }
            }
            else
            {
                Console.WriteLine("BasketConfirmedIntegrationEvent is null!");
            }
        }
    }
}
