namespace OrderService.API.Services.Orders
{
    public interface ICurrentOrdersService
    {
        public List<RoomOrderViewModel> GetAllCurrentOrders();
        public List<UserOrderViewModel> GetUserCurrentOrders(string userEmail, string roomName);
        public RoomOrderViewModel GetRoomOrder(string roomName);
    }
}
