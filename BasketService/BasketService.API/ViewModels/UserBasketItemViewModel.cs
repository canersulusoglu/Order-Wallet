namespace BasketService.API.ViewModels
{
    public class UserBasketItemViewModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }
    }
}
