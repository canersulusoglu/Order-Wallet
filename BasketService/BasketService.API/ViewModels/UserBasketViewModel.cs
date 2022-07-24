namespace BasketService.API.ViewModels
{
    public class UserBasketViewModel
    {
        public string UserId { get; set; }

        public List<UserBasketItemViewModel> UserBasketItems { get; set; }
    }
}
