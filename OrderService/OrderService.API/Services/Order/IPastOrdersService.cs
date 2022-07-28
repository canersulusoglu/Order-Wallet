namespace OrderService.API.Services.Orders
{
    public interface IPastOrdersService
    {
        public List<RoomOrder> GetUserPastOrders(string userEmail, int itemsPerPage, int pageNumber);
        public List<RoomOrder> GetAllPastOrders(int itemsPerPage, int pageNumber);
    }
}
