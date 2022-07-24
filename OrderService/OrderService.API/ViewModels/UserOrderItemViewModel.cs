namespace OrderService.API.ViewModels
{
    public class UserOrderItemViewModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}
