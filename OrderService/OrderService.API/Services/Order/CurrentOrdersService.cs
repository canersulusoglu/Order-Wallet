namespace OrderService.API.Services.Orders
{
    public class CurrentOrdersService : ICurrentOrdersService
    {
        private readonly ICacheOrderRepository _cacheOrderRepository;
        private readonly IOrderRepository _orderRepository;

        public CurrentOrdersService(ICacheOrderRepository cacheOrderRepository, IOrderRepository orderRepository)
        {
            _cacheOrderRepository = cacheOrderRepository;
            _orderRepository = orderRepository;
        }

        public async Task<string[]> GetRoomNamesHaveAnOrder()
        {
            RedisValue[] roomNames = await _cacheOrderRepository.RepositoryContext.SetMembersAsync("Rooms");
            return Array.ConvertAll(roomNames, x => x.ToString());
        }

        public async Task<string[]> GetRoomOrderIds(string roomName)
        {
            var roomOrderIdsKey = "Room" + roomName + "OrderIds";
            RedisValue[] roomOrderIds = await _cacheOrderRepository.RepositoryContext.ListRangeAsync(roomOrderIdsKey, 0, -1);
            return Array.ConvertAll(roomOrderIds, x => x.ToString());
        }

        public async Task<string[]> GetUserOrderIds(string userId)
        {
            var userOrderIdsKey = "User" + userId + "OrderIds";
            RedisValue[] orderIds = await _cacheOrderRepository.RepositoryContext.ListRangeAsync(userOrderIdsKey, 0, -1);
            return Array.ConvertAll(orderIds, x => x.ToString());
        }

        public async Task<string[]> GetOrderIds()
        {
            RedisValue[] orderIds = await _cacheOrderRepository.RepositoryContext.ListRangeAsync("OrderIds", 0, -1);
            return Array.ConvertAll(orderIds, x => x.ToString());
        }

        public async Task<UserOrderViewModel> GetUserOrder(string orderId)
        {
            RedisValue orderValue = await _cacheOrderRepository.RepositoryContext.StringGetAsync(orderId);
            if (orderValue.IsNullOrEmpty)
            {
                throw new OrderNotFoundException();
            }
            return JsonConvert.DeserializeObject<UserOrderViewModel>(orderValue.ToString());
        }

        public async Task<List<UserOrderViewModel>> GetUserCurrentOrders(string userId)
        {
            List<UserOrderViewModel> userCurrentOrders = new List<UserOrderViewModel>();
            string[] userOrderIds = await GetUserOrderIds(userId);
            foreach (string orderId in userOrderIds)
            {
                UserOrderViewModel userOrder = await GetUserOrder(orderId);
                userCurrentOrders.Add(userOrder);
            }
            return userCurrentOrders;
        }

        public async Task<List<UserOrderViewModel>> GetAllCurrentUserOrders()
        {
            List<UserOrderViewModel> currentOrders = new List<UserOrderViewModel>();
            string[] allOrderIds = await GetOrderIds();
            foreach (string orderId in allOrderIds)
            {
                UserOrderViewModel userOrder = await GetUserOrder(orderId);
                currentOrders.Add(userOrder);
            }
            return currentOrders;
        }

        public async Task<List<RoomOrderViewModel>> GetAllCurrentRoomOrders()
        {
            List<RoomOrderViewModel> roomOrders = new List<RoomOrderViewModel>();

            string[] roomNames = await GetRoomNamesHaveAnOrder();
            Dictionary<string, List<UserOrderViewModel>> roomOrdersByRoomName = new Dictionary<string, List<UserOrderViewModel>>();
            foreach (string roomName in roomNames)
            {
                List<UserOrderViewModel> orders = await GetRoomOrders(roomName);
                roomOrdersByRoomName.Add(roomName, orders);
            }

            foreach (KeyValuePair<string, List<UserOrderViewModel>> keyValue in roomOrdersByRoomName)
            {
                string roomName = keyValue.Key;
                List<UserOrderViewModel> orders = keyValue.Value;

                RoomOrderViewModel roomOrder = new RoomOrderViewModel
                {
                    ConfirmedBasketUserId = orders[0].CreatedOrderUserId,
                    RoomName = roomName,
                    UserOrders = orders
                };
                roomOrders.Add(roomOrder);
            }
            return roomOrders;
        }

        public async Task<List<UserOrderViewModel>> GetRoomOrders(string roomName)
        {
            List<UserOrderViewModel> roomOrders = new List<UserOrderViewModel>();
            string[] orderIds = await GetRoomOrderIds(roomName);
            foreach (string orderId in orderIds)
            {
                UserOrderViewModel userOrder = await GetUserOrder(orderId);
                roomOrders.Add(userOrder);
            }
            return roomOrders;
        }

        public async Task FinishUserOrder(string orderId, string employeeId)
        {
            UserOrderViewModel userOrder = await GetUserOrder(orderId);

            // Publish message to Wallet.API here

            List<OrderItem> products = userOrder.Products.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductQuantity = item.ProductQuantity,
                PaymentQuantity = 0,
                ProductPrice = item.ProductPrice
            }).ToList();

            Order order = new Order
            {
                EmployeeId = employeeId,
                UserId = userOrder.UserId,
                RoomName = userOrder.RoomName,
                CreatedTime = userOrder.OrderDate,
                UpdatedTime = userOrder.OrderDate,
                OrderItems = products
            };
            await _orderRepository.RepositoryContext.AddAsync(order);
            await _orderRepository.SaveChanges();

            // Remove order from cache memory
            var roomOrderIdsKey = "Room" + userOrder.RoomName + "OrderIds";
            var userOrderIdsKey = "User" + userOrder.UserId + "OrderIds";
            string[] roomOrderIds = await GetRoomOrderIds(userOrder.RoomName);
            if (roomOrderIds.Length == 1)
            {
                await _cacheOrderRepository.RepositoryContext.SetRemoveAsync("Rooms", userOrder.RoomName);
            }
            await _cacheOrderRepository.RepositoryContext.ListRemoveAsync(roomOrderIdsKey, userOrder.OrderId);
            await _cacheOrderRepository.RepositoryContext.ListRemoveAsync(userOrderIdsKey, userOrder.OrderId);
            await _cacheOrderRepository.RepositoryContext.ListRemoveAsync("OrderIds", userOrder.OrderId);
            await _cacheOrderRepository.RepositoryContext.StringGetDeleteAsync(orderId);
        }

        public async Task FinishUserOrders(string[] orderIds, string employeeId)
        {
            foreach (string orderId in orderIds)
            {
                await FinishUserOrder(orderId, employeeId);
            }
        }
    }
}
