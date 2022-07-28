namespace OrderService.API.Services.Orders
{
    public class PastOrdersService : IPastOrdersService
    {
        private readonly IRoomOrderRepository _orderRepository;
        private readonly IUserOrderItemRepository _orderItemRepository;

        public PastOrdersService(IRoomOrderRepository orderRepository, IUserOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public List<RoomOrder> GetUserPastOrders(string userEmail, int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            List<RoomOrder> orders = _orderRepository.RepositoryContext.Include(x => x.Users).ThenInclude(x => x.Products).Where(x => x.Users.Any(a => a.UserEmail == userEmail)).Skip(offset).Take(itemsPerPage).OrderByDescending(x => x.CreatedTime).ToList();
            return orders;
        }

        public List<RoomOrder> GetAllPastOrders(int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            List<RoomOrder> orders =  _orderRepository.RepositoryContext.Include(x => x.Users).ThenInclude(x => x.Products).Skip(offset).Take(itemsPerPage).OrderByDescending(x => x.CreatedTime).ToList();
            return orders;
        }
    }
}
