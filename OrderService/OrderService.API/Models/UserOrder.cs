namespace OrderService.API.Models
{
    public class UserOrder : BaseModel
    {
        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public ICollection<UserOrderItem> Products { get; set; }

        public RoomOrder? RoomOrder { get; set; }
    }
}
