namespace OrderService.API.ViewModels
{
    public class RoomOrderViewModel
    {
        public string ConfirmedBasketUserId;

        public string RoomName { get; set; }

        public List<UserOrderViewModel> UserOrders { get; set; }
    }
}
