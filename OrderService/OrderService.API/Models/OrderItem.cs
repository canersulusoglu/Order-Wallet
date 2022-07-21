namespace OrderService.API.Models
{
    public class OrderItem : BaseModel
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public int PaymentQuantity { get; set; }

        public Order Order { get; set; }
    }
}
