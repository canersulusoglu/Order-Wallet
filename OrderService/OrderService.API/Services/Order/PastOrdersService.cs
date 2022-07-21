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

        public List<Order> GetUserPastOrders(string userEmail)
        {
            IQueryable<Order> UserPastOrders = _orderRepository.FindByCondition(x => x.UserEmail == userEmail);
            return UserPastOrders.ToList();
        }

        public List<Order> GetAllPastOrders(int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            IQueryable<Order> PastOrders = _orderRepository.FindWithLimitAndOffset(itemsPerPage, offset);
            return PastOrders.ToList();
        }
    }
}
