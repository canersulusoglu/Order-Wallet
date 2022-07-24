namespace OrderService.API.Models
{
    public class Order : BaseModel
    {
        public string UserId { get; set; }
        public string RoomName { get; set; }
        public string EmployeeId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
