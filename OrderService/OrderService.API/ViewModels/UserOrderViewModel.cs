namespace OrderService.API.ViewModels
{
    public class UserOrderViewModel
    {
        public string? OrderId { get; set; }

        public string UserId { get; set; }

        public string CreatedOrderUserId { get; set; }

        public string RoomName { get; set; }

        public List<UserOrderItemViewModel> Products { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
