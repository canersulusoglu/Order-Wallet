namespace OrderService.API.Models
{
    public class RoomOrder : BaseModel
    {
        public string? EmployeeEmail { get; set; }
        public string OrderId { get; set; }
        public string? OrderMessage { get; set; }
        public string ConfirmedBasketUserEmail { get; set; }
        public string ConfirmedBasketUserName { get; set; }
        public string RoomName { get; set; }
        public ICollection<UserOrder> Users { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
