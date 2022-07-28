namespace BasketService.API.Models
{
    public class UserBasketItem
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}
