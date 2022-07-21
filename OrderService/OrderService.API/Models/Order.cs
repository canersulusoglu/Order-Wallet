namespace OrderService.API.Models
{
    public class Order : BaseModel
    {
        public string RoomName { get; set; }

        public string UserEmail { get; set; }

        public string EmployeeEmail { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeSurname { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
