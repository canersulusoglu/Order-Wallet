namespace BasketService.API.Services.Basket
{
    public interface IBasketService
    {
        Task<RoomBasket> GetBasket(string basketUserEmail);
        Task UpdateBasket(string roomName, string confirmedBasketUserEmail, string confirmedBasketUserName, UpdateBasketViewModel basketUsers);
        Task DeleteBasket(string basketUserEmail);
    }
}
