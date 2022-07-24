namespace OrderService.API.Services.Orders
{
    public class PastOrdersService : IPastOrdersService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public PastOrdersService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public List<Order> GetUserPastOrders(string userId, int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            List<Order> orders = _orderRepository.RepositoryContext.Include(x => x.OrderItems).Where(x => x.UserId == userId).Skip(offset).Take(itemsPerPage).OrderByDescending(x => x.CreatedTime).ToList();
            return orders;
        }

        public List<Order> GetAllPastOrders(int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            List<Order> orders =  _orderRepository.RepositoryContext.Include(x => x.OrderItems).Skip(offset).Take(itemsPerPage).OrderByDescending(x => x.CreatedTime).ToList();
            return orders;
        }
    }
}
