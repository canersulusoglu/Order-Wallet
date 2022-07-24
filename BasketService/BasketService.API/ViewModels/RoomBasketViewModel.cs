namespace BasketService.API.ViewModels
{
    public class RoomBasketViewModel
    {
        public string? ConfirmedBasketUserId;
        public string? RoomName { get; set; }
        public List<UserBasketViewModel> UserBaskets { get; set; }
    }
}