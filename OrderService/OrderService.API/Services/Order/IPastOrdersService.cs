namespace OrderService.API.Services.Orders
{
    public interface IPastOrdersService
    {
        public List<Order> GetUserPastOrders(string userEmail);
        public List<Order> GetAllPastOrders(int itemsPerPage, int pageNumber);
    }
}
