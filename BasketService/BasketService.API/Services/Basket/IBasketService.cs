namespace BasketService.API.Services.Basket
{
    public interface IBasketService
    {
        Task<RoomBasketViewModel> GetBasket(string roomName);
        Task<RoomBasketViewModel> UpdateBasket(RoomBasketViewModel roomBasketViewModel);
        Task<RoomBasketViewModel> DeleteBasket(string roomNames);
    }
}
