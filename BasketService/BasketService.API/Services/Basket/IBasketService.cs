namespace BasketService.API.Services.Basket
{
    public interface IBasketService
    {
        Task<RoomBasketViewModel> GetBasket(string roomName);
        Task UpdateBasket(string roomName, RoomBasketViewModel roomBasketViewModel);
        Task DeleteBasket(string roomNames);
    }
}
