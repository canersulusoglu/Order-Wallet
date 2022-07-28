namespace OrderService.API.Models
{
    public class UserOrderItem : BaseModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public UserOrder? UserOrder { get; set; }
    }
}
