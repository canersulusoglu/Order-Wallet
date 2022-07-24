namespace OrderService.API.Services.Orders
{
    public interface IPastOrdersService
    {
        public List<Order> GetUserPastOrders(string userId, int itemsPerPage, int pageNumber);
        public List<Order> GetAllPastOrders(int itemsPerPage, int pageNumber);
    }
}
