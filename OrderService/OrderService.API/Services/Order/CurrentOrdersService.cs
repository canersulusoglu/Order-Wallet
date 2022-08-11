namespace OrderService.API.Services.Orders
{
    public class CurrentOrdersService : ICurrentOrdersService
    {
        private readonly ICacheOrderRepository _cacheOrderRepository;
        private readonly IRoomOrderRepository _orderRepository;

        public CurrentOrdersService(ICacheOrderRepository cacheOrderRepository, IRoomOrderRepository orderRepository)
        {
            _cacheOrderRepository = cacheOrderRepository;
            _orderRepository = orderRepository;
        }

        public async Task<string[]> GetUserOrderIds(string userEmail)
        {
            var userOrderIdsKey = "User" + userEmail + "OrderIds";
            RedisValue[] orderIds = await _cacheOrderRepository.RepositoryContext.ListRangeAsync(userOrderIdsKey, 0, -1);
            return Array.ConvertAll(orderIds, x => x.ToString());
        }

        public async Task<string[]> GetOrderIds()
        {
            RedisValue[] orderIds = await _cacheOrderRepository.RepositoryContext.ListRangeAsync("OrderIds", 0, -1);
            return Array.ConvertAll(orderIds, x => x.ToString());
        }

        public async Task<RoomOrder> GetUserOrder(string orderId)
        {
            RedisValue orderValue = await _cacheOrderRepository.RepositoryContext.StringGetAsync(orderId);
            if (orderValue.IsNullOrEmpty)
            {
                throw new OrderNotFoundException();
            }
            return JsonConvert.DeserializeObject<RoomOrder>(orderValue.ToString());
        }

        public async Task<List<RoomOrder>> GetUserCurrentOrders(string userEmail)
        {
            List<RoomOrder> userCurrentOrders = new List<RoomOrder>();
            string[] userOrderIds = await GetUserOrderIds(userEmail);
            foreach (string orderId in userOrderIds)
            {
                RoomOrder userOrder = await GetUserOrder(orderId);
                userCurrentOrders.Add(userOrder);
            }
            return userCurrentOrders;
            //return JsonConvert.DeserializeObject<List<RoomOrder>>(userCurrentOrders.ToString());
        }

        public async Task<List<RoomOrder>> GetAllCurrentOrders()
        {
            List<RoomOrder> currentOrders = new List<RoomOrder>();
            string[] allOrderIds = await GetOrderIds();
            foreach (string orderId in allOrderIds)
            {
                RoomOrder userOrder = await GetUserOrder(orderId);
                currentOrders.Add(userOrder);
            }
            return currentOrders;
        }

        public async Task FinishOrder(string orderId, string employeeEmail)
        {
            RoomOrder roomOrder = await GetUserOrder(orderId);

            List<UserOrder> orderUsers = new List<UserOrder>();
            foreach (UserOrder userOrder in roomOrder.Users)
            {
                UserOrder orderUser = new UserOrder
                {
                    UserEmail = userOrder.UserEmail,
                    UserName = userOrder.UserName,
                    Products = userOrder.Products.Select(item => new UserOrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductQuantity = item.ProductQuantity,
                        ProductPrice = item.ProductPrice
                    }).ToList()
                };

                orderUsers.Add(orderUser);
            }

            RoomOrder order = new RoomOrder
            {
                OrderId = roomOrder.OrderId,
                EmployeeEmail = employeeEmail,
                RoomName = roomOrder.RoomName,
                ConfirmedBasketUserEmail = roomOrder.ConfirmedBasketUserEmail,
                ConfirmedBasketUserName = roomOrder.ConfirmedBasketUserName,
                Users = orderUsers
            };

            await _orderRepository.RepositoryContext.AddAsync(order);
            await _orderRepository.SaveChanges();

            // Remove order from cache memory
            await _cacheOrderRepository.RepositoryContext.StringGetDeleteAsync(roomOrder.OrderId);
            foreach (UserOrder userBasket in roomOrder.Users)
            {
                var userOrderIdsKey = "User" + userBasket.UserEmail + "OrderIds";
                await _cacheOrderRepository.RepositoryContext.ListRemoveAsync(userOrderIdsKey, roomOrder.OrderId);
            }
            await _cacheOrderRepository.RepositoryContext.ListRemoveAsync("OrderIds", roomOrder.OrderId);

            // Publish message to Wallet.API here
        }

        public async Task FinishOrders(string[] orderIds, string employeeEmail)
        {
            foreach (string orderId in orderIds)
            {
                await FinishOrder(orderId, employeeEmail);
            }
        }
    }
}
