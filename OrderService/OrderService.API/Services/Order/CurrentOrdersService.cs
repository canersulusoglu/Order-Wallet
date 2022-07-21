namespace OrderService.API.Services.Orders
{
    public class CurrentOrdersService : ICurrentOrdersService
    {
        private readonly ICacheOrderRepository<RoomOrderViewModel> _cacheOrderRepository;

        public CurrentOrdersService(ICacheOrderRepository<RoomOrderViewModel> cacheOrderRepository)
        {
            _cacheOrderRepository = cacheOrderRepository;
        }

        public List<UserOrderViewModel> GetUserCurrentOrders(string userEmail, string roomName)
        {
            RoomOrderViewModel? roomOrder = _cacheOrderRepository.GetStringKey(roomName);
            if (roomOrder == null)
            {
                throw new OrderNotFoundException();
            }
            return roomOrder.UserOrders.FindAll(x => x.UserEmail == userEmail);
        }

        public List<RoomOrderViewModel> GetAllCurrentOrders()
        {
            List<RoomOrderViewModel> roomOrders = new List<RoomOrderViewModel>();
            List<string> allOrdersKeys = _cacheOrderRepository.ScanKeys("*");
            foreach (string roomOrderKey in allOrdersKeys)
            {
                RoomOrderViewModel roomOrder = _cacheOrderRepository.GetStringKey(roomOrderKey);
                roomOrders.Add(roomOrder);
            }
            return roomOrders;
        }

        public RoomOrderViewModel GetRoomOrder(string roomName)
        {
            RoomOrderViewModel? roomOrder = _cacheOrderRepository.GetStringKey(roomName);
            if (roomOrder == null)
            {
                throw new OrderNotFoundException();
            }
            return roomOrder;
        }
    }
}
