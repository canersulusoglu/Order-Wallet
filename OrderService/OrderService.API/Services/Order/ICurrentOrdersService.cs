namespace OrderService.API.Services.Orders
{
    public interface ICurrentOrdersService
    {
        public Task<string[]> GetUserOrderIds(string userEmail);
        public Task<string[]> GetOrderIds();
        public Task<RoomOrder> GetUserOrder(string orderId);
        public Task<List<RoomOrder>> GetUserCurrentOrders(string userEmail);
        public Task<List<RoomOrder>> GetAllCurrentOrders();
        public Task FinishOrder(string orderId, string employeeEmail);
        public Task FinishOrders(string[] orderIds, string employeeEmail);
    }
}
