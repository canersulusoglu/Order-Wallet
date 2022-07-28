namespace BasketService.API.Services.Checkout
{
    public interface ICheckoutService
    {
        public Task confirmBasket(string basketUserEmail, string basketUserName, string orderMessage);
    }
}
