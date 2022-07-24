namespace OrderService.API.Services.Orders
{
    public interface ICurrentOrdersService
    {
        public Task<string[]> GetRoomNamesHaveAnOrder();
        public Task<string[]> GetRoomOrderIds(string roomName);
        public Task<string[]> GetUserOrderIds(string userId);
        public Task<string[]> GetOrderIds();
        public Task<UserOrderViewModel> GetUserOrder(string orderId);
        public Task<List<UserOrderViewModel>> GetUserCurrentOrders(string userId);
        public Task<List<UserOrderViewModel>> GetAllCurrentUserOrders();
        public Task<List<RoomOrderViewModel>> GetAllCurrentRoomOrders();
        public Task<List<UserOrderViewModel>> GetRoomOrders(string roomName);
        public Task FinishUserOrder(string orderId, string employeeId);
        public Task FinishUserOrders(string[] orderIds, string employeeId);
    }
}
