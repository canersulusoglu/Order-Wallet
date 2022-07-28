namespace BasketService.API.Models
{
    public class UserBasket
    {
        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public List<UserBasketItem> Products { get; set; }
    }
}
